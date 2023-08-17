using System.Threading;
using System;

namespace asyncDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Action<string> TestHandler=null;
            TestHandler += Test1;
            TestHandler += Test2;
            TestHandler += Test3;
            System.Console.WriteLine($"TestHandler Start: {DateTime.Now}");

            var ar = TestHandler.BeginInvoke("test123", _ =>
            {
                System.Console.WriteLine($"TestHandler End: {DateTime.Now}");
            }, null);
            ar.AsyncWaitHandle.WaitOne();
            // var ls = TestHandler.GetInvocationList();
            // foreach (var item in ls)
            // {
            //     var l1 = item.GetInvocationList();
            //    var r= item.DynamicInvoke("Hello");
            //     ((Action<string>)item).Invoke("123");
            // }
            TestHandler.Invoke("Hello");
            System.Console.WriteLine($"TestHandler End: {DateTime.Now}");
        }

        static void Test1(string msg)
        {
            Thread.Sleep(5000);
            System.Console.WriteLine($"Test1: {msg} {DateTime.Now}");
        }

        static void Test2(string msg)
        {
            Thread.Sleep(5000);
            System.Console.WriteLine($"Test2: {msg} {DateTime.Now}");
        }

        static void Test3(string msg)
        {
            Thread.Sleep(5000);
            System.Console.WriteLine($"Test3: {msg} {DateTime.Now}");
        }
    }
}
