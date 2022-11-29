using Categories.Domain;
using Categories.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Categories.Tests.Common;

public class CategoriesContextFactory
{
    public static Guid CategoryIdForDelete = Guid.NewGuid();
    public static Guid CategoryIdForUpdate = Guid.NewGuid();

    public static CategoriesDbContext Create()
    {
        var options = new DbContextOptionsBuilder<CategoriesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new CategoriesDbContext(options);
        context.Database.EnsureCreated();

        context.Categories.AddRange(
            new Category
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "One"
            },
            new Category
            {
                Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Name = "Two"
            },
            new Category
            {
                Id = CategoryIdForDelete,
                Name = "Three"
            },
            new Category
            {
                Id = CategoryIdForUpdate,
                Name = "Four"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Five"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Six"
            });

        context.Items.AddRange(
            new Item
            {
                Id = Guid.NewGuid(),
                CategoryId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Lorem Ipsum"
            },
            new Item
            {
                Id = Guid.NewGuid(),
                CategoryId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Nam vel nulla"
            },
            new Item
            {
                Id = Guid.NewGuid(),
                CategoryId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Name = "Pellentesque quam tortor"
            }
        );
        context.SaveChanges();
        return context;
    }

    public static void Destroy(CategoriesDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
