using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    internal class ConstructorProxyGenerator 
        : IGenerator<MethodDescription, ConstructorDeclarationSyntax>
    {
        private readonly IMethodBodyGetter methodBodyGetter;

        public ConstructorProxyGenerator(IMethodBodyGetter methodBodyGetter)
        {
            this.methodBodyGetter = methodBodyGetter;
        }

        public ConstructorDeclarationSyntax Generate(MethodDescription description)
        {
            var modifiers = description.ModifiersDescriptions
                .ToTokens(isFieldModifiers: false)
                .ToArray();

            return SyntaxFactory
                .ConstructorDeclaration(description.Name)
                .AddModifiers(modifiers)
                .AddParameters(description.ParametersDescription)
                .WithBody(methodBodyGetter.GetBodyFor(description));
        }
    }
}
