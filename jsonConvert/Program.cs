using System;
using System.Text;
using Newtonsoft.Json;

namespace jsonConvert
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(new string('-', 40));
                Console.Write("Input json:");
                Console.WriteLine(new string('-', 40));

                StringBuilder stringBuilder = new StringBuilder();
                string line;
                while (!String.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    stringBuilder.Append(line);
                }

                var input = stringBuilder.ToString().Trim();
                Console.WriteLine(input);
                // var input = Console.ReadLine();
                Console.Write(new string('-', 40));
                Console.Write("Output result:");
                Console.WriteLine(new string('-', 40));

                try
                {
                    var obj = JsonConvert.DeserializeObject(input);
                    Console.WriteLine(JsonConvert.SerializeObject(new
                    {
                        OperationType = 101,
                        CategoryId = 0x80520000,
                        ContentType = "application/json",
                        ClientRequestId = Guid.NewGuid().ToString(),
                        Data = JsonConvert.SerializeObject(obj)
                    }, Formatting.Indented));
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }
    }
}