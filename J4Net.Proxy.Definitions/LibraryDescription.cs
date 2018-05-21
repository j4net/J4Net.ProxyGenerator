using System;
using System.Collections.Generic;
using System.Linq;

namespace DSL
{
    public class LibraryDescription : Description
    {
        private readonly Dictionary<string, ClassDescription> classes;
        private readonly Dictionary<ClassDescription, string> classFullNames = 
            new Dictionary<ClassDescription, string>();

        public LibraryDescription(string name, Dictionary<string, ClassDescription> classes)
            : base(name, null)
        {
            this.classes = classes;
            foreach (var nameDescriptionPair in classes)
                classFullNames[nameDescriptionPair.Value] = nameDescriptionPair.Key;
        }

        public void AddClassDescription(ClassDescription classDescription, string fullName)
        {
            classes[fullName] = classDescription;
            classFullNames[classDescription] = fullName;
        }

        public ClassDescription GetClassDescription(string fullName)
        {
            if (!classes.ContainsKey(fullName))
                throw new ClassDescriptionNotFoundException("Corresponding class description not found");

            return classes[fullName];
        }

        public string GetFullName(ClassDescription classDescription)
        {
            if (!classFullNames.ContainsKey(classDescription))
                throw new ClassDescriptionNotFoundException("Corresponding class description not found");

            return classFullNames[classDescription];
        }

        public IEnumerable<ClassDescription> GetClasses() => classes.Select(el => el.Value);
    }

    public class ClassDescriptionNotFoundException : Exception
    {
        public ClassDescriptionNotFoundException(string message) : base(message)
        { }
    }
}
