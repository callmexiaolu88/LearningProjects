using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;


namespace expressionLearning
{
    public class CustomExpressionVisitor : ExpressionVisitor, IQueryStringTranslator
    {

        private string _queryString = string.Empty;

        public static string Translate(Expression expression)
        {
            var queryString = string.Empty;
            if (expression != null)
            {
                var translator = new CustomExpressionVisitor();
                System.Console.WriteLine($"Raw expression: {expression}");
                queryString = translator.TranslateExpression(expression);
            }
            return queryString;
        }


        public string TranslateExpression(Expression expression)
        {
            if (expression != null)
                Visit(expression);
            return _queryString;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            System.Console.WriteLine($"VisitMethodCall: {expression.Method.Name}");
            switch (expression)
            {
                case MethodCallExpression methodExpression:
                    {
                        var expressions = new List<MethodCallExpression> { methodExpression };
                        while (methodExpression.Arguments[0] is MethodCallExpression innerExpression)
                        {
                            expressions.Add(innerExpression);
                            methodExpression = innerExpression;
                        }

                        foreach (var exp in expressions)
                        {
                            switch (exp.Method.Name)
                            {
                                // case "OrderBy":
                                //     query.SortBy = TranslateOrderByMethod(exp, true);
                                //     break;
                                // case "OrderByDescending":
                                //     query.SortBy = TranslateOrderByMethod(exp, false);
                                //     break;
                                // case "Select":
                                //     query.Return = TranslateSelectMethod(exp, rootType, attr);
                                //     break;
                                // case "Take":
                                //     query.Limit ??= new SearchLimit { Offset = 0 };
                                //     query.Limit.Number = TranslateTake(exp);
                                //     break;
                                // case "Skip":
                                //     query.Limit ??= new SearchLimit { Number = 100 };
                                //     query.Limit.Offset = TranslateSkip(exp);
                                //     break;
                                // case "First":
                                // case "Any":
                                // case "FirstOrDefault":
                                //     query.Limit ??= new SearchLimit { Offset = 0 };
                                //     query.Limit.Number = 1;
                                //     break;
                                case "Where":
                                    _queryString = TranslateWhereMethod(exp);
                                    break;
                            }
                        }

                        break;
                    }

                    // case LambdaExpression lambda:
                    //     {
                    //         _queryString = BuildQueryFromExpression(lambda.Body);
                    //         break;
                    //     }
            }
            return expression;
        }
        private string TranslateWhereMethod(MethodCallExpression exp)
        {
            var quoteExpression = (UnaryExpression)exp.Arguments[1];
            var lambdaExpression = quoteExpression.Operand as LambdaExpression;
            return BuildQueryFromExpression(lambdaExpression.Body);
        }
        private string BuildQueryFromExpression(Expression body)
        {
            System.Console.WriteLine(body.NodeType);
            System.Console.WriteLine(body);
            if (body is BinaryExpression binaryExpression)
            {
                return TranslateBinaryExpression(binaryExpression);
            }
            if (body is MethodCallExpression method)
            {
               // return ExpressionParserUtilities.TranslateMethodExpressions(method);
            }
            return string.Empty;
        }

        private string TranslateBinaryExpression(BinaryExpression binaryExpression)
        {
            var stringBuilder = new StringBuilder();
            if (binaryExpression.Left is BinaryExpression leftBin)
            {
                stringBuilder.Append("(");
                stringBuilder.Append(TranslateBinaryExpression(leftBin));
                stringBuilder.Append(")");
            }
            return stringBuilder.ToString();
        }

    }
}