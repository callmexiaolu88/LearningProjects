using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace AnalysisApiDemo
{
    public class SyntaxWalker : CSharpSyntaxWalker
    {
        const string programText =
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TopLevel
{
    using Microsoft;
    using System.ComponentModel;

    namespace Child1
    {
        using Microsoft.Win32;
        using System.Runtime.InteropServices;

        class Foo { ASD}
    }

    namespace Child2
    {
        using System.CodeDom;
        using Microsoft.CSharp;

        class Bar { }
    }
}";

        public static void Go()
        {
            var tree = CSharpSyntaxTree.ParseText(programText);
            var root = tree.GetCompilationUnitRoot();
            var visitor = new SyntaxWalker();
            visitor.Visit(root);
        }

        public ICollection<UsingDirectiveSyntax> Usings { get; } = new List<UsingDirectiveSyntax>();

        public override void VisitUsingStatement(UsingStatementSyntax node)
        {
            Console.WriteLine($"VisitUsingStatement\t: {node.ToString()}.");
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            Console.WriteLine($"\tVisitUsingDirective called with {node.Name}.");
            if (node.Name.ToString() != "System" &&
                !node.Name.ToString().StartsWith("System."))
            {
                Console.WriteLine($"\t\tSuccess. Adding {node.Name}.");
                this.Usings.Add(node);
            }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Console.WriteLine($"VisitClassDeclaration: {node.Identifier}.");
        }

    }
}