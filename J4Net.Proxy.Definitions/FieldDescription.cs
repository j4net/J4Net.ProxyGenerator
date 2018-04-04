using System.Collections.Generic;

namespace DSL
{
    public class FieldDescription : Description
    {
        public string Type { get; }
        public FieldDescription(string name, string type, List<Modifier> modifiers)
        {
            Name = name;
            Type = type;
            Modifiers = modifiers;
        }
    }
}
