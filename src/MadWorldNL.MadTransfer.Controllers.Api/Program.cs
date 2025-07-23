using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Databases;
using MadWorldNL.MadTransfer.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

const string allowedCors = nameof(allowedCors);

var builder = WebApplication.CreateBuilder(args);

var authenticationSettings = builder.Configuration
    .GetRequiredSection(AuthenticationSettings.Key)
    .Get<AuthenticationSettings>()!;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authenticationSettings.Authority; // Keycloak realm URL
        options.Audience = authenticationSettings.Authority; // Match the "Client ID" of your Keycloak client
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

builder.Services.AddDbContextPool<MadTransferContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("MadTransferContext"),
        o => o
            .SetPostgresVersion(17, 0)
            .UseNodaTime()));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(allowedCors);

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