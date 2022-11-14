using System.Linq.Expressions;

namespace Complex.LinqProvider.Provider;

public class LinqProvider : IQueryProvider
{
    private readonly ExpressionToSqlTranslator _translator;
    private readonly DbProvider _dbProvider;

    public LinqProvider(ExpressionToSqlTranslator translator, DbProvider dbProvider)
    {
        _translator = translator;
        _dbProvider = dbProvider;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new Query<TElement>(expression, this);

    public object Execute(Expression expression)
    {
        throw new NotImplementedException();
    }

    public TResult Execute<TResult>(Expression expression)
    {
        var query = _translator.Translate(expression);
        var entityType = typeof(TResult).GenericTypeArguments[0];
        var getQueryResultsInfo = typeof(DbProvider).GetMethod(nameof(DbProvider.GetQueryResults));
        if (getQueryResultsInfo != null)
        {
            var getQueryResults = getQueryResultsInfo.MakeGenericMethod(entityType);
            return (TResult)getQueryResults.Invoke(_dbProvider, new object[] {query});
        }

        return default;
    }
}