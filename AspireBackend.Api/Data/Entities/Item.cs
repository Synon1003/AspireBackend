using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireBackend.Api.Data.Entities;

public class Item
{
    public Guid Id { get; set; }
    [Required, MaxLength(50)]
    public required string Name { get; set; }
    [Required, MaxLength(500)]
    public required string Description { get; set; }

    [Range(1, 10000), Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required DateTime LastUpdated { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }
}