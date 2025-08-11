using JetBrains.Annotations;
using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Databases;
using MadWorldNL.MadTransfer.Endpoints;
using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.Upload;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

const string allowedCors = nameof(allowedCors);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var authenticationSettings = builder.Configuration
    .GetRequiredSection(AuthenticationSettings.Key)
    .Get<AuthenticationSettings>()!;

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

// TODO: Move use-cases
builder.Services.AddScoped<UploadUserFileUseCase>();

// TODO: Move Database & Storage
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

var app = builder.Build();

app.UseCors(allowedCors);
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapHealthChecks("/healthz");
app.AddFileEndpoints();
app.AddDebugEndpoints();

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