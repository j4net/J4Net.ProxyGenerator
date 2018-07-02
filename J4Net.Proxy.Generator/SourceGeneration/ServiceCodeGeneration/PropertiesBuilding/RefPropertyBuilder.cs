using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration.PropertiesBuilding
{
    internal static class RefPropertyBuilder
    {
        private const string GLOBAL_REF_TYPE = "GlobalRef";

        public static IEnumerable<PropertyDeclarationSyntax> Build(
            IEnumerable<MethodDescription> methodsDescription,
            string classRefName,
            string lockObjectName)
        {
            yield return BuildProperty(classRefName, GenerateFindClassExpression(), lockObjectName);

            foreach (var description in methodsDescription)
            {
                var isStatic = description.ModifiersDescriptions.Contains(ModifierDescription.STATIC);
                var methodCallable = SyntaxFactory.IdentifierName(
                    $"Get{(isStatic ? "Static" : "")}Method");

                var args = new List<ExpressionSyntax>
                {
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(description.IsConstructor ? "<init>" : description.Name)),

                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(
                            ServiceUtils.GetMethodJavaSignature(description)))
                };

                yield return BuildProperty(
                    ServiceUtils.GetMethodRefName(description),
                    RoslynUtils.CallExpression(methodCallable, args),
                    lockObjectName);
            }
        }

        private static PropertyDeclarationSyntax BuildProperty(
            string fieldName,
            ExpressionSyntax fieldAssignmentExpression,
            string lockObjectName,
            string propertyName = null)
        {
            var fieldIdentifierName = SyntaxFactory.IdentifierName(fieldName);

            var propertyGetter = SyntaxFactory.Block()
                .AddNotNullStatement(fieldName, fieldIdentifierName)
                .AddLockStatement(lockObjectName, fieldName, fieldIdentifierName, 
                    fieldAssignmentExpression);

            var type = SyntaxFactory.ParseTypeName(GLOBAL_REF_TYPE);
            var accessors = SyntaxFactory
                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithBody(SyntaxFactory.Block(propertyGetter));

            if (propertyName == null)
                propertyName = fieldName.UppercaseFirstLetter();

            return SyntaxFactory
                .PropertyDeclaration(type, propertyName)
                .AddAccessorListAccessors(accessors)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        }

        #region auxiliary methods
        private static BlockSyntax AddNotNullStatement(this BlockSyntax blockSyntax, string fieldName,
            ExpressionSyntax fieldIdentifierName)
        {
            var fieldNotNullStatement = RoslynUtils.CheckNullStatement(
                fieldName,
                SyntaxFactory.ReturnStatement(fieldIdentifierName),
                invert: true
            );

            return blockSyntax.AddStatements(fieldNotNullStatement);
        }

        private static BlockSyntax AddLockStatement(
            this BlockSyntax blockSyntax,
            string lockObjectName,
            string fieldName,
            ExpressionSyntax fieldIdentifierName,
            ExpressionSyntax fieldAssignmentExpression)
        {
            var assignmentStatement = RoslynUtils.AssignmentExpression(fieldName, fieldAssignmentExpression);
            var fieldNullStatement = RoslynUtils.CheckNullStatement(fieldName, 
                assignmentStatement.ToStatement());

            var lockStat = RoslynUtils.LockStatement(
                SyntaxFactory.IdentifierName(lockObjectName),
                SyntaxFactory.Block(fieldNullStatement, SyntaxFactory.ReturnStatement(fieldIdentifierName))
            );

            return blockSyntax.AddStatements(lockStat);
        }

        private static ExpressionSyntax GenerateFindClassExpression()
        {
            var getEnvCallExpr = RoslynUtils.CallExpression(
                SyntaxFactory.IdentifierName("JvmManager")
                    .MemberAccessExpression("INSTANCE")
                    .MemberAccessExpression("GetEnv"));

            return RoslynUtils.CallExpression(
                getEnvCallExpr.MemberAccessExpression("FindClass"));
        }
        #endregion
    }
}
