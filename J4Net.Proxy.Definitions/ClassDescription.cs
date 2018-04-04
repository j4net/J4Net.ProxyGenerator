using System;
using System.Collections.Generic;
using System.Linq;

namespace DSL
{
    public class ClassDescription : Description
    {
        public string PackageName { get; private set; }
        public string FullName { get; private set; }
        public List<FieldDescription> Fields { get; }
        public List<MethodDescription> Methods { get; }
        public List<ClassDescription> Dependencies { get; }
        public List<ClassDescription> NestedClasses { get; }
        public readonly bool IsNested;

        public ClassDescription(
            string fullName,
            List<Modifier> modifiers,
            List<FieldDescription> fields,
            List<MethodDescription> methods,
            List<ClassDescription> dependencies,
            List<ClassDescription> nestedClasses,
            bool isNested)
        {
            Modifiers = modifiers;
            Fields = fields;
            Methods = methods;
            Dependencies = dependencies;
            IsNested = isNested;
            NestedClasses = nestedClasses;
            Parse(fullName);
        }

        private void Parse(string fullName)
        {
            const char nestedClassesSeparator = '$';
            const char javaPathSeparator = '.';

            FullName = fullName.Replace(nestedClassesSeparator, javaPathSeparator);

            if (IsNested)
            {
                var splitedFullName = fullName.Split(nestedClassesSeparator);
                Name = splitedFullName[splitedFullName.Length - 1];
            }
            else
            {
                var splitedFullName = fullName.Split(new []{javaPathSeparator}, 
                    StringSplitOptions.RemoveEmptyEntries);
                var splitedNameSize = splitedFullName.Length;

                Name = splitedFullName[splitedNameSize - 1];
                PackageName = string.Join(".", splitedFullName.Take(splitedNameSize - 1).ToArray());
            }
        }
    }
}