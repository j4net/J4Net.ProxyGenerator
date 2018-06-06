using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using DSL;
using ProxyGenerator.Compilation;
using ProxyGenerator.SourceGeneration;

namespace ProxyGenerator
{
    public class ProxyGenerator
    {
        public void Generate(
            LibraryDescription description,
            string assemblyName,
            string assemblyOutputPath,
            Action<IEnumerable<SyntaxNode>> sourceWriter = null, 
            Action<string> compiltionLogger = null)
        {
            var classGenerator = new ClassProxyGenerator(new MethodBodyGetter());
            var proxySource = new LibraryProxyGenerator(classGenerator).Generate(description);

            var trees = CompilationTreesGetter.GetCompilationTreesFrom(proxySource);
            sourceWriter?.Invoke(trees.Select(el => el.GetRoot()));

            new Compiller().Compile(trees, assemblyName, assemblyOutputPath, compiltionLogger);
        }
    }
}
