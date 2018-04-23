using System.Collections.Generic;

namespace DSL
{
    public class MethodDescription : Description
    {
        public readonly List<ParametrDescription> ParametersDescription;
        public readonly string ReturnType;

        public MethodDescription(string name, List<ModifierDescription> modifiersDescriptions,
            List<ParametrDescription> parametersDescription, string returnType)
                : base(name, modifiersDescriptions)
        {
            ParametersDescription = parametersDescription;
            ReturnType = returnType;
        }
    }
}