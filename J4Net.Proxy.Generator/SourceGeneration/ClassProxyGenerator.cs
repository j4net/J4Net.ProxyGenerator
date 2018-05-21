using System.Collections.Generic;
using System.Linq;
using ProxyGenerator.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DSL;
using Microsoft.CodeAnalysis;

namespace ProxyGenerator.SourceGeneration
{
    internal class ClassProxyGenerator : IGenerator<ClassDescription, ClassDeclarationSyntax>
    {
        private readonly IMethodBodyGetter methodBodyGetter;

        public ClassProxyGenerator(IMethodBodyGetter methodBodyGetter) => this.methodBodyGetter = methodBodyGetter;

        public ClassDeclarationSyntax Generate(ClassDescription classDescription)
        {
            var modifiers = classDescription.ModifiersDescriptions
                .Select(ModifiersConverter.ModifierDescriptionToSyntaxKind)
                .Where(el => el != SyntaxKind.None)
                .Select(SyntaxFactory.Token);

            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.Name)
                .AddModifiers(modifiers.ToArray());

            var methods = GenerateMethods(
                classDescription
                    .MethodsDescriptions
                    .Where(el => el.ModifiersDescriptions.ContainsAnyFrom(ModifierDescription.PUBLIC, 
                                                                          ModifierDescription.PROTECTED))
                );

            return classDeclaration.AddMembers(methods.ToArray());
        }

        private IEnumerable<MethodDeclarationSyntax> GenerateMethods(IEnumerable<MethodDescription> methodDescriptions)
        {
            foreach (var methodDescription in methodDescriptions)
            {
                var modifiersDescription = methodDescription.ModifiersDescriptions;

                var modifiers = modifiersDescription
                    .Select(ModifiersConverter.ModifierDescriptionToSyntaxKind)
                    .Where(el => el != default(SyntaxKind))
                    .Select(SyntaxFactory.Token);

                modifiers = AddDefaultModifiers(modifiers, modifiersDescription);

                var returnType = SyntaxFactory.ParseTypeName(methodDescription.ReturnType);

                if (methodDescription.ReturnType == "void")
                    returnType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));

                var methodDeclaration = SyntaxFactory
                    .MethodDeclaration(returnType, methodDescription.Name)
                    .AddModifiers(modifiers.ToArray());

                var parameters = methodDescription.ParametersDescription
                    .Select(el => new
                        {
                            Name = SyntaxFactory.Identifier(el.Name),
                            Type = SyntaxFactory.ParseTypeName(el.Type)
                        })
                    .Select(el => SyntaxFactory.Parameter(el.Name).WithType(el.Type));

                methodDeclaration = methodDeclaration
                    .AddParameterListParameters(parameters.ToArray());

                yield return methodDeclaration.WithBody(methodBodyGetter.GetBodyFor(methodDeclaration));
            }
        }

        private IEnumerable<SyntaxToken> AddDefaultModifiers(IEnumerable<SyntaxToken> modifiers,
            IEnumerable<ModifierDescription> modifiersDescription)
        {
            if (!modifiersDescription.ContainsAnyFrom(ModifierDescription.FINAL, ModifierDescription.STATIC,
                    ModifierDescription.PUBLIC))
                modifiers = modifiers.Concat(new[] { SyntaxFactory.Token(SyntaxKind.VirtualKeyword) });

            return modifiers;
        }
    }
}
