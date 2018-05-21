using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration
{
    internal class MethodBodyGetter : IMethodBodyGetter
    {
        public BlockSyntax GetBodyFor(MethodDeclarationSyntax methodDeclaration)
        {
            return SyntaxFactory.Block();
        }
    }
}
