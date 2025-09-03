using MadWorldNL.MadTransfer.Configurations;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MadWorldNL.MadTransfer.Builder;

internal static class OpenTelemetryExtensions
{
    internal static void AddDefaultOpenTelemetry(this WebApplicationBuilder builder)
    {
        var openTelemetrySettings = builder.Configuration
            .GetRequiredSection(OpenTelemetrySettings.Key)
            .Get<OpenTelemetrySettings>()!;
        
        var openTelemetryBuilder = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddNpgsql())
            .WithMetrics(metrics => metrics
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation());

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            
            if (openTelemetrySettings.Mode == OpenTelemetryMode.FullStack)
            {                                                       
                options.AddOtlpExporter(exporter =>       
                {
                    exporter.Endpoint = new Uri($"{openTelemetrySettings.Seq.Url}/ingest/otlp/v1/logs");
                    exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                    exporter.Headers = $"X-Seq-ApiKey={openTelemetrySettings.Seq.ApiKey}";
                });
            }
        });

        if (openTelemetrySettings.Mode == OpenTelemetryMode.Aspire)
        {
            openTelemetryBuilder.UseOtlpExporter();
        }
    }
}