using System;
using Microsoft.CodeAnalysis.CSharp;
using DSL;

namespace CodeGenerator.SourceGeneration
{
    internal static class DescriptionToSyntaxModifiersTransformer
    {
        public static SyntaxKind Transform(ModifierDescription modifierDescription)
        {
            var result = default(SyntaxKind);

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
                    result = SyntaxKind.SealedKeyword;
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
                case ModifierDescription.NATIVE:
                    break;
                case ModifierDescription.SYNCHRONIZED:
                    break;
                case ModifierDescription.TRANSIENT:
                    break;
                case ModifierDescription.VOLATILE:
                    break;
                default:
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
