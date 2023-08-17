using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace asynclocal
{
    class Program
    {
        static AsyncLocal<string> local = new AsyncLocal<string>();
        static void Main(string[] args)
        {
            var concreteType = typeof(IEnumerable<IEquatable<string>>);
            var genericType1 = typeof(IEnumerable<>);
            var t1 = concreteType.GetInterfaces();
            var t2 = genericType1.GetInterfaces();
            var genericType2 = typeof(IEquatable<>);
            System.Console.WriteLine(concreteType.GetGenericTypeDefinition()==(genericType1));
            var argType = concreteType.GetGenericArguments()[0];
            System.Console.WriteLine(argType.GetGenericTypeDefinition().IsAssignableFrom(genericType2));

            Console.WriteLine("Hello World!");
            _ = Test(1);
            local.Value = "t2";
            _ = Test(2);
            //local.Value = "t3";
            _ = Test(3);
            //local.Value = "t4";
            _ = Test(4);
            Console.Read();
        }

       static async Task Test(int index){
            System.Console.WriteLine($"{index}------> Before:{local.Value}");
            await Task.Delay(5000);
            System.Console.WriteLine($"{index}------> After:{local.Value}");
        }
    }
}
