namespace MadWorldNL.MadTransfer.Endpoints;

internal static class DebugEndpoints
{
    internal static void AddDebugEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/Debug")
            .DisableAntiforgery();

        endpoints.MapGet("/Authorization", Results.NoContent).RequireAuthorization();
    }
}