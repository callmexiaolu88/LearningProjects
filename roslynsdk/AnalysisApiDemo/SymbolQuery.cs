using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalysisApiDemo
{
    public class SymbolQuery
    {
        const string programText =
@"using System;
using System.Collections.Generic;
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
            var tree = CSharpSyntaxTree.ParseText(programText);
            var root = tree.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("helloworld")
            .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
            .AddSyntaxTrees(tree);
            var sementicModel = compilation.GetSemanticModel(tree);
            UsingDirectiveSyntax usingDirectiveSyntax = root.Usings.First();
            NameSyntax usingName = usingDirectiveSyntax.Name;
            //return by name
            SymbolInfo nameSymbol = sementicModel.GetSymbolInfo(usingName);
            if (nameSymbol.Symbol != null)
            {
                System.Console.WriteLine($"NameSymbol: {nameSymbol.Symbol.Kind}");
                var systemSymbol = nameSymbol.Symbol as INamespaceSymbol;
                if (systemSymbol?.GetNamespaceMembers() != null)
                {
                    foreach (INamespaceSymbol ns in systemSymbol?.GetNamespaceMembers()!)
                    {
                        Console.WriteLine(ns);
                    }
                }
            }
            LiteralExpressionSyntax helloWorldString = root.DescendantNodes()
            .OfType<LiteralExpressionSyntax>()
            .Single();
            TypeInfo literalInfo = sementicModel.GetTypeInfo(helloWorldString);
            SymbolInfo literalSymbolInfo = sementicModel.GetSymbolInfo(helloWorldString); //return null
            var stringTypeSymbol = (INamedTypeSymbol?)literalInfo.Type;
            var allMembers = stringTypeSymbol?.GetMembers();
            var methods = allMembers?.OfType<IMethodSymbol>();
            var publicStringReturningMethods = methods?
                .Where(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, stringTypeSymbol) &&
                m.DeclaredAccessibility == Accessibility.Public);
            var distinctMethods = publicStringReturningMethods?.Select(m => m.Name).Distinct();
            foreach (string name in (from method in stringTypeSymbol?
                                     .GetMembers().OfType<IMethodSymbol>()
                                     where SymbolEqualityComparer.Default.Equals(method.ReturnType, stringTypeSymbol) &&
                                     method.DeclaredAccessibility == Accessibility.Public
                                     select method.Name).Distinct())
            {
                Console.WriteLine(name);
            }
        }
    }
}