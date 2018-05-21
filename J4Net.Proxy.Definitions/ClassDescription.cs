using System.Collections.Generic;

namespace DSL
{
    public class ClassDescription : Description
    {
        public readonly string PackageName;
        public readonly List<FieldDescription> FieldsDescriptions;
        public readonly List<MethodDescription> MethodsDescriptions;
        public readonly List<string> DependenciesNames;
        public readonly List<ClassDescription> NestedClassesDescriptions;
        public readonly bool IsNested;

        public ClassDescription(
            string name,
            string packageName,
            List<ModifierDescription> modifiersesDescriptions,
            List<FieldDescription> fieldsDescriptions,
            List<MethodDescription> methodsDescriptions,
            List<string> dependenciesNames,
            List<ClassDescription> nestedClassesDescriptions,
            bool isNested) 
                : base(name, modifiersesDescriptions)
        {
            PackageName = packageName;
            FieldsDescriptions = fieldsDescriptions;
            MethodsDescriptions = methodsDescriptions;
            DependenciesNames = dependenciesNames;
            IsNested = isNested;
            NestedClassesDescriptions = nestedClassesDescriptions;
        }
    }
}