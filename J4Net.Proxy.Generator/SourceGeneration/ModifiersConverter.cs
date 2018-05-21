using System;
using DSL;
using Microsoft.CodeAnalysis.CSharp;

namespace ProxyGenerator
{
    public static class ModifiersConverter
    {
        public static SyntaxKind ModifierDescriptionToSyntaxKind(ModifierDescription modifierDescription)
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
                default:
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
