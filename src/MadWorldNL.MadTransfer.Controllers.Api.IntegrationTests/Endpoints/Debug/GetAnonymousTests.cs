namespace MadWorldNL.MadTransfer.Endpoints.Debug;

public sealed class GetAnonymousTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Get_WhenGivenAnEmptyRequest_ReturnsOk()
    {
        // Arrange
        using var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/Debug/Anonymous");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("\"OK\"");
    }
}