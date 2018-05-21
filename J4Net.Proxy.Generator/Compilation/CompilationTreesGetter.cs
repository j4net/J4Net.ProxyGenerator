using System.Collections.Generic;
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
            var compilationTrees = new List<SyntaxTree>();

            foreach (var namespaceUnit in proxySource.NamespaceUnits)
            {
                var namespaceDeclaration = namespaceUnit.NamespaceDeclaration;
                var usings = SyntaxFactory.List(namespaceUnit.Usings);

                var compilationUnit = SyntaxFactory.CompilationUnit()
                    .AddMembers(namespaceDeclaration)
                    .WithUsings(usings);

                compilationTrees.Add(SyntaxFactory.SyntaxTree(compilationUnit));
            }

            return compilationTrees;
        }
    }
}
