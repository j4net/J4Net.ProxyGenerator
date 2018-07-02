using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DSL;
using ProxyGenerator.Infrastructure;
using ProxyGenerator.SourceGeneration.ServiceCodeGeneration;

namespace ProxyGenerator.SourceGeneration
{
    internal class ClassProxyGenerator : IGenerator<ClassDescription, ClassDeclarationSyntax>
    {
        private readonly IGenerator<MethodDescription, MethodDeclarationSyntax> methodProxyGenerator;

        private readonly IGenerator<MethodDescription, ConstructorDeclarationSyntax> 
            constructorProxyGenerator;

        public ClassProxyGenerator(
            IGenerator<MethodDescription, MethodDeclarationSyntax> methodProxyGenerator,
            IGenerator<MethodDescription, ConstructorDeclarationSyntax> constructorProxyGenerator
            )
        {
            this.constructorProxyGenerator = constructorProxyGenerator;
            this.methodProxyGenerator = methodProxyGenerator;
        }

        public ClassDeclarationSyntax Generate(ClassDescription classDescription)
        {
            var methods = classDescription
                .MethodsDescriptions
                .Where(el => el.ModifiersDescriptions.ContainsAnyFrom(ModifierDescription.PUBLIC,
                                                                      ModifierDescription.PROTECTED))
                .Where(el => !el.IsConstructor)
                .Select(methodProxyGenerator.Generate)
                .ToArray();

            var constructors = classDescription
                .MethodsDescriptions
                .Where(el => el.IsConstructor)
                .Select(constructorProxyGenerator.Generate)
                .ToArray();

            var modifiers = classDescription
                .ModifiersDescriptions
                .ToTokens(isFieldModifiers: false);

            const string classRefName = "classRef";
            const string lockObjectName = "monitor";
            const string jObjectName = "jObject";

            return SyntaxFactory.ClassDeclaration(classDescription.Name)
                .AddModifiers(modifiers.ToArray())
                .AddServiceFields(classDescription, classRefName, jObjectName, lockObjectName)
                .AddServiceProperties(classDescription, classRefName, lockObjectName)
                .AddMembers(constructors)
                .AddMembers(methods);
        }
    }
}
