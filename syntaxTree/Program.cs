using System.Reflection;
using System;
using Microsoft.CodeAnalysis.CSharp;

namespace syntaxTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var tree = CSharpSyntaxTree.ParseText(programText);
            var root = tree.GetRoot();
            var unitRoot = tree.GetCompilationUnitRoot();
            //CSharpCompilation.CreateScriptCompilation()
        }

        const string programText =
@"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
    }
}
