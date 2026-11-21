using Microsoft.EntityFrameworkCore;
using AspireBackend.Api.Data;

namespace AspireBackend.Api.Endpoints.Items.GetItems;

public static class GetItemsEndpoint
{
    public static void MapGetItems(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            ApplicationDbContext dbContext,
            [AsParameters] GetItemsDto request) =>
        {
            var skipCount = (request.PageNumber - 1) * request.PageSize;
            var filteredItems = dbContext.Items
                .AsNoTracking()
                .Where(item => string.IsNullOrWhiteSpace(request.Name) || item.Name.Contains(request.Name ?? ""))
                .OrderBy(item => item.Name)
                .Skip(skipCount)
                .Take(request.PageSize)
                .Include(item => item.Category)
                .ToList();

            var itemsOnPage = filteredItems.Select(item => new ItemSummaryDto(
                item.Id,
                item.Name,
                item.Category!.Name,
                item.Price,
                item.ReleaseDate,
                item.LastUpdated
            )).ToList();

            var totalItems = dbContext.Items.AsNoTracking()
                .Count(item => string.IsNullOrWhiteSpace(request.Name) || item.Name.Contains(request.Name ?? ""));
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return Results.Ok(new ItemsPageDto(totalPages, itemsOnPage));
        })
        .Produces<ItemsPageDto>();
    }
}