using System.Net.Http.Headers;

namespace MadWorldNL.MadTransfer.Endpoints.File;

public sealed class PostUploadTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Post_WhenHasFile_ThenSaveFile()
    {
        // Arrange
        using var client = factory.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", factory.GetJwtToken());
        
        // Prepare file content (for example, from memory or disk)
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("File content here"));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent, "file", "testfile.txt");
        
        // Act
        var response = await client.PostAsync("/File/Upload", multipartContent);
        
        // Assert
        response.EnsureSuccessStatusCode();
        //TODO: Real Assert
    }
}