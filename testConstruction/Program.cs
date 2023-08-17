using System;

namespace testConstruction
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var obj = new TestObj();
            System.Console.WriteLine(obj.Name);
        }
    }

    class TestObj
    {
        public TestObj()
        {
            Name = "123";
        }

        private string UU = "1";
        public string Name { get; private set; } = "456";
    }
}
