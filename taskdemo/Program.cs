using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace taskdemo
{
    class Program
    {
        static AsyncLocal<int> mainValue = null;
        static AsyncLocal<int> withValue = null;
        static AsyncLocal<int> withoutValue = null;
        static async Task Main(string[] args)
        {
            try
            {
                var tasks = new List<Task>();
                for (int i = 0; i < 500; i++)
                {
                    var index = i;
                    tasks.Add(Task.Run(() =>
                      {
                          Thread.Sleep(10000);
                          System.Console.WriteLine($"This is Thread [{index}]");
                      }));
                }

                int workerThreads, completionPortThreads;
                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Min Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Max Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Available Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                System.Console.WriteLine($"Existing Threads: [{ThreadPool.ThreadCount}]");

                ThreadPool.SetMinThreads(64, 64);
                System.Console.WriteLine($"Afater set Threads: [{ThreadPool.ThreadCount}]");
                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Min Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Max Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Available Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                System.Console.WriteLine($"Existing Threads: [{ThreadPool.ThreadCount}]");

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(5000);
                    System.Console.WriteLine($"[{i}] Delay 5s Threads: [{ThreadPool.ThreadCount}]");
                    ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                    System.Console.WriteLine($"[{i}] Min Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                    ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                    System.Console.WriteLine($"[{i}] Max Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                    ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                    System.Console.WriteLine($"[{i}] Available Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                    System.Console.WriteLine($"[{i}] Existing Threads: [{ThreadPool.ThreadCount}]");
                }

                await Task.WhenAll(tasks);
                System.Console.WriteLine($"Delay all Threads: [{ThreadPool.ThreadCount}]");
                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Min Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Max Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Available Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                System.Console.WriteLine($"Existing Threads: [{ThreadPool.ThreadCount}]");

                Thread.Sleep(20000);
                System.Console.WriteLine($"After Delay all Threads: [{ThreadPool.ThreadCount}]");
                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Min Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Max Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                System.Console.WriteLine($"Available Threads: workerThreads [{workerThreads}], completionPortThreads [{completionPortThreads}]");
                System.Console.WriteLine($"Existing Threads: [{ThreadPool.ThreadCount}]");

                Console.Read();

                // System.Console.WriteLine($"Main [{Thread.CurrentThread.ManagedThreadId}] main:[${mainValue?.Value}]");

                // //ThreadPool.QueueUserWorkItem(TestWithAsyncLocalValue);
                // new Thread(TestWithAsyncLocalValue).Start();
                // mainValue = new AsyncLocal<int>() { Value = 100 };
                // //ThreadPool.QueueUserWorkItem(TestWithoutAsyncLocalValue);
                // new Thread(TestWithoutAsyncLocalValue).Start();
                // System.Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] main:[${mainValue.Value}]");

                // // Console.WriteLine("Hello World!");
                // // await Execute();
                // // Console.WriteLine("Hello World!");
                // Console.Read();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static async Task Execute()
        {
            System.Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]");
            foreach (var item in Enumerable.Range(1, 10).Reverse())
            {
                await Task.Yield();
                System.Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Start:{item} {DateTime.Now}");
                Print(item);
            }
        }

        static async Task Print(int i)
        {
            var tcs = new TaskCompletionSource<Object>();
            await tcs.Task;
            //await Task.Delay(i * 1000);
            Thread.Sleep(i * 1000);
            System.Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] End:{i} {DateTime.Now}");
            tcs.SetResult(new object());
        }

        static void TestWithAsyncLocalValue(object value)
        {
            System.Console.WriteLine($"TestWithAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] Before main:[${mainValue?.Value}]");
            withValue = new AsyncLocal<int>() { Value = 99 };
            System.Console.WriteLine($"TestWithAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] After set withValue:[${withValue.Value}]");
            System.Console.WriteLine($"TestWithAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] End main:[${mainValue?.Value}]");
            System.Console.WriteLine($"TestWithAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] Last withValue:[${withValue.Value}]");
            ThreadPool.QueueUserWorkItem(TestWithoutAsyncLocalValue);
            Thread.Sleep(1000);
        }

        
        static void TestWithoutAsyncLocalValue(object value)
        {
            System.Console.WriteLine($"TestWithoutAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] Before main:[${mainValue?.Value}]");
            System.Console.WriteLine($"TestWithoutAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] Before withValue:[${withValue.Value}]");
            System.Console.WriteLine($"TestWithoutAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] End main:[${mainValue?.Value}]");
            System.Console.WriteLine($"TestWithoutAsyncLocalValue [{Thread.CurrentThread.ManagedThreadId}] Last withValue:[${withValue.Value}]");
        }

    }
}
