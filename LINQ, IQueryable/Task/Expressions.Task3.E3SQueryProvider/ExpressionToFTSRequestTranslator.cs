using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                ConvertToStringBuilder(node.Object, node.Arguments[0], node.Method.Name);
                return node;
            }

            return base.VisitMethodCall(node);
        }

        private void ConvertToStringBuilder(Expression nodeObject, Expression nodeArgument, string methodName)
        {
            Visit(nodeObject);

            _resultStringBuilder.Append(":");
            _resultStringBuilder.Append("(");
            
            if (new string[]{ "Contains", "EndsWith" }.Contains(methodName))
                _resultStringBuilder.Append("*");
            
            Visit(nodeArgument);
            
            if (new string[] {"Contains", "StartsWith"}.Contains(methodName))
                _resultStringBuilder.Append("*");
            
            _resultStringBuilder.Append(")");
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                        ConvertToStringBuilder(node.Left, node.Right, null);

                    else
                        ConvertToStringBuilder(node.Right, node.Left, null);
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(";");
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
