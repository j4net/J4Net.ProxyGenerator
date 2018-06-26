using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DSL;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration
{
    internal class LibraryProxyGenerator : IGenerator<LibraryDescription, ProxySource>
    {
        private readonly IGenerator<ClassDescription, ClassDeclarationSyntax> classProxyGenerator;

        public LibraryProxyGenerator(IGenerator<ClassDescription, ClassDeclarationSyntax> classProxyGenerator)
        {
            this.classProxyGenerator = classProxyGenerator;
        }

        public ProxySource Generate(LibraryDescription libraryDescription)
        {
            var toProxyClasses = new Queue<ClassDescription>();
            var proxyClassNames = new HashSet<string>();
            var namespaceUnits = new Dictionary<string, NamespaceUnit>();

            var classes = libraryDescription.GetClasses();
            toProxyClasses.EnqueueRange(classes);
            proxyClassNames.AddMany(classes.Select(libraryDescription.GetFullName));

            while (toProxyClasses.Count != 0)
            {
                var classDescription = toProxyClasses.Dequeue();

                if (classDescription.IsNested)
                    continue;

                ProcessClassDescription(classDescription, proxyClassNames, libraryDescription, toProxyClasses, namespaceUnits);
            }

            return new ProxySource(namespaceUnits.Values);
        }


        public void ProcessClassDescription(
            ClassDescription classDescription, 
            HashSet<string> proxyClassNames, 
            LibraryDescription libraryDescription,
            Queue<ClassDescription> toProxyClasses,
            Dictionary<string, NamespaceUnit> namespaceUnits)
        {
            var toProxyDependencies = classDescription
                .DependenciesNames
                .Where(el => !proxyClassNames.Contains(el))
                .Select(libraryDescription.GetClassDescription);

            toProxyClasses.EnqueueRange(toProxyDependencies);
            proxyClassNames.Add(libraryDescription.GetFullName(classDescription));

            var classProxy = classProxyGenerator.Generate(classDescription);

            var nestedClassProxies = classDescription.NestedClassesDescriptions
                .Select(el => classProxyGenerator.Generate(el))
                .ToArray();

            classProxy = classProxy.AddMembers(nestedClassProxies);
            AddClassToNamespaceUnits(libraryDescription, classProxy, classDescription, namespaceUnits);
        }

        private void AddClassToNamespaceUnits(
            LibraryDescription libraryDescription,
            MemberDeclarationSyntax classProxy,
            ClassDescription classDescription,
            IDictionary<string, NamespaceUnit> namespaceUnits)
        {
            var packageName = classDescription.PackageName;
            if (!namespaceUnits.ContainsKey(packageName))
                namespaceUnits[packageName] = new NamespaceUnit(packageName);

            var newDeclaration = namespaceUnits[packageName]
                .NamespaceDeclaration
                .AddMembers(classProxy);

            namespaceUnits[packageName].NamespaceDeclaration = newDeclaration;

            var serviceUsings = new[]
            {
                "J4Net.Core",
                "J4Net.Core.JNICore.Interface"
            };

            var usings = classDescription
                .DependenciesNames
                .Select(libraryDescription.GetClassDescription)
                .Select(el => el.PackageName)
                .Where(el => el != classDescription.PackageName)
                .Concat(serviceUsings)
                .Select(el => SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(el)));

            namespaceUnits[packageName].Usings.AddRange(usings);
        }
    }
}