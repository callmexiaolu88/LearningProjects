using System;
using Newtonsoft.Json;

namespace Scte104Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var scte = Console.ReadLine();
            var scte = "FFFF001E00010300020000010101000E0301000203000100000000000000";
            var message = Scte104Parser.Scte104Messages.Scte104MessageHelper.ParseScte104Message(scte);
            var options = new JsonSerializerSettings
            {
                Formatting=Formatting.Indented,
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };
            var serilizedString = JsonConvert.SerializeObject(message,options);
            System.Console.WriteLine(serilizedString);
        }
    }
}
