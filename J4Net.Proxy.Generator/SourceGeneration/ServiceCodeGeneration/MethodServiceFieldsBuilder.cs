using System;
using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.SourceGeneration.FieldGeneration;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal class MethodServiceFieldsBuilder
    {
        private const string GLOBAL_REF_TYPE = "GlobalRef";

        public static FieldDeclarationSyntax[] Build(
            IEnumerable<MethodDescription> methodsDescription)
        {
            var fieldGenerator = new FieldGenerator();

            return methodsDescription
                .SelectMany(el => new[]
                {
                    Tuple.Create(el, GenerateMethodServiceFieldDescription(el, "Ref", GLOBAL_REF_TYPE)),
                    Tuple.Create(el, GenerateMethodServiceFieldDescription(el, "Signature", "string"))
                })
                .Select(el =>
                {
                    if (el.Item2.Name.Contains("Signature"))
                        fieldGenerator = fieldGenerator.InitializedWith(GetSignatureFieldsInitExpression(el.Item1));
                    return fieldGenerator.Generate(el.Item2);
                })
                .ToArray();
        }

        private static FieldDescription GenerateMethodServiceFieldDescription(
            MethodDescription methodDescription,
            string fieldDestinationMessage,
            string fieldType)
        {
            return new FieldDescription(
                Utils.GetServiceFieldName(methodDescription, fieldDestinationMessage),
                fieldType,
                new List<ModifierDescription>());
        }

        private static ExpressionSyntax GetSignatureFieldsInitExpression(MethodDescription description)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(Utils.GetServiceSignature(description))
                );
        }
    }
}
