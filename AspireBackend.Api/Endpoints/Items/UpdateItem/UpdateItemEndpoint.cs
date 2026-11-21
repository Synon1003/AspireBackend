using Microsoft.EntityFrameworkCore;
using AspireBackend.Api.Data;
using AspireBackend.Api.Data.Entities;

namespace AspireBackend.Api.Endpoints.Items.UpdateItem;

public static class UpdateItemEndpoint
{
    public static void MapUpdateItem(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", async (
            Guid id,
            UpdateItemDto itemDto,
            ApplicationDbContext dbContext) =>
        {
            Item? item = await dbContext.Items
                .FirstOrDefaultAsync(item => item.Id == id);
            if (item is not null)
            {
                item.Name = itemDto.Name;
                item.CategoryId = itemDto.CategoryId;
                item.Price = itemDto.Price;
                item.ReleaseDate = itemDto.ReleaseDate;
                item.Description = itemDto.Description;
                item.LastUpdated = DateTime.UtcNow;
                
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }

            else return Results.NotFound();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }
}