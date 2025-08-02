namespace MadWorldNL.MadTransfer.Endpoints;

internal static class DebugEndpoints
{
    internal static void AddDebugEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/Debug")
            .DisableAntiforgery();

        endpoints.MapGet("/Authorization", () => Results.Ok("OK")).RequireAuthorization();

        endpoints.MapGet("/Anonymous", () => Results.Ok("OK"));
    }
}