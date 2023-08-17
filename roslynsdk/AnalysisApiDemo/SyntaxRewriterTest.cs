using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalysisApiDemo
{
    public class SyntaxRewriterTest : CSharpSyntaxRewriter
    {
        private readonly SemanticModel semanticModel;

        public SyntaxRewriterTest(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (node.Declaration.Variables.Count > 1)
            {
                return node;
            }
            if (node.Declaration.Variables[0].Initializer == null)
            {
                return node;
            }
            var declarator = node.Declaration.Variables.First();
            var variableTypeName = node.Declaration.Type;
            var variableType = (ITypeSymbol)semanticModel.GetSymbolInfo(variableTypeName).Symbol;
            var initializerInfo = semanticModel.GetTypeInfo(declarator.Initializer.Value);
            if (SymbolEqualityComparer.Default.Equals(variableType, initializerInfo.Type))
            {
                TypeSyntax varTypeName = SyntaxFactory.IdentifierName("var")
                    .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                    .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

                return node.ReplaceNode(variableTypeName, varTypeName);
            }
            else
            {
                return node;
            }
        }
    }
}