using System;
using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProxyGenerator.Infrastructure;
using ProxyGenerator.SourceGeneration.ServiceCodeGeneration;

namespace ProxyGenerator.SourceGeneration.MethodGeneration
{
    internal class MethodBodyGetter : IMethodBodyGetter
    {
        private readonly string classRefName;
        private readonly string jObjectName;

        public MethodBodyGetter(string classRefName, string jObjectName)
        {
            this.classRefName = classRefName;
            this.jObjectName = jObjectName;
        }

        public BlockSyntax GetBodyFor(MethodDescription methodDescription)
        {
            var body = methodDescription.IsConstructor 
                ? GetConstrctorBody(methodDescription) 
                : GetMethodBody(methodDescription);

            return WrapByUsingStatements(methodDescription.ParametersDescription, body);
        }

        private BlockSyntax GetConstrctorBody(MethodDescription description)
        {
            var callJvmExpression = CallJvmExpression(
                "NewObject",
                classRefName,
                ServiceUtils.GetMethodRefProperty(description),
                description.ParametersDescription);

            return SyntaxFactory.Block(
                RoslynUtils
                    .AssignmentExpression(jObjectName, callJvmExpression)
                    .ToStatement());
        }

        private BlockSyntax GetMethodBody(MethodDescription description)
        {
            var isStatic = description.ModifiersDescriptions.Contains(ModifierDescription.STATIC);
            string type;
            string MethodBuilder(string ret) => $"Call{(isStatic ? "Static" : "")}{ret}Method";

            if (description.ReturnType == "void" || description.ReturnType == "int")
                type = description.ReturnType.UppercaseFirstLetter();
            else
                type = "Object";

            var callJvmExpression = CallJvmExpression(
                MethodBuilder(type),
                jObjectName,
                ServiceUtils.GetMethodRefProperty(description),
                description.ParametersDescription
            );

            var statement = (description.ReturnType == "void")
                ? callJvmExpression.ToStatement()
                : SyntaxFactory.ReturnStatement(callJvmExpression);

            return SyntaxFactory.Block(statement);
        }

        private ExpressionSyntax CallJvmExpression(string method, string callableObject,
            string methodRefName, IEnumerable<ParameterDescription> arguments)
        {
            var newObjExpr = SyntaxFactory
                .IdentifierName("Env")
                .MemberAccessExpression(method);

            var args = new List<ExpressionSyntax>
            {
                SyntaxFactory.IdentifierName(callableObject).MemberAccessExpression("Ptr"),
                SyntaxFactory
                    .IdentifierName(methodRefName)
                    .MemberAccessExpression("Ptr"),
            };

            args.AddRange(arguments.Select(GenerateJValueExpressionFor));
            return RoslynUtils.CallExpression(newObjExpr, args);
        }

        private ExpressionSyntax GenerateJValueExpressionFor(ParameterDescription parameter)
        {
            var isPrimitive = JavaUtils.IsPrimitive(parameter.Type);
            ExpressionSyntax jValueParameter = SyntaxFactory.IdentifierName(isPrimitive
                ? parameter.Name
                : $"{SyntaxFactory.IdentifierName(parameter.Name)}_using");
            if (!isPrimitive)
                jValueParameter = jValueParameter.MemberAccessExpression("Ptr");

            return RoslynUtils.NewExpression(
                "JValue",
                RoslynUtils.CollectionInitExpression(new[]
                {
                    RoslynUtils.AssignmentExpression(
                        JavaUtils.GetTypeName(parameter.Type),
                        jValueParameter)
                }));
        }

        private BlockSyntax WrapByUsingStatements(
            IEnumerable<ParameterDescription> parametersDescription,
            BlockSyntax body)
        {
            StatementSyntax currentStatement = null;

            foreach (var parameter in parametersDescription)
            {
                if (JavaUtils.IsPrimitive(parameter.Type)) continue;

                var childBody = currentStatement ?? body;

                currentStatement = RoslynUtils.UsingStatement(
                    "var",
                    $"{parameter.Name}_using",
                    GetExpressionFor(parameter),
                    childBody);
            }

            return currentStatement == null
                ? body
                : SyntaxFactory.Block(currentStatement);
        }

        private ExpressionSyntax GetExpressionFor(ParameterDescription parameter)
        {
            var jvmEnvExpr = SyntaxFactory.IdentifierName("Env");

            var jObjectsGettingExpressions = new Dictionary<string, ExpressionSyntax>
            {
                {"string", RoslynUtils.CallExpression(
                    jvmEnvExpr.MemberAccessExpression("NewStringUtf"), new List<ExpressionSyntax>
                    {
                        SyntaxFactory.IdentifierName(parameter.Name)
                    })}
            };

            return jObjectsGettingExpressions[parameter.Type];
        }
    }
}