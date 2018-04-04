using System.Collections.Generic;

namespace DSL
{
    public abstract class Description
    {
        public string Name { get; protected set; }
        public List<Modifier> Modifiers { get; protected set; }
    }
}
