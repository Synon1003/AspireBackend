using AspireBackend.Api.Endpoints.Files.UploadFile;
using AspireBackend.Api.Endpoints.Files.DownloadFile;
using AspireBackend.Api.Endpoints.Files.DeleteFile;

namespace AspireBackend.Api.Endpoints.Files;

public static class FilesEndpoints
{
    public static void MapFiles(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/files");

        group.MapUploadFile();
        group.MapDownloadFile();
        group.MapDeleteFile();
    }
}