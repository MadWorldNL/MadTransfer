using System.Net.Http.Headers;
using System.Text;
using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;
using Microsoft.Extensions.DependencyInjection;

namespace MadWorldNL.MadTransfer.Endpoints.File;

public sealed class GetDownloadTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Get_WhenGivenAnId_ReturnsFile()
    {
        // Arrange
        const string id = "MDU4Y2I2YWEtZjA4My00Njg5LWFmNTUtODMxZWI4YzlhYTMy";

        await SetupCorrectUserFile();
        using var client = factory.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", ApiWebApplicationFactory.GetJwtToken());
        
        // Act
        var response = await client.GetAsync($"/File/Download?id={id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.MediaType.ShouldBe("text/plain");
        
        var contentDisposition = response.Content.Headers.ContentDisposition;
        contentDisposition.ShouldNotBeNull();
        contentDisposition!.FileNameStar.ShouldBe("download_test_file.txt");
        
        var stream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        content.ShouldBe("Hello, Stream!");
    }
    
    private async Task SetupCorrectUserFile()
    {
        var fileId = new FileId(Guid.Parse("76cbd313-7889-4ee2-ab9b-42d0c5b605f2"));
        var metaData = FileMetaData.Create("download_test_file.txt", "674c33d7-171e-4b25-aff9-0ffc64f408a78", ".txt", 14);
        var hyperLink = Hyperlink.Create(Guid.Parse("058cb6aa-f083-4689-af55-831eb8c9aa32"));
        var userId = new UserId(Guid.Parse("2ce326c2-e9c9-4a12-9e50-08c2b1ea6493"));
        var userFile = new UserFile(fileId, metaData.ThrowIfFail(), hyperLink, userId);

        using var scope = factory.Server.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MadTransferContext>();
        context.Files.Add(userFile);
        await context.SaveChangesAsync();
        
        var fileStorage = scope.ServiceProvider.GetRequiredService<IFileStorage>();
        const string text = "Hello, Stream!";
        var byteArray = Encoding.UTF8.GetBytes(text);
        var stream = new MemoryStream(byteArray);
        
        await fileStorage.Upload(userFile.MetaData, new FilePath("userfiles"), stream);
    }
}