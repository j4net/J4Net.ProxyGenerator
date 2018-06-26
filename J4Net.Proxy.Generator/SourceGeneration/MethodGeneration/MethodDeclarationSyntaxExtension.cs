using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    internal static class MethodDeclarationSyntaxExtension
    {
        public static MethodDeclarationSyntax AddParameters(
            this MethodDeclarationSyntax declaration,
            IEnumerable<ParameterDescription> parametersDescription)
        {
            return declaration.AddParameterListParameters(
                CreateParameters(parametersDescription));
        }

        public static ConstructorDeclarationSyntax AddParameters(
            this ConstructorDeclarationSyntax declaration,
            IEnumerable<ParameterDescription> parametersDescription)
        {
            return declaration.AddParameterListParameters(
                CreateParameters(parametersDescription));
        }

        private static ParameterSyntax[] CreateParameters(
            IEnumerable<ParameterDescription> description)
        {
            return description
                .Select(el => new
                {
                    Name = SyntaxFactory.Identifier(el.Name),
                    Type = SyntaxFactory.ParseTypeName(el.Type)
                })
                .Select(el => SyntaxFactory.Parameter(el.Name).WithType(el.Type))
                .ToArray();
        }
    }
}
