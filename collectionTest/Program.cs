using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Threading;

namespace collectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                List<int> ls = new List<int>();
                for (int i = 0; i < 1000; i++)
                {
                    ls.Add(i);
                }
                Task.Run(() =>
                {
                    foreach (var item in ls)
                    {
                        System.Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Read:{item}");
                        Thread.Sleep(5000);
                    }
                });

                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    ls.Sort();
                    return;
                });

                Console.Read();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }

        }
    }
}
