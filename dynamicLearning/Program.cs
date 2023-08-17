using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dynamicLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var personal = new Personal { Name = "lyl", ID = 100 };
            var data = JsonConvert.SerializeObject(personal);
            Console.WriteLine($"Serialize data: {data}");
            var @dynamic = JsonConvert.DeserializeObject<dynamic>(data);
            PrintValue(@dynamic.Name, @dynamic.ID);
            Console.WriteLine(@dynamic.Name);
            Console.WriteLine(@dynamic.name);
            Console.WriteLine(@dynamic.NAME);
            Console.WriteLine(@dynamic.nAme);
            Console.WriteLine(@dynamic.ID);
            Console.WriteLine(@dynamic.id);
            Console.WriteLine(@dynamic.Id);
            Console.WriteLine(@dynamic.iD);
        }

        static void PrintValue(JValue name, JValue id){
            System.Console.WriteLine($"{nameof(PrintValue)}() Name: {name}, ID: {id}");
        }

    }

    class Personal
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
