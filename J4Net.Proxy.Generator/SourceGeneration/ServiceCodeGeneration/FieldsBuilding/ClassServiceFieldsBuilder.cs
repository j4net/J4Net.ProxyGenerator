using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.SourceGeneration.FieldGeneration;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration.FieldsBuilding
{
    internal class ClassServiceFieldsBuilder
    {
        private const string GLOBAL_REF_TYPE = "GlobalRef";

        public static IEnumerable<FieldDeclarationSyntax> Build(
            string classRefName,
            string jObjectName,
            string lockObjectName)
        {
            var classRefDescription = new FieldDescription(
                classRefName,
                GLOBAL_REF_TYPE,
                new List<ModifierDescription>());

            var jObjectDescription = new FieldDescription(
                jObjectName,
                GLOBAL_REF_TYPE,
                new List<ModifierDescription> { ModifierDescription.FINAL });

            var monitorDescription = new FieldDescription(
                lockObjectName,
                "object",
                new List<ModifierDescription>
                {
                    ModifierDescription.FINAL,
                    ModifierDescription.STATIC
                });
            
            var fieldGenerator = new FieldGenerator();

            return new[] { classRefDescription, jObjectDescription, monitorDescription }
                .Select(el =>
                {
                    if (el.Name == "monitor")
                        fieldGenerator = fieldGenerator.InitializedWith(GetMonitorFieldInitExpression());
                    return fieldGenerator.Generate(el);
                })
                .ToArray();
        }

        private static ObjectCreationExpressionSyntax GetMonitorFieldInitExpression()
        {
            return SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.Token(SyntaxKind.NewKeyword),
                SyntaxFactory.ParseTypeName("object"),
                SyntaxFactory.ArgumentList(),
                null
            );
        }
    }
}
