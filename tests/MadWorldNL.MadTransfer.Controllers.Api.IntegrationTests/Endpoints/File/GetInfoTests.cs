using System.Net.Http.Headers;
using System.Net.Http.Json;
using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.GetInfo;
using MadWorldNL.MadTransfer.Users;
using MadWorldNL.MadTransfer.Web;
using Microsoft.Extensions.DependencyInjection;

namespace MadWorldNL.MadTransfer.Endpoints.File;

public sealed class GetInfoTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Get_WhenGivenAnId_ReturnsInfoAboutFile()
    {
        // Arrange
        const string id = "YTA4MDRkN2UtNDY4ZC00NmE2LThmYWYtMWFjOWJhZDQ0ZGZh";
        
        SetupCorrectUserFile();
        using var client = factory.CreateClient();
        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", ApiWebApplicationFactory.GetJwtToken());
        
        // Act
        var response = await client.GetAsync($"/File/Info?id={id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<GetInfoUserFileResult>();
        content.ShouldNotBeNull();
        content.Id.ShouldBe(id);
        content.FileName.ShouldBe("test.txt");
        content.FileSize.ShouldBe(100);
    }

    private void SetupCorrectUserFile()
    {
        var fileId = new FileId(Guid.Parse("3fd35c30-4d8c-4b64-b924-7dd1f865cb14"));
        var metaData = FileMetaData.Create("test.txt", "5664a54f-0465-4727-a646-bd3f1c749018", ".txt", 100);
        var hyperLink = Hyperlink.Create(Guid.Parse("a0804d7e-468d-46a6-8faf-1ac9bad44dfa"));
        var userId = new UserId(Guid.Parse("1cfc74c6-5f8a-4f23-899c-ab7b1911eaa3"));
        var userFile = new UserFile(fileId, metaData.ThrowIfFail(), hyperLink, userId);

        using var scope = factory.Server.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MadTransferContext>();
        context.Files.Add(userFile);
        context.SaveChanges();
    }
}