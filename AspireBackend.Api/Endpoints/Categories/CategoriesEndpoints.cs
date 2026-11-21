using AspireBackend.Api.Endpoints.Categories.GetCategories;

namespace AspireBackend.Api.Endpoints.Categories;

public static class CategoriesEndpoints
{
    public static void MapCategories(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/categories");

        group.MapGetCategories();
    }
}