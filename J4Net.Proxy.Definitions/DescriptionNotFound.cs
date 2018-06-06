using System;

namespace DSL
{
    public class DescriptionNotFoundException : Exception
    {
        public DescriptionNotFoundException(string message) : base(message)
        { }
    }
}
