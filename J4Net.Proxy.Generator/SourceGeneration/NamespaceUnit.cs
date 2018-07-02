using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration
{
    internal class NamespaceUnit
    {
        public NamespaceDeclarationSyntax NamespaceDeclaration { get; set; }
        public List<UsingDirectiveSyntax> Usings { get; }

        public NamespaceUnit(string name)
        {
            var namespaceDesignation = SyntaxFactory.IdentifierName(name);
            NamespaceDeclaration = SyntaxFactory.NamespaceDeclaration(namespaceDesignation);
            Usings = new List<UsingDirectiveSyntax>();
        }

        public NamespaceUnit(NamespaceDeclarationSyntax namespaceDeclaration, List<UsingDirectiveSyntax> usings)
        {
            NamespaceDeclaration = namespaceDeclaration;
            Usings = usings;
        }
    }
}
