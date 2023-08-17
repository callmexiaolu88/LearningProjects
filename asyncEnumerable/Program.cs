using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asyncEnumerable
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var enumerableObj = new ConcreteAsyncEnumerable();
            await foreach (var item in enumerableObj)
            {
                Console.WriteLine(item);
            }
            var ls = new List<String>();
            
            foreach (var item in enumerableObj)
            {
                Console.WriteLine(await item);
            }
            Console.WriteLine("await end.");
        }
    }
}
