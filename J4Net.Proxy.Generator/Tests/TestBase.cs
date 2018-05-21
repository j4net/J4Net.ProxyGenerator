using System;
using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace ProxyGenerator.Tests
{
    public static class TestBase
    {
        public static void CheckMethods(ClassDeclarationSyntax classDeclaration,
                 IEnumerable<MethodDescription> expectedMethods)
        {
            var methodsDeclaration = classDeclaration
                .Members
                .OfType<MethodDeclarationSyntax>();

            var expected = expectedMethods.Count();
            var actual = methodsDeclaration.Count();
            Assert.AreEqual(expected, actual,
                $"Expected {expected} methods, but was {actual} methods");

            var expectedMethodGenerator = expectedMethods.GetEnumerator();
            expectedMethodGenerator.MoveNext();
            foreach (var methodDeclaration in methodsDeclaration)
            {
                CheckMethod(expectedMethodGenerator.Current, methodDeclaration);
                expectedMethodGenerator.MoveNext();
            }
        }

        public static void CheckMethod(MethodDescription expectedMethod, MethodDeclarationSyntax methodDeclaration)
        {
            Assert.AreEqual(expectedMethod.Name, methodDeclaration.Identifier.ToString(),
                $"Method name expected: \"{expectedMethod.Name}\", but was \"{methodDeclaration.Identifier}\"");
            Assert.AreEqual(expectedMethod.ReturnType, methodDeclaration.ReturnType.ToString(),
                $"Method return type expected \"{expectedMethod.ReturnType}\"," +
                $"but was \"{methodDeclaration.ReturnType}\"");

            var modifiers = expectedMethod
                .ModifiersDescriptions
                .Select(ModifierToString)
                .Where(el => el != null)
                .ToList();
            CheckModifiers(new HashSet<string>(modifiers), methodDeclaration.Modifiers);
            CheckParameters(expectedMethod.Name, expectedMethod.ParametersDescription,
                methodDeclaration.ParameterList.Parameters);
        }

        public static void CheckParameters(string methodName, List<ParameterDescription> expectedParameters,
            SeparatedSyntaxList<ParameterSyntax> actualParameters)
        {
            Assert.AreEqual(expectedParameters.Count, actualParameters.Count,
                $"Expected {expectedParameters.Count} parameters, but was {actualParameters.Count} parameters");

            foreach (var actualParameter in actualParameters)
                Assert.IsTrue(ContainsParameter(expectedParameters, actualParameter),
                    $"Method \"{methodName}\" does not contain \"{actualParameter.Identifier}\" with " +
                    $"\"{actualParameter.Type}\" type.");
        }

        public static bool ContainsParameter(List<ParameterDescription> collection, ParameterSyntax parameter)
        {
            foreach (var parameterDescription in collection)
            {
                if (parameterDescription.Type != parameter.Type.ToString()
                    || parameterDescription.Name != parameter.Identifier.ToString())
                    continue;

                return true;
            }

            return false;
        }

        public static void CheckModifiers(HashSet<string> exepectedModifiers, SyntaxTokenList actualModifiers)
        {
            Assert.AreEqual(actualModifiers.Count, exepectedModifiers.Count,
                $"Expected {exepectedModifiers.Count} modifiers, but was {actualModifiers.Count} modifiers");

            foreach (var actualModifier in actualModifiers)
            {
                Assert.IsTrue(
                    exepectedModifiers.Contains(actualModifier.ToString()),
                    $"Extra modifier found. Modifier \"{actualModifier}\"");
            }
        }

        public static void CheckNestedClasses(HashSet<string> expectedClassesNames,
            ClassDeclarationSyntax classDeclaration)
        {
            var actualClassesNames = classDeclaration
                .Members
                .OfType<ClassDeclarationSyntax>()
                .Select(el => el.Identifier.ToString())
                .ToList();

            Assert.AreEqual(expectedClassesNames.Count, actualClassesNames.Count,
                $"Expected {expectedClassesNames.Count} modifiers, but was {actualClassesNames.Count} modifiers");

            foreach (var actualModifier in actualClassesNames)
            {
                Assert.IsTrue(
                    expectedClassesNames.Contains(actualModifier),
                    $"Extra class found. Class \"{actualModifier}\"");
            }
        }

        public static string ModifierToString(ModifierDescription description)
        {
            string result = null;

            switch (description)
            {
                case ModifierDescription.PUBLIC:
                {
                    result = "public";
                    break;
                }
                case ModifierDescription.ABSTRACT:
                {
                    result = "abstaract";
                    break;
                }
                case ModifierDescription.FINAL:
                {
                    result = "sealed";
                    break;
                }
                case ModifierDescription.PROTECTED:
                {
                    result = "protected";
                    break;
                }
                case ModifierDescription.STATIC:
                {
                    result = "static";
                    break;
                }
                case ModifierDescription.NATIVE:
                    break;
                case ModifierDescription.SYNCHRONIZED:
                    break;
                case ModifierDescription.TRANSIENT:
                    break;
                case ModifierDescription.VOLATILE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(description), description, null);
            }

            return result;
        }
    }
}
