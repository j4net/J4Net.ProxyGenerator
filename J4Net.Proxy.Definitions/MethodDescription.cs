using System.Collections.Generic;

namespace DSL
{
    public class MethodDescription : Description
    {
        public List<ParametrDescription> ParametersDescription { get; }
        public string ReturnType { get; }

        public MethodDescription(string name, List<Modifier> modifiers,
            List<ParametrDescription> parametersDescription, string returnType)
        {
            Name = name;
            Modifiers = modifiers;
            ParametersDescription = parametersDescription;
            ReturnType = returnType;
        }
    }
}