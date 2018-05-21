using System.Collections.Generic;
using System.Linq;
using DSL;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using ProxyGenerator.SourceGeneration;

namespace ProxyGenerator.Tests
{
    [TestFixture]
    class ProxyLibraryGenerator_integration_should
    {
        [Test]
        public void CallClassProxyGeneratorForDependencies()
        {
            var dependenciesClasses = new List<ClassDescription>();
            var dependenciesFullNames = new List<string>
            {
                "packageName.DependenceClass0",
                "packageName.DependenceClass1",
                "packageName.DependenceClass2"
            };

            for (int i = 0; i < 3; i++)
            {
                var dependence = new ClassDescription(
                    $"DependenceClass{i}",
                    "packageName",
                    new List<ModifierDescription>(),
                    new List<FieldDescription>(),
                    new List<MethodDescription>(),
                    new List<string>(),
                    new List<ClassDescription>(),
                    isNested: false);

                dependenciesClasses.Add(dependence);
            }

            var mainClassDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                dependenciesFullNames,
                new List<ClassDescription>(),
                isNested: false);

            var libraryDescripton = new LibraryDescription("MyLibrary", new Dictionary<string, ClassDescription>());
            libraryDescripton.AddClassDescription(mainClassDescription, 
                "packageName.MyClass");
            for (int i = 0; i < 3; i++)
                libraryDescripton.AddClassDescription(dependenciesClasses[i], dependenciesFullNames[i]);

            var classProxyGenerator = new MockClassProxyGenerator();
            new LibraryProxyGenerator(classProxyGenerator).Generate(libraryDescripton);

            Assert.AreEqual(4, classProxyGenerator.CallsArguments.Count, "A lot of classes was proxy");
            Assert.AreEqual(
                "packageName.MyClass", 
                libraryDescripton.GetFullName(classProxyGenerator.CallsArguments[0]),
                "ClassProxyGenerator call order incorrect");

            for (int i = 1; i < 4; i++)
            {
                var name = libraryDescripton.GetFullName(classProxyGenerator.CallsArguments[i]);
                Assert.AreEqual(dependenciesFullNames[i-1], name, "ClassProxyGenerator call order incorrect");
            }
        }

        [Test]
        public void ClassProxyGeneratorForNestedClasses()
        {
            var nestedClassDescription = new ClassDescription(
                "NestedClass",
                null,
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(),
                new List<ClassDescription>(),
                isNested: true);

            var mainClassDescription = new ClassDescription(
                "MyClass",
                "packageName",
                new List<ModifierDescription>(),
                new List<FieldDescription>(),
                new List<MethodDescription>(),
                new List<string>(), 
                new List<ClassDescription>{nestedClassDescription},
                isNested: false);

            var libraryDescripton = new LibraryDescription("MyLibrary", new Dictionary<string, ClassDescription>());
            libraryDescripton.AddClassDescription(mainClassDescription,
                "packageName.MyClass");

            libraryDescripton.AddClassDescription(nestedClassDescription, "packageName.MyClass$NestedClass");

            var classProxyGenerator = new MockClassProxyGenerator();
            new LibraryProxyGenerator(classProxyGenerator).Generate(libraryDescripton);

            
        }
    }

    public class MockClassProxyGenerator : IGenerator<ClassDescription, ClassDeclarationSyntax>
    {
        public List<ClassDescription> CallsArguments = new List<ClassDescription>();
        
        public ClassDeclarationSyntax Generate(ClassDescription description)
        {
            CallsArguments.Add(description);
            return SyntaxFactory.ClassDeclaration($"Class{CallsArguments.Count}");
        }
    }
}
