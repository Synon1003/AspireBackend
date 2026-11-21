namespace AspireBackend.Api.Endpoints.Items.GetItems;

public record GetItemsDto(
    int PageNumber = 1,
    int PageSize = 5,
    string? Name = null);

public record ItemsPageDto(int TotalPages, List<ItemSummaryDto> Data);

public record ItemSummaryDto(
    Guid Id,
    string Name,
    string Category,
    decimal Price,
    DateOnly ReleaseDate,
    DateTime LastUpdated
);