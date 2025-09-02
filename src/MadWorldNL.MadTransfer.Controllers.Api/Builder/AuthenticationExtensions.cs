using MadWorldNL.MadTransfer.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MadWorldNL.MadTransfer.Builder;

public static class AuthenticationExtensions
{
    public static void AddDefaultAuthentication(this WebApplicationBuilder builder)
    {
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
                        Log.Logger.Information("Token validated for: {IdentityName}",
                            context.Principal?.Identity?.Name ?? "Unknown");
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
    }
}