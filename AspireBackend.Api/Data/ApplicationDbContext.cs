using AspireBackend.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspireBackend.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .HasOne(item => item.Category)
            .WithMany()
            .HasForeignKey(item => item.CategoryId);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        Category actionCategory = new Category { Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Action" };
        Category isekaiCategory = new Category { Id = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Isekai" };
        Category fantasyCategory = new Category { Id = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Fantasy" };

        modelBuilder.Entity<Category>().HasData(
            actionCategory, isekaiCategory, fantasyCategory
        );

        modelBuilder.Entity<Item>().HasData(
            new Item
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Name = "Solo Leveling",
                CategoryId = actionCategory.Id,
                Price = 54.99m,
                ReleaseDate = new DateOnly(2024, 1, 7),
                Description = "Dark fantasy action series following weak hunter Sung Jin-woo as he gains a unique system to level up without limits",
                LastUpdated = new DateTime(2026, 12, 1, 10, 0, 0, DateTimeKind.Utc),
            },
            new Item
            {
                Id = new Guid("00000000-0000-0000-0000-000000000005"),
                Name = "The Water Magician",
                CategoryId = isekaiCategory.Id,
                Price = 44.99m,
                ReleaseDate = new DateOnly(2025, 7, 4),
                Description = "Isekai fantasy following Ryo Mihara, who reincarnates in another world with water magic and eternal youth",
                LastUpdated = new DateTime(2026, 12, 1, 10, 0, 0, DateTimeKind.Utc),
            },
            new Item
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Name = "Frieren: Beyond Journey's End",
                CategoryId = fantasyCategory.Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2023, 9, 29),
                Description = "Fantasy adventure series following an elven mage reflecting on time, mortality, and companionship",
                LastUpdated = new DateTime(2026, 12, 1, 10, 0, 0, DateTimeKind.Utc),
            }
        );
    }
}