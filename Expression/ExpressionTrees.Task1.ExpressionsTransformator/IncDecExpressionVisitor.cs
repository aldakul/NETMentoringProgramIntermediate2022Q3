using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private Dictionary<string, double> _listOfValues = new Dictionary<string, double>();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression expression;

            if (node.Right is ConstantExpression c && Convert.ToInt32(c.Value) == 1)
            {
                if ((node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract) && 
                    node.Left.NodeType == ExpressionType.Parameter && 
                    !IsParameterReplaced((node.Left as ParameterExpression).Name))
                {
                    if (node.NodeType == ExpressionType.Add)
                    {
                        expression = Expression.Increment(node.Left);
                    }
                    else
                    {
                        expression = Expression.Decrement(node.Left);
                    }
                }
                else
                {
                    expression = base.VisitBinary(node);
                }
            }
            else
            {
                expression = base.VisitBinary(node);
            }

            return expression;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(typeof(T), Visit(node.Body), node.Parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return IsParameterReplaced(node.Name)
                        ? Expression.Constant(_listOfValues[node.Name], node.Type)
                        : base.VisitParameter(node);
        }

        private bool IsParameterReplaced(string parameterName)
        {
            return _listOfValues?.ContainsKey(parameterName) ?? false;
        }

        public Expression Execute(Expression expression, Dictionary<string, double> listOfValues = null)
        {
            _listOfValues = listOfValues;
            return Visit(expression);
        }
    }
}
