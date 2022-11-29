using Categories.Persistence;

namespace Categories.Tests.Common;

public abstract class TestCommandBase : IDisposable
{
    protected readonly CategoriesDbContext Context;

    public TestCommandBase()
    {
        Context = CategoriesContextFactory.Create();
    }

    public void Dispose()
    {
        CategoriesContextFactory.Destroy(Context);
    }
}
