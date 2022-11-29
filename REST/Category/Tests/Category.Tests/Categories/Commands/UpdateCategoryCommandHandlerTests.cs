using Categories.Application.Categories.Commands.UpdateCategory;
using Categories.Application.Common.Exceptions;
using Categories.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace Categories.Tests.Categories.Commands;

public class UpdateCategoryCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task UpdateCategoryCommandHandler_Success()
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(Context);
        var updatedName = "new updatedName";

        // Act
        await handler.Handle(new UpdateCategoryCommand
        {
            Id = CategoriesContextFactory.CategoryIdForUpdate,
            Name = updatedName
        }, CancellationToken.None);

        // Assert
        Assert.NotNull(await Context.Categories.SingleOrDefaultAsync(Category =>
            Category.Id == CategoriesContextFactory.CategoryIdForUpdate &&
            Category.Name == updatedName));
    }

    [Fact]
    public async Task UpdateCategoryCommandHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(
                new UpdateCategoryCommand
                {
                    Id = Guid.NewGuid()
                },
                CancellationToken.None));
    }
}
