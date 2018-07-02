using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal static class ServiceCodeAddingExtension
    {
        public static ClassDeclarationSyntax AddServiceFields(
            this ClassDeclarationSyntax classDeclaration,
            ClassDescription classDescription,
            string classRefName,
            string jObjectName,
            string lockObjectName)
        {
            var fields = ClassServiceFieldsBuilder
                .Build(classRefName, jObjectName, lockObjectName)
                .Concat(MethodRefFieldsBuilder.Build(classDescription.MethodsDescriptions))
                .ToArray();

            return classDeclaration
                .AddMembers(fields);
        }

        public static ClassDeclarationSyntax AddServiceProperties(
            this ClassDeclarationSyntax classDeclaration,
            ClassDescription classDescription,
            string classRefName,
            string lockObjectName)
        {
            var refProperties = RefPropertyBuilder
                .Build(classDescription.MethodsDescriptions, classRefName, lockObjectName)
                .ToArray();

            return classDeclaration
                .AddMembers(refProperties);
        }
    }
}
