using System;
using app;

namespace SGConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var obj = new TestNS.TestClass();
            obj.Print();
            System.Console.WriteLine(Test.GetName()); 
        }

    }
}
