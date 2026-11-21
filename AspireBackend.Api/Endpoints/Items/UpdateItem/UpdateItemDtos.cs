using System.ComponentModel.DataAnnotations;

namespace AspireBackend.Api.Endpoints.Items.UpdateItem;

public record UpdateItemDto(
    [Required][StringLength(50)] string Name,
    [Required] Guid CategoryId,
    [Range(1, 10000)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);