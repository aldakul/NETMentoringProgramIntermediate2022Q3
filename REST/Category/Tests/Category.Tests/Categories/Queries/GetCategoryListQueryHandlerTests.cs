using AutoMapper;
using Categories.Application.Categories.Queries.GetCategoryList;
using Categories.Persistence;
using Categories.Tests.Common;

namespace Categories.Tests.Categories.Queries;

[Collection("QueryCollection")]
public class GetCategoryListQueryHandlerTests
{
    private readonly CategoriesDbContext Context;
    private readonly IMapper Mapper;

    public GetCategoryListQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetCategoryListQueryHandler_Success()
    {
        // Arrange
        var handler = new GetCategoryListQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(
            new GetCategoryListQuery(),
            CancellationToken.None);

        // Assert
        Assert.IsType<CategoryListVm>(result);
        result.Categories.Count.Equals(2);
    }
}
