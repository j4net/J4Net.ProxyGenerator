using System.Collections.Generic;

namespace DSL
{
    public class FieldDescription : Description
    {
        public readonly string Type;
        public FieldDescription(string name, string type, List<ModifierDescription> modifiersDescriptions)
            : base(name, modifiersDescriptions) => Type = type;
    }
}
