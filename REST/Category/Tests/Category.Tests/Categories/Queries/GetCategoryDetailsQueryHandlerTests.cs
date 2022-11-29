using AutoMapper;
using Categories.Application.Categories.Queries.GetCategoryDetails;
using Categories.Persistence;
using Categories.Tests.Common;

namespace Categories.Tests.Categories.Queries;

[Collection("QueryCollection")]
public class GetCategoryDetailsQueryHandlerTests
{
    private readonly CategoriesDbContext Context;
    private readonly IMapper Mapper;

    public GetCategoryDetailsQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetCategoryDetailsQueryHandler_Success()
    {
        // Arrange
        var handler = new GetCategoryDetailsQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(
            new GetCategoryDetailsQuery
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            },
            CancellationToken.None);

        // Assert
        Assert.IsType<CategoryDetailsVm>(result);
        Assert.Equal("One", result.Name);
    }
}
