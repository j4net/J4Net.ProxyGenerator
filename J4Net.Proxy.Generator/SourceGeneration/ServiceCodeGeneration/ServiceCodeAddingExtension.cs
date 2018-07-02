using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.SourceGeneration.ServiceCodeGeneration.FieldsBuilding;
using ProxyGenerator.SourceGeneration.ServiceCodeGeneration.PropertiesBuilding;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal static class ServiceCodeAddingExtension
    {
        private const string CLASS_REF_NAME = "classRef";
        private const string J_OBJECT_NAME = "jObject";
        private const string LOCK_OBJECT_NAME = "monitor";

        public static ClassDeclarationSyntax AddServiceFields(
            this ClassDeclarationSyntax classDeclaration,
            ClassDescription classDescription)
        {
            var fields = ServiceFieldsBuilder.Build(
                    classDescription,
                    CLASS_REF_NAME,
                    J_OBJECT_NAME,
                    LOCK_OBJECT_NAME
                ).ToArray();

            return classDeclaration
                .AddMembers(fields);
        }

        public static ClassDeclarationSyntax AddServiceProperties(
            this ClassDeclarationSyntax classDeclaration,
            ClassDescription classDescription)
        {
            var refProperties = PropertiesBuilder.Build(
                    classDescription,
                    CLASS_REF_NAME,
                    LOCK_OBJECT_NAME)
                .ToArray();

            return classDeclaration
                .AddMembers(refProperties);
        }
    }
}
