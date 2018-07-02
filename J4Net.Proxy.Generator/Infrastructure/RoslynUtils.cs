using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProxyGenerator.Infrastructure
{
    internal static class RoslynUtils
    {
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

        public static StatementSyntax AssignmentStatement(string assignmentVariableName,
            ExpressionSyntax assignableExpression)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(assignmentVariableName),
                    assignableExpression
                )
            );
        }

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

        public static StatementSyntax ToStatement(this ExpressionSyntax expression)
        {
            return SyntaxFactory.ExpressionStatement(expression);
        }
    }
}
