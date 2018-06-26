using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    public interface IMethodBodyGetter
    {
        BlockSyntax GetBodyFor(MethodDescription methodDescription);
    }
}
