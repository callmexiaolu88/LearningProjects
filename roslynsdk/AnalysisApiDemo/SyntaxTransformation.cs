using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Console;

namespace AnalysisApiDemo
{
    public class SyntaxTransformation
    {
        private const string sampleCode =
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

        public static void Go()
        {
            NameSyntax name = SyntaxFactory.IdentifierName("System");
            WriteLine($"\tCreated the identifier {name}");
            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Collections"));
            WriteLine(name.ToString());
            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Generic"));
            WriteLine(name.ToString());

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sampleCode);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name);
            WriteLine(root.ToString());
            root = root.ReplaceNode(oldUsing, newUsing);
            WriteLine(root.ToString());

        }
    }
}