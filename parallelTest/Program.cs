using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace parallelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<int> ls = new List<int>();
            Random random = new Random(DateTime.Now.Second);
            for (int i = 0; i < 100; i++)
            {
                ls.Add(random.Next(1, 60));
            }

            Parallel.ForEach(ls, async i =>
            {
                var thread = Thread.CurrentThread;
                System.Console.WriteLine($"{i}: ThreadId:{thread.ManagedThreadId}, IsThreadPool:{thread.IsThreadPoolThread}, IsBack:{thread.IsBackground}");
                await Task.Delay(i * 1000);
                System.Console.WriteLine($"{i}: End");
            });

            Console.WriteLine("Press C+Control exit.");
            Console.Read();
        }
    }
}
