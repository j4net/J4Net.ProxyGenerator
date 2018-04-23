using System.Collections.Generic;

namespace DSL
{
    public class LibraryDescription : Description
    {
        public readonly List<ClassDescription> Classes;
        public LibraryDescription(string name, List<ClassDescription> classes) 
            : base(name, null) => Classes = classes;
    }
}
