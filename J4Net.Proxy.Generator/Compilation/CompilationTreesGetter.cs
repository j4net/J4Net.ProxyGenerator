using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProxyGenerator.SourceGeneration;

namespace ProxyGenerator.Compilation
{
    internal static class CompilationTreesGetter
    {
        public static List<SyntaxTree> GetCompilationTreesFrom(
            ProxySource proxySource)
        {
            return proxySource
                .NamespaceUnits
                .Select(NamespaceUnitToSyntaxTree)
                .ToList();
        }

        public static SyntaxTree NamespaceUnitToSyntaxTree(NamespaceUnit unit)
        {
            var namespaceDeclaration = unit.NamespaceDeclaration;
            var usings = SyntaxFactory.List(unit.Usings);

            var compilationUnit = SyntaxFactory.CompilationUnit()
                .AddMembers(namespaceDeclaration)
                .WithUsings(usings);

            return SyntaxFactory.SyntaxTree(compilationUnit);
        }
    }
}
