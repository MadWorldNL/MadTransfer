using System.Net.Http.Headers;

namespace MadWorldNL.MadTransfer.Endpoints.Debug;

public sealed class GetAuthorizationTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Get_WhenHasAuthorization_ReturnsOk()
    {
        // Arrange
        using var client = factory.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", ApiWebApplicationFactory.GetJwtToken());
        
        // Act
        var response = await client.GetAsync("/Debug/Authorization");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("\"OK\"");
    }
}