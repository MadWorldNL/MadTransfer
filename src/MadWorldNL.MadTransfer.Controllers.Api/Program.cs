using JetBrains.Annotations;
using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Builder;
using MadWorldNL.MadTransfer.Endpoints;
using MadWorldNL.MadTransfer.Web;
using Serilog;

const string allowedCors = nameof(allowedCors);

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("SerilogSettings:Active"))
{
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));   
}

builder.AddDefaultOpenTelemetry();  
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

builder.AddApplication();

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

await app.RunAsync();

/// <summary>
/// Exposes the application's entry point so that integration tests
/// can create a <see cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory{TEntryPoint}"/>
/// using <see cref="Program"/> as the entry point type.
/// </summary>
[UsedImplicitly]
public partial class Program
{
}