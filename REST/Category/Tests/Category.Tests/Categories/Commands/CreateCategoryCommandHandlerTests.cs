using Categories.Application.Categories.Commands.CreateCategory;
using Categories.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace Categories.Tests.Categories.Commands;

public class CreateCategoryCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateCategoryCommandHandler_Success()
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(Context);
        var categoryName = "categoryname";

        // Act
        var res = await handler.Handle(
            new CreateCategoryCommand
            {
                Name = categoryName
            },
            CancellationToken.None);

        // Assert
        Assert.NotNull(
            await Context.Categories
            .SingleOrDefaultAsync(x => x.Id == res));
    }
}
