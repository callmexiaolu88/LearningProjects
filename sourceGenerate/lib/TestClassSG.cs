using Microsoft.CodeAnalysis;

namespace SGLib
{
    [Generator]
    public class TestClassSG : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("TestClass.cs", @"
namespace TestNS
{
    public class TestClass{
        public void Print(){
            System.Console.WriteLine(""This is SG!"");
        }
    }
}
           ");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}