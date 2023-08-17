using System;
using System.Collections.Generic;

namespace sortListLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var items = new List<NameValue>();
                        items.Add(new NameValue { Name = "1", Value = 1 });

                        items.Add(new NameValue { Name = "3", Value = 3 });


            items.Add(new NameValue { Name = "2", Value = 2 });
            items.Sort((x, y) => y.Value - x.Value);
            foreach (var item in items)
            {
                System.Console.WriteLine(item.Name);
            }
        }
    }

    class NameValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
