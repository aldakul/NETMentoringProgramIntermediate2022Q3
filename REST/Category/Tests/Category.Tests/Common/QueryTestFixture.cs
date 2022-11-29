using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Application.Interfaces;
using Categories.Persistence;

namespace Categories.Tests.Common;

public class QueryTestFixture : IDisposable
{
    public CategoriesDbContext Context;
    public IMapper Mapper;

    public QueryTestFixture()
    {
        Context = CategoriesContextFactory.Create();
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AssemblyMappingProfile(
                typeof(ICategoriesDbContext).Assembly));
        });
        Mapper = configurationProvider.CreateMapper();
    }

    public void Dispose()
    {
        CategoriesContextFactory.Destroy(Context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
