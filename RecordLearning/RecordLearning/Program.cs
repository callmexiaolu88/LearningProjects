using System;

namespace RecordLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var a = Get();
            Console.WriteLine(a.To());
            a.TestName.Name = "123";
            Console.WriteLine(a.To());
            var b = a with { LastName = "212" };
            Console.WriteLine(b.To());
            Console.Read();
        }

        static A Get()
        {
            return new A("Asd", "asdasd",new Test { Name="TestName"});
        }

        record A(string FirstName, string LastName, Test TestName) { 
            public string To()
            {
                return $"{FirstName} {LastName} {TestName.Name} {TestName.InitName}";
            }
        }

        class Test
        {
            public string Name { get; set; }
            public string InitName { get; init; } = "5656";

            public Test()
            {
                InitName = "1212";

                Console.WriteLine(Letter('2'));
            }

            public bool Letter(char c) => c is > '1' and < '5';
        }
    }
}
