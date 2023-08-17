using System;

namespace AnalysisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Demonstrate source generator:");
            var generator = new SourceGeneratorTest();
            generator.Hello("Generator");
        }
    }
}
