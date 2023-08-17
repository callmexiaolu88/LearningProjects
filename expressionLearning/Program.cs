using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace expressionLearning
{

    class Program
    {

        //public event Action<string> Notify { add; re; }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var visitor = new CustomExpressionVisitor();

            var queryable = new CustomQueryable<string>();
            System.Console.WriteLine(queryable.GetHashCode().ToString());
            var newqueryable = queryable.Where(i => i.Contains("test") && i.Length > 2 * 5).Select(i => i.ToUpper()).OrderBy(i => 1 * 1);
            var value = newqueryable.FirstOrDefault();
            var values = newqueryable.ToList();
            System.Console.WriteLine($"Query string: [{visitor.TranslateExpression(newqueryable.Expression)}]");

            // String[] names = new[] { "Zhang Liang", "Jiang Chuan", "Hua Dong", "You min" };
            // IQueryable<String> query = names.AsQueryable<String>().Where(s => s.Length > 10);
            // Console.WriteLine(query.Expression.ToString());
            // System.Console.WriteLine(visitor.Visit(query.Expression).ToString());
            // MethodCallExpression methodCallExpression = query.Expression as MethodCallExpression;

            //Test1
            //TestMt();
            //Expression<Func<T, int>> exp = t => t.Id;
            //Expression<Func<T, string>> exp1 = t => t.Id.ToString();
            //TestExp(exp);
            //TestExp(exp1);

            //Test2
            //var st = new Test<T>();
            //var t = new T();
            //st.Set(i => i.Id, 100);
            //st.Setter(t);
            //Console.WriteLine(t);

            // EventModel model = new EventModel();
            // int timestamp = 100;
            // var findOne = FindOne<EventModel>(item => model.Id != item.Id && timestamp >= item.Start && timestamp < item.End);



            //Test3
            // var test = new NewExpressionTest<NewExpressionPlayback>();
            // var parameter = new NewExpressionTestParameter(Guid.NewGuid());
            // var playback = test.GeneratePlayback(parameter, 1);
            // Console.WriteLine(playback.Parameter == parameter);
        }

        static void TestExp(LambdaExpression exp)
        {
            Console.WriteLine(exp.Type);
            if (exp is LambdaExpression lambdaExpression)
            {
                Console.WriteLine("lambda expression:");
                Console.WriteLine(lambdaExpression.Type);
                Console.WriteLine(lambdaExpression.ReturnType);
                if (lambdaExpression.Body is MemberExpression memberExpression)
                {
                    Console.WriteLine("member expression:");
                    Console.WriteLine(memberExpression.Type);
                    Console.WriteLine(memberExpression.Member.DeclaringType);
                    Console.WriteLine(memberExpression.Member.Name);
                    Console.WriteLine(memberExpression.Member.MemberType);
                }
                return;
            }
            Console.WriteLine("exp is not lambda expression.");
        }

        private static void TestMt()
        {
            Expression<Func<string, string, string>> ts = (a, b) => a + b;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(DateTime));
            MemberExpression memberExpression = Expression.Property(null, typeof(DateTime).GetProperty("Now"));
            Expression<Func<DateTime>> lambdaExpression = Expression.Lambda<Func<DateTime>>(memberExpression, null);
            var func = lambdaExpression.Compile();
            Console.WriteLine(lambdaExpression);
            Console.WriteLine(func);
            Console.WriteLine(func());
            Console.WriteLine(ts.Compile()("123123", "asdad"));

            ParameterExpression parameterExp = Expression.Parameter(typeof(int), "a");
            MethodCallExpression methodCallExp = Expression.Call(typeof(Console).GetMethod("WriteLine", new[] { typeof(int) }), parameterExp);
            ParameterExpression parameter1Exp = Expression.Parameter(typeof(int), "b");
            MethodCallExpression methodCall1Exp = Expression.Call(typeof(Console).GetMethod("WriteLine", new[] { typeof(int) }), parameter1Exp);
            BlockExpression blockExpression = Expression.Block(methodCallExp, methodCall1Exp);
            Expression<Action<int, int>> exp = Expression.Lambda<Action<int, int>>(blockExpression, parameterExp, parameter1Exp);
            var func1 = exp.Compile();
            Console.WriteLine(exp);
            Console.WriteLine(func1);
            func1(22, 345);
        }

        class T
        {
            public int Id { get; set; }
        }

        class EventModel
        {
            public string Id { get; set; }
            public int Start { get; set; }
            public int End { get; set; }

        }

        public static T FindOne<T>(Expression<Func<T, bool>> predicate)
        {
            var param = predicate.Parameters[0];
            VisitExpression(predicate.Body);
            return default;
        }

        public static void VisitExpression(Expression expr)
        {
            if (expr is MemberExpression && expr.Type == typeof(bool))
            {
                System.Console.WriteLine(expr.NodeType);
                return;
            }

            if (expr.NodeType == ExpressionType.Not)
            {
                System.Console.WriteLine(expr.NodeType); return;
            }

            if (expr.NodeType == ExpressionType.Equal)
            {
                System.Console.WriteLine(expr.NodeType); return;
            }

            if (expr.NodeType == ExpressionType.NotEqual)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression2 = expr as BinaryExpression;
                System.Console.WriteLine(binaryExpression2.Left.GetPath());
                System.Console.WriteLine(binaryExpression2.Right.GetPath());
                System.Console.WriteLine(binaryExpression2.Left);
                System.Console.WriteLine(binaryExpression2.Right);
                return;
            }

            if (expr.NodeType == ExpressionType.LessThan)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression2 = expr as BinaryExpression;
                System.Console.WriteLine(binaryExpression2.Left.GetPath());
                System.Console.WriteLine(binaryExpression2.Right.GetPath());
                System.Console.WriteLine(binaryExpression2.Left);
                System.Console.WriteLine(binaryExpression2.Right);
                return;
            }

            if (expr.NodeType == ExpressionType.LessThanOrEqual)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression2 = expr as BinaryExpression;
                System.Console.WriteLine(binaryExpression2.Left.GetPath());
                System.Console.WriteLine(binaryExpression2.Right.GetPath());
                System.Console.WriteLine(binaryExpression2.Left);
                System.Console.WriteLine(binaryExpression2.Right);
                return;
            }

            if (expr.NodeType == ExpressionType.GreaterThan)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression2 = expr as BinaryExpression;
                System.Console.WriteLine(binaryExpression2.Left.GetPath());
                System.Console.WriteLine(binaryExpression2.Right.GetPath());
                System.Console.WriteLine(binaryExpression2.Left);
                System.Console.WriteLine(binaryExpression2.Right);
                return;
            }

            if (expr.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression2 = expr as BinaryExpression;
                System.Console.WriteLine(binaryExpression2.Left.GetPath());
                System.Console.WriteLine(binaryExpression2.Right.GetPath());
                System.Console.WriteLine(binaryExpression2.Left);
                System.Console.WriteLine(binaryExpression2.Right);
                return;
            }

            if (expr.NodeType == ExpressionType.AndAlso)
            {
                System.Console.WriteLine(expr.NodeType);
                BinaryExpression binaryExpression7 = expr as BinaryExpression;
                VisitExpression(binaryExpression7.Left);
                VisitExpression(binaryExpression7.Right); return;
            }

            if (expr.NodeType == ExpressionType.OrElse)
            {
                System.Console.WriteLine(expr.NodeType); return;
            }

            if (expr.NodeType == ExpressionType.Constant)
            {
                System.Console.WriteLine(expr.NodeType); return;
            }
            else
            {
                if (expr.NodeType == ExpressionType.Invoke)
                {
                    {
                        System.Console.WriteLine(expr.NodeType); return;
                    }
                }

                if (expr is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = expr as MethodCallExpression;
                    string name = methodCallExpression.Method.Name;
                    Type declaringType = methodCallExpression.Method.DeclaringType;
                    ExpressionType? expressionType = ((methodCallExpression.Arguments[0] is MemberExpression) ? new ExpressionType?((methodCallExpression.Arguments[0] as MemberExpression).Expression.NodeType) : null);
                    switch (name)
                    {
                        case "StartsWith":
                            {
                                System.Console.WriteLine("StartsWith"); break;

                            }
                        case "Equals":
                            {
                                System.Console.WriteLine("Equals"); break;

                            }
                        case "Contains":
                            {
                                System.Console.WriteLine("Contains");

                                break;
                            }
                    }

                    if (name == "Contains" && declaringType == typeof(Enumerable))
                    {
                        System.Console.WriteLine("ContainsEnumerable"); return;

                    }

                    if (name == "Any" && declaringType == typeof(Enumerable) && expressionType == ExpressionType.Parameter)
                    {
                        System.Console.WriteLine("Any"); return;

                    }

                    if (declaringType == typeof(Enumerable))
                    {
                        System.Console.WriteLine("Enumerable"); return;
                    }
                }
            }
        }


    }

    internal static class ExpressionExtensions1
    {
        private static Regex _removeSelect = new Regex("\\.Select\\s*\\(\\s*\\w+\\s*=>\\s*\\w+\\.", RegexOptions.Compiled);

        private static Regex _removeList = new Regex("\\.get_Item\\(\\d+\\)", RegexOptions.Compiled);

        private static Regex _removeArray = new Regex("\\[\\d+\\]", RegexOptions.Compiled);

        //
        // Summary:
        //     Get Path (better ToString) from an Expression. Support multi levels: x => x.Customer.Address
        //     Support list levels: x => x.Addresses.Select(z => z.StreetName)
        public static string GetPath(this Expression expr)
        {
            while (expr.NodeType == ExpressionType.Convert || expr.NodeType == ExpressionType.ConvertChecked)
            {
                expr = ((UnaryExpression)expr).Operand;
            }

            UnaryExpression unaryExpression;
            while (expr.NodeType == ExpressionType.Lambda && (unaryExpression = ((LambdaExpression)expr).Body as UnaryExpression) != null)
            {
                expr = unaryExpression.Operand;
            }

            string text = expr.ToString();
            int num = text.IndexOf('.');
            string input = ((num < 0) ? text : text.Substring(num + 1).TrimEnd(new char[1] { ')' }));
            input = _removeList.Replace(input, "");
            input = _removeArray.Replace(input, "");
            return _removeSelect.Replace(input, ".").Replace(")", "");
        }
    }
}
