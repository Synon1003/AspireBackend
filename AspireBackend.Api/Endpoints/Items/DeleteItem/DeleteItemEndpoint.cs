using Microsoft.EntityFrameworkCore;
using AspireBackend.Api.Data;
using AspireBackend.Api.Data.Entities;

namespace AspireBackend.Api.Endpoints.Items.DeleteItem;

public static class DeleteItemEndpoint
{
    public static void MapDeleteItem(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", async (
            Guid id,
            ApplicationDbContext dbContext) =>
        {
            Item? item = await dbContext.Items
                .FirstOrDefaultAsync(item => item.Id == id);
                
            if (item is not null)
            {
                dbContext.Items.Remove(item);
                await dbContext.SaveChangesAsync();
            }

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}