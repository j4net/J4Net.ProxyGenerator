using System.Collections.Generic;

namespace DSL
{
    public class ClassDescription : Description
    {
        public readonly string PackageName;
        public readonly List<FieldDescription> FieldsDescriptions;
        public readonly List<MethodDescription> MethodsDescriptions;
        public readonly List<ClassDescription> DependenciesDescriptions;
        public readonly List<ClassDescription> NestedClassesDescriptions;
        public readonly bool IsNested;

        public ClassDescription(
            string name,
            string packageName,
            List<ModifierDescription> modifiersesDescriptions,
            List<FieldDescription> fieldsDescriptions,
            List<MethodDescription> methodsDescriptions,
            List<ClassDescription> dependenciesDescriptions,
            List<ClassDescription> nestedClassesDescriptions,
            bool isNested) 
                : base(name, modifiersesDescriptions)
        {
            PackageName = packageName;
            FieldsDescriptions = fieldsDescriptions;
            MethodsDescriptions = methodsDescriptions;
            DependenciesDescriptions = dependenciesDescriptions;
            IsNested = isNested;
            NestedClassesDescriptions = nestedClassesDescriptions;
        }
    }
}