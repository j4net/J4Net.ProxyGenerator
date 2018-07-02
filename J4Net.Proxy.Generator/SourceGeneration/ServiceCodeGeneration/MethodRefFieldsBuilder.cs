using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.SourceGeneration.FieldGeneration;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal class MethodRefFieldsBuilder
    {
        private const string GLOBAL_REF_TYPE = "GlobalRef";

        public static IEnumerable<FieldDeclarationSyntax> Build(
            IEnumerable<MethodDescription> methodsDescription)
        {
            var fieldGenerator = new FieldGenerator();

            foreach (var description in methodsDescription)
            {
                var refField = GenerateMethodRefFieldDescription(description);
                yield return fieldGenerator.Generate(refField);
            }
        }

        private static FieldDescription GenerateMethodRefFieldDescription(
            MethodDescription methodDescription)
        {
            return new FieldDescription(
                Utils.GetMethodRefName(methodDescription),
                GLOBAL_REF_TYPE,
                new List<ModifierDescription>());
        }
    }
}
