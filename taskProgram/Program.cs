using System.Diagnostics;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace taskProgram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cc = new C1();
            if (cc.CC?.Name.IsNullOrWhiteSpace() == true)
                Console.WriteLine("PASS!");
            Console.WriteLine("Hello World!");
            ReturnVoid();
            await ReturnTask();
            await ReturnTaskValue();
            await foreach (var item in ReturnAsyncEnumerator())
            {
                Console.WriteLine(item);
            }
        }


        static async void ReturnVoid()
        {
            await Task.Delay(1000);
            Console.WriteLine("ReturnVoid");
        }

        static async Task ReturnTask()
        {
            await Task.Delay(1000);
            Console.WriteLine("ReturnTask");
            return;
        }

        static async Task<int> ReturnTaskValue()
        {
            await Task.Delay(1000);
            Console.WriteLine("ReturnTaskValue");
            return 10;
        }

        static async IAsyncEnumerable<int> ReturnAsyncEnumerator()
        {
            await Task.Delay(1000);
            Console.WriteLine("ReturnAsyncEnumerator");
            yield return 10;
        }
    }

    class C1
    {
        public C12 CC { get; set; }
    }

    class C12
    {
        public string Name { get; set; }
    }

    internal static class TypeExtensions
    {

        public static bool NotNullOrWhiteSpace(this string? str)
           => !string.IsNullOrWhiteSpace(str);

        public static bool IsNullOrWhiteSpace(this string? str)
            => string.IsNullOrWhiteSpace(str);
    }
}
