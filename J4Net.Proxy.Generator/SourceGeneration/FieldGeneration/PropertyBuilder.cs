using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.FieldGeneration
{
    internal class PropertyBuilder
    {
        public PropertyDeclarationSyntax Build(
            string name, 
            string type)
        {
            return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(type), name);
        }
    }
}
