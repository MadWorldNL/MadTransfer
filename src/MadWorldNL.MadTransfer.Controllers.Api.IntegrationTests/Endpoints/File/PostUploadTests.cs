using System.Net.Http.Headers;
using System.Net.Http.Json;
using MadWorldNL.MadTransfer.Files;
using Xunit.Abstractions;

namespace MadWorldNL.MadTransfer.Endpoints.File;

public sealed class PostUploadTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public PostUploadTests(ApiWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _factory.SetOutputHelper(testOutputHelper);
    }
    
    [Fact]
    public async Task Post_WhenHasFile_ThenSaveFile()
    {
        // Arrange
        using var client = _factory.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", _factory.GetJwtToken());
        
        // Prepare file content (for example, from memory or disk)
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("File content here"));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent, "file", "testfile.txt");
        
        // Act
        var response = await client.PostAsync("/File/Upload", multipartContent);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<UploadResponse>();
        content.ShouldNotBeNull();
        content.DownloadUrl.ShouldNotBeNull();
    }
}