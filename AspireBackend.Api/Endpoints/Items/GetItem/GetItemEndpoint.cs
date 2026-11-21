using Microsoft.EntityFrameworkCore;
using AspireBackend.Api.Data;
using AspireBackend.Api.Data.Entities;
using AspireBackend.Api.Endpoints.Items.Constants;

namespace AspireBackend.Api.Endpoints.Items.GetItem;

public static class GetItemEndpoint
{
    public static void MapGetItem(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", async (
            Guid id,
            ApplicationDbContext dbContext) =>
        {
            Item? item = await dbContext.Items.AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id);

            if (item is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new ItemDetailsDto(
                item.Id,
                item.Name,
                item.CategoryId,
                item.Price,
                item.ReleaseDate,
                item.Description,
                item.LastUpdated
            ));
        })
        .WithName(EndpointNames.GetItem)
        .Produces<ItemDetailsDto>()
        .Produces(StatusCodes.Status404NotFound);
    }
}