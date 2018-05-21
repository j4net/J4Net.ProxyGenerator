using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration
{
    internal interface IMethodBodyGetter
    {
        BlockSyntax GetBodyFor(MethodDeclarationSyntax methodDeclaration);
    }
}
