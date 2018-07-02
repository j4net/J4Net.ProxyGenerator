using System;
using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ProxyGenerator.SourceGeneration
{
    internal static class ModifiersDescriptionExtension
    {
        public static IEnumerable<SyntaxToken> ToTokens(
            this IEnumerable<ModifierDescription> modifiersDescription,
            bool isFieldModifiers)
        {
            return modifiersDescription
                .Select(el => ModifierDescriptionToSyntaxKind(el, isFieldModifiers))
                .Where(el => el != SyntaxKind.None)
                .Select(SyntaxFactory.Token);
        }

        private static SyntaxKind ModifierDescriptionToSyntaxKind(
            ModifierDescription modifierDescription,
            bool isFieldModifiers)
        {
            SyntaxKind result;

            switch (modifierDescription)
            {
                case ModifierDescription.PUBLIC:
                {
                    result = SyntaxKind.PublicKeyword;
                    break;
                }
                case ModifierDescription.ABSTRACT:
                {
                    result = SyntaxKind.AbstractKeyword;
                    break;
                }
                case ModifierDescription.FINAL:
                {
                    result = isFieldModifiers 
                                    ? SyntaxKind.ReadOnlyKeyword
                                    : SyntaxKind.SealedKeyword;
                    break;
                }
                case ModifierDescription.PROTECTED:
                {
                    result = SyntaxKind.ProtectedKeyword;
                    break;
                }
                case ModifierDescription.STATIC:
                {
                    result = SyntaxKind.StaticKeyword;
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
