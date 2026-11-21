using Microsoft.AspNetCore.Http.HttpResults;
using AspireBackend.Api.Services.Storage;
using Azure;

namespace AspireBackend.Api.Endpoints.Files.DownloadFile;

public static class DownloadFileEndpoint
{
    public static void MapDownloadFile(this IEndpointRouteBuilder app)
    {
        // GET /files
        app.MapGet("/{fileId}", async (
            Guid fileId,
            IBlobService blobService) =>
        {
            FileResponse fileResponse;
            try
            {
                fileResponse = await blobService.DownloadAsync(fileId);
            }
            catch(RequestFailedException ex)
            {
                if (ex.Status == 404)
                    return Results.NotFound("The specified blob does not exist");

                return Results.InternalServerError(ex.Message);
            }

            return Results.File(fileResponse.stream, fileResponse.ContentType);
        })
        .Produces<FileStreamHttpResult>(StatusCodes.Status200OK)
        .Produces<FileStreamHttpResult>(StatusCodes.Status404NotFound)
        .Produces<FileStreamHttpResult>(StatusCodes.Status500InternalServerError);
    }
}