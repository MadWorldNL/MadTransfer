using System.Net.Http.Json;
using MadWorldNL.MadTransfer.Status;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace MadWorldNL.MadTransfer.Endpoints.Status;

public class GetStatusTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Get_WhenGivenEmptyRequest_ReturnsStatusForServices()
    {
        // Arrange
        var identityServer = factory.StartIdentityServer();
        identityServer.Given(Request.Create()
                .WithPath("/")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody("{ \"message\": \"Hello from WireMock.Net!\" }"));
        
        using var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/Status");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<StatusResponse>();
        content.ShouldNotBeNull();
        content.IsAlive.ShouldBeTrue();
        content.IsDatabaseAlive.ShouldBeTrue();
        content.IsIdentityServerAlive.ShouldBeTrue();
        content.IsStorageAlive.ShouldBeTrue();
    }
}