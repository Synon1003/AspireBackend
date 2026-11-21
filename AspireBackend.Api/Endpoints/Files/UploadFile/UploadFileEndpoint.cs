using AspireBackend.Api.Services.Storage;

namespace AspireBackend.Api.Endpoints.Files.UploadFile;

public static class UploadFileEndpoint
{
    public static void MapUploadFile(this IEndpointRouteBuilder app)
    {
        // POST /files
        app.MapPost("/", async (
            IFormFile file,
            IBlobService blobService) =>
        {
            using Stream stream = file.OpenReadStream();
            Guid fileId;

            try
            {
                fileId = await blobService.UploadAsync(stream, file.ContentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Results.BadRequest();
            }
            
            return Results.Ok(fileId);
        })
        .DisableAntiforgery()
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

    }
}