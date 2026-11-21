using AspireBackend.Api.Data;

namespace AspireBackend.Api.Endpoints.Categories.GetCategories;

public static class GetCategoriesEndpoint
{
    public static void MapGetCategories(this IEndpointRouteBuilder app)
    {
        // GET /categories
        app.MapGet("/", async (ApplicationDbContext dbContext) =>
        {
            List<CategoryDto> categoryDtos = dbContext.Categories
                .Select(category => new CategoryDto(category.Id, category.Name))
                .ToList();

            return Results.Ok(categoryDtos);
        })
        .Produces<List<CategoryDto>>();
    }
}