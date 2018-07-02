using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration.FieldsBuilding
{
    internal static class ServiceFieldsBuilder
    {
        public static IEnumerable<FieldDeclarationSyntax> Build(
            ClassDescription classDescription,
            string classRefName,
            string jObjectName,
            string lockObjectName)
        {
            return ClassServiceFieldsBuilder
                .Build(classRefName, jObjectName, lockObjectName)
                .Concat(MethodRefFieldsBuilder.Build(classDescription.MethodsDescriptions))
                .ToArray();
        }
    }
}
