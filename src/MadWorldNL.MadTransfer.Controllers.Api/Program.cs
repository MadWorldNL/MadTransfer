using System.Threading.RateLimiting;
using JetBrains.Annotations;
using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Builder;
using MadWorldNL.MadTransfer.Configurations;
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
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

const string allowedCors = nameof(allowedCors);

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("SerilogSettings:Active"))
{
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));   
}

builder.AddOpenTelemetryForDevelopment();
builder.AddDefaultAuthentication();
builder.AddDefaultRateLimiter();
builder.AddDefaultCors(allowedCors);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddAntiforgery();
builder.AddDefaultDatabase(); 

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