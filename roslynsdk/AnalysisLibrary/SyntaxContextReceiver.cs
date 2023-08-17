using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalysisLibrary
{
    public class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        public List<IMethodSymbol> methods = new List<IMethodSymbol>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is MethodDeclarationSyntax methodSyntax &&
                methodSyntax.AttributeLists.Any())
            {
                foreach (var itemls in methodSyntax.AttributeLists)
                {
                    foreach (var item in itemls.Attributes)
                    {
                        if (context.SemanticModel.GetTypeInfo(item).Type?.MetadataName == "HelloAttribute")
                        {
                            if (context.SemanticModel.GetDeclaredSymbol(methodSyntax) is IMethodSymbol methodSymbol)
                            {
                                methods.Add(methodSymbol);
                            }
                        }
                    }
                }
            }
        }
    }

    public class ClassInfo
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
    }
}