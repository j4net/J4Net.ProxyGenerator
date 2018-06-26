using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    internal class MethodBodyGetter : IMethodBodyGetter
    {
        public BlockSyntax GetBodyFor(MethodDescription methodDescription)
        {
            var returnStatement = SyntaxFactory.ReturnStatement(
                SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                    SyntaxFactory.Literal(0)));

            return methodDescription.ReturnType == "void"
                   ? SyntaxFactory.Block()
                   : SyntaxFactory.Block().WithStatements(
                        SyntaxFactory.List(new List<StatementSyntax>
                        {
                            returnStatement
                        }));
        }
    }
}