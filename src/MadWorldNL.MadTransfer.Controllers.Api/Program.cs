using System.Threading.RateLimiting;
using JetBrains.Annotations;
using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Databases;
using MadWorldNL.MadTransfer.Endpoints;
using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.Download;
using MadWorldNL.MadTransfer.Files.GetInfo;
using MadWorldNL.MadTransfer.Files.Upload;
using MadWorldNL.MadTransfer.Identities;
using MadWorldNL.MadTransfer.Status;
using MadWorldNL.MadTransfer.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serilog;

const string allowedCors = nameof(allowedCors);

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("SerilogSettings:Active"))
{
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));   
}

var authenticationSettings = builder.Configuration
    .GetRequiredSection(AuthenticationSettings.Key)
    .Get<AuthenticationSettings>()!;

builder.Services.AddSingleton(Options.Create(authenticationSettings));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authenticationSettings.Authority; // Keycloak realm URL
        options.Audience = authenticationSettings.Audience; // Match the "Client ID" of your Keycloak client
        options.RequireHttpsMetadata = false; // Only for dev/local

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = authenticationSettings.ValidateUser,
            ValidateAudience = authenticationSettings.ValidateUser,
            ValidateLifetime = authenticationSettings.ValidateUser,
            ValidateIssuerSigningKey = authenticationSettings.ValidateUser,
            NameClaimType = "preferred_username",
            RoleClaimType = "roles"
        };

        if (!authenticationSettings.ValidateUser)
        {
            options.TokenValidationParameters.SignatureValidator = (token, _) =>
            {
                // Just return the token without validating signature
                var handler = new JsonWebTokenHandler();
                return handler.ReadJsonWebToken(token);
            };
        }
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Logger.Information("Authentication failed: {ContextException}", context.Exception);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Log.Logger.Information("Token validated for: {IdentityName}", context.Principal?.Identity?.Name ?? "Unknown");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        const string retryAfterSeconds = "60";
        
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;
        
        await context.HttpContext.Response.WriteAsync(
            $"{{\"error\":\"Too Many Requests\",\"message\":\"Try again in {retryAfterSeconds} seconds.\"}}", token);
    };
    
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.GetIpAddress();

        return RateLimitPartition.GetTokenBucketLimiter(ip, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 100,
            QueueLimit = 0,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1),
            TokensPerPeriod = 100,
            AutoReplenishment = true,
        });
    });
    
    options.AddPolicy(RateLimiterNames.PerUserPolicy, httpContext =>
    {
        var userId = httpContext.User.GetUserId();

        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(10),
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowedCors, policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddAntiforgery();

var databaseSettings = builder.Configuration
    .GetRequiredSection(DatabaseSettings.Key)
    .Get<DatabaseSettings>()!;

builder.Services.AddDbContextPool<MadTransferContext>(opt =>
    opt.UseNpgsql(
        databaseSettings.ConnectionString,
        o => o
            .SetPostgresVersion(17, 0)
            .UseNodaTime()));

builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection(StorageSettings.Key));

builder.Services.AddHttpClient();

// TODO: Move use-cases
builder.Services.AddScoped<GetInfoUserFileUseCase>();
builder.Services.AddScoped<DownloadUserFileUseCase>();
builder.Services.AddScoped<UploadUserFileUseCase>();

builder.Services.AddScoped<CheckStatusUseCase>();

// TODO: Move Database & Storage
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusStorage, StatusStorage>();
builder.Services.AddScoped<IStatusIdentity, StatusIdentity>();

var app = builder.Build();

app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseCors(allowedCors);
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapHealthChecks("/healthz");
app.AddDebugEndpoints();
app.AddFileEndpoints();
app.AddStatusEndpoints();

app.Services.MigrateDatabase<MadTransferContext>();

app.Run();

/// <summary>
/// Exposes the application's entry point so that integration tests
/// can create a <see cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory{TEntryPoint}"/>
/// using <see cref="Program"/> as the entry point type.
/// </summary>
[UsedImplicitly]
public partial class Program
{
}