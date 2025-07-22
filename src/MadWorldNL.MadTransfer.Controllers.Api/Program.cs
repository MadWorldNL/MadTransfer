using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Databases;
using MadWorldNL.MadTransfer.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5555/realms/madworld"; // Keycloak realm URL
        options.Audience = "mad-transfer-api"; // Match the "Client ID" of your Keycloak client
        options.RequireHttpsMetadata = false; // Only for dev/local

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "preferred_username",
            RoleClaimType = "roles"
        };
    });

builder.Services.AddAuthorization();

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