using System.Collections.Generic;

namespace DSL
{
    public class MethodDescription : Description
    {
        public readonly List<ParameterDescription> ParametersDescription;
        public readonly string ReturnType;

        public MethodDescription(string name, List<ModifierDescription> modifiersDescriptions,
            List<ParameterDescription> parametersDescription, string returnType)
                : base(name, modifiersDescriptions)
        {
            ParametersDescription = parametersDescription;
            ReturnType = returnType;
        }
    }
}