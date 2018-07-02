using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using ProxyGenerator.SourceGeneration;
using ProxyGenerator.SourceGeneration.MethodGeneration;

namespace ProxyGenerator.Tests
{
    [TestFixture]
    class ClassProxyGenerator_integration_should
    {
        [Test]
        public void MethodBodyGetterCall()
        {
            var methodDescription = new MethodDescription(
                "myMethod",
                new List<ModifierDescription> { ModifierDescription.PUBLIC },
                new List<ParameterDescription>(),
                "void",
                isConstructor: false
            );

            var classDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription> { methodDescription },
                new List<string>(),
                new List<ClassDescription>(),
                isNested: false);

            var methodBodyGetter = new MockMethodBodyGetter();
            var methodProxyGenerator = new MethodProxyGenerator(methodBodyGetter);
            var constructorProxyGenerator = new ConstructorProxyGenerator(methodBodyGetter);

            var classProxyGenerator = new ClassProxyGenerator(methodProxyGenerator, constructorProxyGenerator);
            classProxyGenerator.Generate(classDescription);

            Assert.AreEqual(1, methodBodyGetter.CallsCount);
            Assert.AreEqual("myMethod", methodBodyGetter.CallArgumentName);
        }
    }

    public class MockMethodBodyGetter : IMethodBodyGetter
    {
        public int CallsCount;
        public string CallArgumentName;

        public BlockSyntax GetBodyFor(MethodDescription methodDescription)
        {
            CallsCount++;
            CallArgumentName = methodDescription.Name;
            return SyntaxFactory.Block();
        }
    }
}
