using System.ComponentModel.DataAnnotations;

namespace AspireBackend.Api.Endpoints.Items.CreateItem;

public record CreateItemDto(
    [Required][StringLength(50)] string Name,
    Guid CategoryId,
    [Range(1, 10000)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);

public record ItemDetailsDto(
    Guid Id,
    string Name,
    Guid CategoryId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    DateTime LastUpdated);