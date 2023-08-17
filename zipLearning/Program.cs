using System;
using System.Linq;

namespace zipLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var a1 = new string[] { "Lu", "Yu", "Long" };
            var a2 = new int[] { 1, 2, 3, 4, 5, 6 };
            var r = a2.Zip(a1);
            foreach (var item in r)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
