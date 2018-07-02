using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration.PropertiesBuilding
{
    internal static class PropertiesBuilder
    {
        public static IEnumerable<PropertyDeclarationSyntax> Build(
            ClassDescription classDescription,
            string classRefName,
            string lockObjectName)
        {
            return JvmPropertyBuilder
                .Build()
                .Concat(
                    RefPropertyBuilder
                        .Build(
                            classDescription.MethodsDescriptions,
                            classRefName,
                            lockObjectName)
                );
        }
    }
}
