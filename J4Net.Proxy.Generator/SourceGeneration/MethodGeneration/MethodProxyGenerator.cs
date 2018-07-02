using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    internal class MethodProxyGenerator : IGenerator<MethodDescription, MethodDeclarationSyntax>
    {
        private readonly IMethodBodyGetter methodBodyGetter;

        public MethodProxyGenerator(IMethodBodyGetter methodBodyGetter)
        {
            this.methodBodyGetter = methodBodyGetter;
        }

        public MethodDeclarationSyntax Generate(MethodDescription description)
        {
            var modifiersDescription = description.ModifiersDescriptions;
            var modifiers = modifiersDescription.ToTokens(isFieldModifiers: false);
            modifiers = AddDefaultModifiers(modifiers, modifiersDescription);

            var returnType = SyntaxFactory.ParseTypeName(description.ReturnType);

            if (description.ReturnType == "void")
                returnType = SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword));

            return SyntaxFactory
                .MethodDeclaration(returnType, description.Name)
                .AddModifiers(modifiers.ToArray())
                .AddParameters(description.ParametersDescription)
                .WithBody(methodBodyGetter.GetBodyFor(description));
        }

        private static IEnumerable<SyntaxToken> AddDefaultModifiers(
            IEnumerable<SyntaxToken> modifiers,
            IEnumerable<ModifierDescription> modifiersDescription)
        {
            if (!modifiersDescription.ContainsAnyFrom(ModifierDescription.FINAL, 
                                                      ModifierDescription.STATIC,
                                                      ModifierDescription.PUBLIC))
                modifiers = modifiers.Concat(new[]
                {
                    SyntaxFactory.Token(SyntaxKind.VirtualKeyword)
                });

            return modifiers;
        }
    }
}
