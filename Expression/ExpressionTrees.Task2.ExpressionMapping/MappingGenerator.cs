using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator<TSource, TDestination>
    {
        private readonly Dictionary<string, LambdaExpression> _mappings =
            new Dictionary<string, LambdaExpression>();

        public MappingGenerator<TSource, TDestination> MapMember<T>(
            Expression<Func<TDestination, T>> destinationMemberExpression,
            Expression<Func<TSource, T>> sourceMappingExpression)
        {
            _mappings.Add(((MemberExpression)destinationMemberExpression.Body).Member.Name, sourceMappingExpression);
            return this;
        }

        public Mapper<TSource, TDestination> GenerateMapperFunc()
        {
            var mapper = new Mapper<TSource, TDestination>(GetMapFunction().Compile());
            return mapper;
        }

        private Expression<Func<TSource, TDestination>> GetMapFunction()
        {
            var sourceInstance = Expression.Parameter(typeof(TSource), "source");
            var destinationInstance = Expression.Variable(typeof(TDestination), "destination");
            var destinationConstructor = typeof(TDestination).GetTypeInfo().DeclaredConstructors
                        .First(c => !c.IsStatic && c.GetParameters().Length == 0);
            var expressions = new List<Expression>
            {
                sourceInstance,
                Expression.Assign(destinationInstance, Expression.New(destinationConstructor))
            };

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var sourceMembers = typeof(TSource).GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(typeof(TSource).GetProperties(bindingFlags)).ToArray();
            var destinationMembers = typeof(TDestination).GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(typeof(TDestination).GetProperties(bindingFlags)).ToArray();

            IEnumerable<BinaryExpression> collection()
            {
                foreach (var destinationMember in destinationMembers)
                {
                    Expression expression = null;

                    if (_mappings.ContainsKey(destinationMember.Name))
                        expression = Expression.Invoke(_mappings[destinationMember.Name], sourceInstance);
                    else
                    {
                        var sourceMember = sourceMembers.FirstOrDefault(sp => destinationMember.Name == sp.Name);
                        if (sourceMember != null)
                        {
                            if (sourceMember is PropertyInfo propertyInfo)
                            {
                                expression = Expression.Property(sourceInstance, propertyInfo);
                            }
                            else
                            {
                                expression = Expression.Field(sourceInstance, (FieldInfo)sourceMember);
                            }
                        }
                    }
                    var sourceValue = expression;
                    if (sourceValue != null)
                    {
                        Expression destinationValue = null;
                        if (destinationMember is PropertyInfo propertyInfo)
                        {
                            destinationValue = Expression.Property(destinationInstance, propertyInfo);
                        }
                        else
                        {
                            destinationValue = Expression.Field(destinationInstance, (FieldInfo)destinationMember);
                        }
                        yield return Expression.Assign(destinationValue, sourceValue);
                    }
                }
            }
            expressions.AddRange(collection());
            expressions.Add(destinationInstance);
            var body = Expression.Block(new[] { destinationInstance }, expressions);
            return Expression.Lambda<Func<TSource, TDestination>>(body, sourceInstance);
        }
    }
}