/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            //Example 1
            Expression<Func<int, int>> inputExpression1 = i => (i + 1) + 2 * (i - 1);
            Console.WriteLine($"Input exptession {inputExpression1}\n");
            Expression outputExpression1 = new IncDecExpressionVisitor().Execute(inputExpression1);
            Console.WriteLine($"Output exptession {outputExpression1}\n\n");

            //Example 2
            var listOfValues = new Dictionary<string, double>{{ "pi", Math.Round(Math.PI, 2)}};
            Expression<Func<double, double, double, double, int, double>> inputExpression2 =
                (pi, a, b, c, d) => Math.Pow(1.1, (a - 1) * (b - 1) / Math.Sqrt(1 + c) + pi) + Math.Round(1.2, d + 1);

            Console.WriteLine($"Input exptession {inputExpression2}\n");
            Expression outputExpression2 = new IncDecExpressionVisitor().Execute(inputExpression2, listOfValues);
            Console.WriteLine($"Output exptession {outputExpression2} \n");
            Console.ReadLine();
        }
    }
}
