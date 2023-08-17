using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace AnalysisLibrary
{
    [Generator]
    public class InnerHelloGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(ctx => ctx.AddSource("helloattribute.g.cs", @"
using System;

namespace AnalysisLibrary {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class HelloAttribute : Attribute
    {
    }
}
            "));
            context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is SyntaxContextReceiver syntaxContextReceiver)
            {
                var groupingList = syntaxContextReceiver.methods.GroupBy<IMethodSymbol, INamedTypeSymbol>(m => m.ContainingType, SymbolEqualityComparer.Default);
                foreach (var grouping in groupingList)
                {
                    var source = processClass(grouping.Key, grouping.ToList());
                    if (!string.IsNullOrWhiteSpace(source))
                        context.AddSource($"{grouping.Key.Name}_helloattr.g.cs", source);
                }
            }
        }

        private string processClass(INamedTypeSymbol classSymbol, IEnumerable<IMethodSymbol> methodSymbols)
        {
            string result = null;
            if (classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                var namepace = classSymbol.ContainingNamespace.ToDisplayString();
                StringBuilder sb = new StringBuilder();
                sb.Append($@"
using System;

namespace {namepace}
{{
    public partial class {classSymbol.Name}
    {{
                ");
                foreach (var methodSymbol in methodSymbols)
                {
                    var modifer = getModifer(methodSymbol.DeclaredAccessibility);
                    if (!string.IsNullOrWhiteSpace(modifer))
                        sb.Append($"{modifer} ");
                    if (methodSymbol.IsPartialDefinition)
                        sb.Append("partial ");
                    if (methodSymbol.IsStatic)
                        sb.Append("static ");
                    if (methodSymbol.ReturnsVoid)
                        sb.Append("void ");
                    else
                        sb.Append($"{methodSymbol.ReceiverType} ");
                    sb.Append($"{methodSymbol.Name}(");
                    for (int i = 0; i < methodSymbol.Parameters.Length; i++)
                    {
                        sb.Append($"{methodSymbol.Parameters[i].ToDisplayString()}");
                        if (methodSymbol.Parameters.Length - i > 1)
                            sb.Append(",");
                    }
                    sb.Append(")");
                    sb.Append($"=> Console.WriteLine(\"{methodSymbol.Name}\");");
                }
                sb.Append("}}");
                result = sb.ToString();
            }
            return result;
        }

        protected internal string getModifer(Accessibility accessibility)
        {
            string result = string.Empty;
            switch (accessibility)
            {
                case Accessibility.Private:
                    return string.Empty;
                case Accessibility.ProtectedAndInternal:
                    return "private protected";
                case Accessibility.Protected:
                    return "protected";
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.ProtectedOrInternal:
                    return "protected internal";
                case Accessibility.Public:
                    return "public";
                default:
                    return string.Empty;
            }
        }
    }
}