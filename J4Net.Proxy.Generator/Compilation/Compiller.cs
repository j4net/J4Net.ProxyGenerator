using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace ProxyGenerator.Compilation
{
    internal class Compiller
    {
        public void Compile(
            List<SyntaxTree> compilationTrees,
            string assemblyName,
            string assemblyOutputPath,
            Action<string> compilationLogWriter = null)
        {
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                compilationTrees,
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var path = Path.Combine(assemblyOutputPath, assemblyName);
            LogCompilationResult(compilationLogWriter, compilation.Emit(path));
        }

        private void LogCompilationResult(Action<string> logger, EmitResult compilationResult)
        {
            if (logger == null)
                return;

            foreach (var diagnostic in compilationResult.Diagnostics)
            {
                var type = diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error
                    ? "Error"
                    : "Info";

                logger($"{type} -- {diagnostic.Id}: {diagnostic.GetMessage()}");
            }

            logger($"Info -- {(compilationResult.Success ? "S" : "Uns")}uccess compilation");
        }
    }
}
