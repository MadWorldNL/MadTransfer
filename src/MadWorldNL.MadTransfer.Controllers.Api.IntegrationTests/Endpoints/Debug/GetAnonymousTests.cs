using MadWorldNL.MadTransfer.Common;
using Shouldly;

namespace MadWorldNL.MadTransfer.Endpoints.Debug;

public class GetAnonymousTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Get_WhenGivenAnEmptyRequest_ReturnsOk()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/Debug/Anonymous");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("OK");
    }
}