using System.Linq.Expressions;
using System.Text;
using Complex.LinqProvider.Entities;

namespace Complex.LinqProvider.Provider;

public class ExpressionToSqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _resultSqlQueryBuilder = new("SELECT * FROM ");

    public string Translate(Expression expression)
    {

        _resultSqlQueryBuilder.Append($"{GetEntityType(expression)}");
        _resultSqlQueryBuilder.Append(' ');
        Visit(expression);
        return _resultSqlQueryBuilder.ToString().Trim();
    }

    private static string GetEntityType(Expression expression)
    {
        var type = expression.Type;
        while (true)
        {
            if (type.IsSubclassOf(typeof(BaseEntity)))
                return type.Name;
            type = type.GenericTypeArguments[0];
        }
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.DeclaringType == typeof(Queryable)
            && node.Method.Name == "Where")
        {
            _resultSqlQueryBuilder.Append("WHERE");

            _resultSqlQueryBuilder.Append(' ');
            var predicate = node.Arguments[1];
            Visit(predicate);
            return node;
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);

        _resultSqlQueryBuilder.Append(node.NodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "AND",
            _ => string.Empty
        });
        _resultSqlQueryBuilder.Append(' ');
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        _resultSqlQueryBuilder.Append(node.Member.Name);
        _resultSqlQueryBuilder.Append(' ');
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (!node.Type.Name.Contains("EntitySet"))
        {
            if (node.Type == typeof(string))
                _resultSqlQueryBuilder.Append('\'');

            _resultSqlQueryBuilder.Append($"{node.Value}");

            if (node.Type == typeof(string))
                _resultSqlQueryBuilder.Append('\'');

            _resultSqlQueryBuilder.Append(' ');
        }

        return node;
    }
}
