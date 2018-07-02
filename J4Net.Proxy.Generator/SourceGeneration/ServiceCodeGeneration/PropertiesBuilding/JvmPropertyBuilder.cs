using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration.PropertiesBuilding
{
    internal class JvmPropertyBuilder
    {
        public static IEnumerable<PropertyDeclarationSyntax> Build()
        {
            var propertyGetter = SyntaxFactory.Block(JvmReturnStatement());

            var accessors = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithBody(propertyGetter);

            return new []{
                SyntaxFactory
                    .PropertyDeclaration(SyntaxFactory.ParseTypeName("JniEnvWrapper"), 
                                         SyntaxFactory.Identifier("Env"))
                    .AddAccessorListAccessors(accessors)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))
            };
        }

        private static StatementSyntax JvmReturnStatement()
        {
            return RoslynUtils.CallExpression(
                    SyntaxFactory.IdentifierName("JvmManager")
                        .MemberAccessExpression("INSTANCE")
                        .MemberAccessExpression("GetEnv")
                ).ToStatement();
        }
    }
}
