using AspireBackend.Api.Endpoints.Items.DeleteItem;
using AspireBackend.Api.Endpoints.Items.UpdateItem;
using AspireBackend.Api.Endpoints.Items.CreateItem;
using AspireBackend.Api.Endpoints.Items.GetItem;
using AspireBackend.Api.Endpoints.Items.GetItems;

namespace AspireBackend.Api.Endpoints.Items;

public static class ItemsEndpoints
{
    public static void MapItems(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/items");

        group.MapGetItems();
        group.MapGetItem();
        group.MapCreateItem();
        group.MapUpdateItem();
        group.MapDeleteItem();
    }
}