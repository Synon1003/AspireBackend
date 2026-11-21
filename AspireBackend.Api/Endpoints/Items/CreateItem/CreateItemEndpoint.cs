using AspireBackend.Api.Data;
using AspireBackend.Api.Data.Entities;
using AspireBackend.Api.Endpoints.Items.Constants;

namespace AspireBackend.Api.Endpoints.Items.CreateItem;

public static class CreateItemEndpoint
{
    public static void MapCreateItem(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            CreateItemDto itemDto,
            ApplicationDbContext dbContext) =>
        {
            var item = new Item
            {
                Name = itemDto.Name,
                CategoryId = itemDto.CategoryId,
                Price = itemDto.Price,
                ReleaseDate = itemDto.ReleaseDate,
                Description = itemDto.Description,
                LastUpdated = DateTime.UtcNow
            };

            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute(
                EndpointNames.GetItem,
                new { id = item.Id },
                new ItemDetailsDto(
                    item.Id,
                    item.Name,
                    item.CategoryId,
                    item.Price,
                    item.ReleaseDate,
                    item.Description,
                    item.LastUpdated
                ));
        })
        .Produces<ItemDetailsDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }
}