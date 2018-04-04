using System.Collections.Generic;

namespace DSL
{
    public class LibraryDescription
    {
        public List<ClassDescription> Classes;

        public LibraryDescription(List<ClassDescription> classes)
        {
            Classes = classes;
        }
    }
}
