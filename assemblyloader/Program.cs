using System;
using System.Runtime.Loader;

namespace assemblyloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var alc = new CollectibleAssemblyLoadContext();
        }
    }
}
