using System;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal static class ServiceCodeAddingExtension
    {
        public static ClassDeclarationSyntax AddServiceFields(
            this ClassDeclarationSyntax classDeclaration,
            ClassDescription classDescription)
        {
            var standardClassFields = ClassServiceFieldsBuilder.Build();
            var methodsServiceFields = MethodServiceFieldsBuilder.Build(classDescription.MethodsDescriptions);

            return classDeclaration
                .AddMembers(standardClassFields)
                .AddMembers(methodsServiceFields);
        }

        public static ClassDeclarationSyntax AddServiceProperties()
        {
            throw new NotImplementedException();
        }
    }
}
