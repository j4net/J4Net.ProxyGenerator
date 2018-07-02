using System;
using System.Collections.Generic;
using DSL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using ProxyGenerator.SourceGeneration;
using ProxyGenerator.SourceGeneration.MethodGeneration;

namespace ProxyGenerator.Tests
{
    [TestFixture]
    class ClassProxyGenerator_should
    {
        private readonly ClassProxyGenerator classProxyGenerator;

        public ClassProxyGenerator_should()
        {
            var methodBodyGetter = new MethodBodyGetter("classRef", "jObject");
            var methodProxyGenerator = new MethodProxyGenerator(methodBodyGetter);
            var constructorProxyGenerator = new ConstructorProxyGenerator(methodBodyGetter);

            classProxyGenerator = new ClassProxyGenerator(methodProxyGenerator, constructorProxyGenerator);
        }

        [Test]
        public void GenerateSimpleClass()
        {
            var classDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(),
                new List<ClassDescription>(),
                isNested: false);

            var classDeclaration = classProxyGenerator.Generate(classDescription);
            var serviceMembersCount = 4;

            Assert.AreEqual("MyClass", classDeclaration.Identifier.ToString());
            Assert.AreEqual(serviceMembersCount, classDeclaration.Members.Count);
            Assert.AreEqual(0, classDeclaration.Modifiers.Count);

            TestBase.CheckModifiers(new HashSet<string>(), classDeclaration.Modifiers);
        }

        [Test]
        public void GenerateClassWithMethod()
        {
            var methodDescription = new MethodDescription(
                "myMethod",
                new List<ModifierDescription>{ModifierDescription.PUBLIC},
                new List<ParameterDescription>(),
                "void",
                isConstructor: false
            );

            var classDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>{methodDescription},
                new List<string>(),
                new List<ClassDescription>(),
                isNested: false);
            
            var classDeclaration = classProxyGenerator.Generate(classDescription);
            TestBase.CheckMethods(classDeclaration, new List<MethodDescription>{methodDescription});
        }

        [Test]
        public void GenerateClassWithNested()
        {
            var nestedClassDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(),
                new List<ClassDescription>(),
                isNested: true);

            var classDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(),
                new List<ClassDescription>{nestedClassDescription},
                isNested: false);

            var classDeclaration = classProxyGenerator.Generate(classDescription);
            TestBase.CheckNestedClasses(new HashSet<string>(), classDeclaration);
        }

        [Test]
        public void GenerateSealedClass()
        {
            var classDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>{ModifierDescription.PUBLIC, ModifierDescription.FINAL},
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(),
                new List<ClassDescription>(),
                isNested: false);

            var classDeclaration = classProxyGenerator.Generate(classDescription);
            TestBase.CheckModifiers(new HashSet<string> {"public", "sealed"}, classDeclaration.Modifiers);
        }

        [Test]
        public void GenerateMethodWithArgs()
        {
            var methodDescription = new MethodDescription(
                "myMethod",
                new List<ModifierDescription> { ModifierDescription.PUBLIC,
                    ModifierDescription.STATIC },
                new List<ParameterDescription>
                {
                    new ParameterDescription("arg0", "string[]"), 
                    new ParameterDescription("arg1", "int"),
                    new ParameterDescription("arg31", "MyType")
                },
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

            var classDeclaration = classProxyGenerator.Generate(classDescription);
            var method = GetFirstMethod(classDeclaration);

            Assert.AreNotEqual(null, method, "Class doesn't exist any methods");
            TestBase.CheckParameters(
                    methodDescription.Name,
                    methodDescription.ParametersDescription,
                    method.ParameterList.Parameters
                );
        }

        [Test]
        public void MarkVirtualNotPublicStaticFinalMethods()
        {
            var methodDescription = new MethodDescription(
                "myMethod",
                new List<ModifierDescription> { ModifierDescription.PROTECTED },
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

            var classDeclaration = classProxyGenerator.Generate(classDescription);
            var method = GetFirstMethod(classDeclaration);

            Assert.AreNotEqual(null, method, "Class doesn't exist any methods");
            TestBase.CheckModifiers(new HashSet<string>{"protected","virtual"}, method.Modifiers);
        }

        private MethodDeclarationSyntax GetFirstMethod(ClassDeclarationSyntax declaration)
        {
            foreach (var member in declaration.Members)
            {
                if (member is MethodDeclarationSyntax method)
                    return method;
            }

            return null;
        }
    }
}