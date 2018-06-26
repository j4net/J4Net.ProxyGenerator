using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration.FieldGeneration
{
    internal class FieldGenerator : IGenerator<FieldDescription, FieldDeclarationSyntax>
    {
        private EqualsValueClauseSyntax equalsValueClauseSyntax;

        public FieldGenerator(ExpressionSyntax initExpression = null)
        {
            equalsValueClauseSyntax = initExpression == null
                ? null
                : SyntaxFactory.EqualsValueClause(initExpression);
        }

        public FieldDeclarationSyntax Generate(FieldDescription description)
        {
            var variableDeclaration = SyntaxFactory.VariableDeclaration(
                SyntaxFactory.ParseTypeName(description.Type),
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(description.Name),
                        null,
                        equalsValueClauseSyntax
                    )
                })
            );

            var modifiersDescription = description.ModifiersDescriptions;
            var modifiers = modifiersDescription.ToTokens(isFieldModifiers: true);
            modifiers = AddDefaultModifiers(modifiers, modifiersDescription);

            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                    .AddModifiers(modifiers.ToArray());

            equalsValueClauseSyntax = null;
            return fieldDeclaration;
        }

        private IEnumerable<SyntaxToken> AddDefaultModifiers(IEnumerable<SyntaxToken> modifiers, 
            IEnumerable<ModifierDescription> modifiersDescription)
        {
            if (!modifiersDescription.ContainsAnyFrom(ModifierDescription.PROTECTED,
                                                      ModifierDescription.PUBLIC))
                modifiers = modifiers.Concat(new[]
                {
                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                });

            return modifiers;
        }

        public FieldGenerator InitializedWith(ExpressionSyntax 
            initializeExpression) => new FieldGenerator(initializeExpression);
    }
}
