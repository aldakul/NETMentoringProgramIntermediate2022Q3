using Categories.Application.Categories.Commands.CreateCategory;
using Categories.Application.Categories.Commands.DeleteCommand;
using Categories.Application.Common.Exceptions;
using Categories.Tests.Common;

namespace Categories.Tests.Categories.Commands;

public class DeleteCategoryCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task DeleteCategoryCommandHandler_Success()
    {
        // Arrange
        var handler = new DeleteCategoryCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteCategoryCommand
        {
            Id = CategoriesContextFactory.CategoryIdForDelete
        }, CancellationToken.None);

        // Assert
        Assert.Null(Context.Categories.SingleOrDefault(Category =>
            Category.Id == CategoriesContextFactory.CategoryIdForDelete));
    }

    [Fact]
    public async Task DeleteCategoryCommandHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new DeleteCategoryCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(
                new DeleteCategoryCommand
                {
                    Id = Guid.NewGuid()
                },
                CancellationToken.None));
    }
}