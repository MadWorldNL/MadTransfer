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

            if (builder.Environment.IsProduction())
            {
                options.AddOtlpExporter(exporter =>
                {
                    exporter.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
                    exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                    exporter.Headers = "X-Seq-ApiKey=abcde12345";
                });
            }
        });

        if (builder.Environment.IsDevelopment())
        {
            openTelemetryBuilder.UseOtlpExporter();
        }
    }
}