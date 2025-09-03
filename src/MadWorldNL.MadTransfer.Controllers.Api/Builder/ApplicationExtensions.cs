using MadWorldNL.MadTransfer.Files;
using MadWorldNL.MadTransfer.Files.Download;
using MadWorldNL.MadTransfer.Files.GetInfo;
using MadWorldNL.MadTransfer.Files.Upload;
using MadWorldNL.MadTransfer.Status;

namespace MadWorldNL.MadTransfer.Builder;

internal static class ApplicationExtensions
{
    internal static void AddApplication(this WebApplicationBuilder builder)
    {
        builder.AddFunctions();
        builder.AddDatabase();
        builder.AddStorages();
        builder.AddKeyCloak();
    }
    
    private static void AddFunctions(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetInfoUserFileUseCase>();
        builder.Services.AddScoped<DownloadUserFileUseCase>();
        builder.Services.AddScoped<UploadUserFileUseCase>();

        builder.Services.AddScoped<CheckStatusUseCase>();
    }
    
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileRepository, FileRepository>();
        builder.Services.AddScoped<IStatusRepository, StatusRepository>();
    }

    private static void AddStorages(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileStorage, FileStorage>();
        builder.Services.AddScoped<IStatusStorage, StatusStorage>();
    }

    private static void AddKeyCloak(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IStatusIdentity, StatusIdentity>();
    }
}