using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using DSL;
using ProxyGenerator.Compilation;
using ProxyGenerator.SourceGeneration;
using ProxyGenerator.SourceGeneration.MethodGeneration;

namespace ProxyGenerator
{
    public class ProxyGenerator
    {
        public void Generate(
            LibraryDescription description,
            string assemblyName,
            string assemblyOutputPath,
            Action<IEnumerable<string>> sourceLogger = null, 
            Action<string> compiltionLogger = null)
        {
            var methodBodyGetter = new MethodBodyGetter();
            var methodProxyGenerator = new MethodProxyGenerator(methodBodyGetter);
            var constructorProxyGenerator = new ConstructorProxyGenerator(methodBodyGetter);

            var classGenerator = new ClassProxyGenerator(methodProxyGenerator, constructorProxyGenerator);
            var proxySource = new LibraryProxyGenerator(classGenerator).Generate(description);

            var trees = CompilationTreesGetter.GetCompilationTreesFrom(proxySource);
            sourceLogger?.Invoke(trees.Select(el => el
                    .GetRoot()
                    .NormalizeWhitespace()
                    .ToFullString())
                );

            new Compiller().Compile(trees, assemblyName, assemblyOutputPath, compiltionLogger);
        }
    }
}
