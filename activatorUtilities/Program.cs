using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace activatorUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // ServiceCollection services = new ServiceCollection();
            // services.AddTransient<Param1>();
            // services.AddTransient<Param2>();
            // var provider = services.BuildServiceProvider();
            // var ct = typeof(TestProgram).GetConstructors();
            // var cm = new ConstructorMatcher(ct[1]);
            // var c = cm.Match(new object[] { new Param1() });
            // ActivatorUtilities.CreateInstance<TestProgram>(provider, new Param1());
            var mySerializer = new XmlSerializer(typeof(xmlclass));
            // To read the file, create a FileStream.
            using var myFileStream = new FileStream("gridsource.xml", FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            var myObject = (xmlclass)mySerializer.Deserialize(myFileStream);

            var type = Type.GetType("activatorUtilities.TestProgram");
            var wrapperMethod = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).
                Where(m => m.Name == "playoutExecuter" && m.IsGenericMethod && m.GetParameters().Length == 5).First();
            var parameterType = typeof(Param1);
            var methodInfo = type.GetMethod("innerTest", BindingFlags.Instance | BindingFlags.NonPublic);
            var instanceExpression = Expression.Constant(new TestProgram(new Param1()));
            var parameterExpressions = wrapperMethod.GetParameters().Select(p => p.ParameterType.IsGenericType ?
            Expression.Parameter(p.ParameterType.GetGenericTypeDefinition().MakeGenericType(parameterType, typeof(string), typeof(Param))) :
            Expression.Parameter(p.ParameterType)).ToArray();
            var playoutExecuterGenericMethodInfo = wrapperMethod.MakeGenericMethod(parameterType);
            var playoutExecuterExpression = Expression.Call(instanceExpression, playoutExecuterGenericMethodInfo, parameterExpressions);
            var playoutExecuterFunc = Expression.Lambda(playoutExecuterExpression, parameterExpressions).Compile();

            var parameter1Expression = Expression.Parameter(parameterType);
            var parameter2Expression = Expression.Parameter(typeof(string));
            var callExpression = Expression.Call(instanceExpression, methodInfo, parameter1Expression, parameter2Expression);
            var funcExpression = Expression.Lambda(callExpression, parameter1Expression, parameter2Expression);
            System.Console.WriteLine(funcExpression.ToString());
            var func = funcExpression.Compile();
            var p = playoutExecuterFunc.DynamicInvoke("1", "2", "3", func, "test");
            System.Console.WriteLine(p);

        }
    }

    public class TestProgram
    {
        //[ActivatorUtilitiesConstructor]
        public TestProgram(Param1 p)
        {

        }

        public TestProgram(Param1 p, Param2 name, string asd)
        {

        }

        private Param innerTest(Param1 p, string str)
        {
            return new Param();
        }

        private Param playoutExecuter(Func<Param> func, [CallerMemberName] string callerName = null)
        {
            return func();
        }

        private Param playoutExecuter(string clientId, Func<string, Param> func, [CallerMemberName] string callerName = null) => playoutExecuter(() =>
        {
            return default(Param);
        }, callerName);

        private Param playoutExecuter<T>(string contentType, string operationParam, Func<T, Param> func, [CallerMemberName] string callerName = null) => playoutExecuter(() =>
         {
             return default(Param);
         }, callerName);

        private Param playoutExecuter<T>(string contentType, string operationParam, string clientId, Func<T, string, Param> func, [CallerMemberName] string callerName = null) => playoutExecuter(() =>
         {
             return func(default(T), clientId);
         }, callerName);

    }
    public class Param
    {
    }
    public class Param1
    {
    }

    public class Param2
    {
    }
}
