using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace lib
{
    [Generator]
    public class IncreasedSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider.CreateSyntaxProvider<ClassDeclarationSyntax>(
                                predicate: (sd, _) =>
                                {
                                    if (sd is ClassDeclarationSyntax cd && 
                                    cd.Modifiers.Any(i => i.IsKind(SyntaxKind.PartialKeyword)))
                                        return true;
                                    return false;
                                },
                                transform: (syntaxcontext, _) =>
                                {
                                    return syntaxcontext.Node as ClassDeclarationSyntax;
                                }).
                                Where(i => i != null);
            context.RegisterSourceOutput(provider, (ctx, cds) =>
            {
                var npm = cds.Parent as NamespaceDeclarationSyntax;
                System.Console.WriteLine(npm.Name);
                var isStatic = cds.Modifiers.Any(i => i.IsKind(SyntaxKind.StaticKeyword));
                ctx.AddSource($"{cds.Identifier.ValueText}.g.cs", @"
using System;
namespace app
{
    partial class Test
    {
        public static int GetName()
        {
            return 123;
        }
    }
}
                ");
            });
        }
    }
}