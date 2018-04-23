using System.Collections.Generic;

namespace DSL
{
    public abstract class Description
    {
        public readonly string Name;
        public readonly List<ModifierDescription> ModifiersDescriptions;

        protected Description(string name, List<ModifierDescription> modifiersDescriptions)
        {
            Name = name;
            ModifiersDescriptions = modifiersDescriptions;
        }
    }
}
