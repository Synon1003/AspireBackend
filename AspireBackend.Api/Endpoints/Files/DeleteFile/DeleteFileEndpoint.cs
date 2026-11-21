using Microsoft.AspNetCore.Http.HttpResults;
using AspireBackend.Api.Services.Storage;

namespace AspireBackend.Api.Endpoints.Files.DeleteFile;

public static class DeleteFileEndpoint
{
    public static void MapDeleteFile(this IEndpointRouteBuilder app)
    {
        // DELETE /files
        app.MapDelete("/{fileId}", async (
            Guid fileId,
            IBlobService blobService) =>
        {
            await blobService.DeleteAsync(fileId);

            return Results.NoContent();
        })
        .Produces<FileStreamHttpResult>(StatusCodes.Status204NoContent);
    }
}