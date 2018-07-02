using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.Infrastructure
{
    internal static class RoslynUtils
    {
        #region expressions
        public static ExpressionSyntax MemberAccessExpression(
            this ExpressionSyntax sourceExpression, string memberName)
        {
            return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                sourceExpression,
                SyntaxFactory.IdentifierName(memberName));
        }

        public static ExpressionSyntax CallExpression(ExpressionSyntax callable,
            IEnumerable<ExpressionSyntax> arguments = null)
        {
            if (arguments == null)
                arguments = Enumerable.Empty<ExpressionSyntax>();

            var argumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(
                arguments.Select(SyntaxFactory.Argument)));

            return SyntaxFactory.InvocationExpression(callable, argumentList);
        }

        public static ExpressionSyntax NewExpression(string type,
            InitializerExpressionSyntax initExpression)
        {
            return SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.Token(SyntaxKind.NewKeyword),
                SyntaxFactory.ParseTypeName(type),
                null,
                initExpression
            );
        }

        public static InitializerExpressionSyntax CollectionInitExpression(
            IEnumerable<ExpressionSyntax> initValues)
        {
            return SyntaxFactory.InitializerExpression(
                SyntaxKind.CollectionInitializerExpression,
                SyntaxFactory.SeparatedList(initValues));
        }

        public static ExpressionSyntax AssignmentExpression(string assignmentVariableName,
            ExpressionSyntax assignableExpression)
        {
            return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(assignmentVariableName),
                assignableExpression
            );
        }
        #endregion

        #region statements
        public static StatementSyntax CheckNullStatement(string checkVariableName,
            StatementSyntax trueStatement, bool invert = false)
        {
            var kind = invert
                ? SyntaxKind.EqualsExpression
                : SyntaxKind.NotEqualsExpression;

            return SyntaxFactory.IfStatement(
                SyntaxFactory.BinaryExpression(
                    kind,
                    SyntaxFactory.IdentifierName(checkVariableName),
                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression,
                        SyntaxFactory.Token(SyntaxKind.NullKeyword))
                ),
                SyntaxFactory.Block(trueStatement)
            );
        }

        public static StatementSyntax LockStatement(
            ExpressionSyntax parenthesInsideExpression,
            BlockSyntax body)
        {
            return SyntaxFactory.LockStatement(
                SyntaxFactory.Token(SyntaxKind.LockKeyword),
                SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                parenthesInsideExpression,
                SyntaxFactory.Token(SyntaxKind.CloseParenToken),
                body
            );
        }

        public static StatementSyntax UsingStatement(
            string usingVariableType,
            string usingVariableName,
            ExpressionSyntax usingVariableInitExpression,
            StatementSyntax body)
        {
            var variableDeclaration =
                SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(usingVariableType),
                    SyntaxFactory.SeparatedList(new[]
                    {
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(usingVariableName),
                            null,
                            SyntaxFactory.EqualsValueClause(usingVariableInitExpression))
                    }));

            return SyntaxFactory.UsingStatement(
                SyntaxFactory.Token(SyntaxKind.UsingKeyword),
                SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                variableDeclaration,
                null,
                SyntaxFactory.Token(SyntaxKind.CloseParenToken),
                body);
        }
        #endregion

        public static StatementSyntax ToStatement(this ExpressionSyntax expression)
        {
            return SyntaxFactory.ExpressionStatement(expression);
        }
    }
}
