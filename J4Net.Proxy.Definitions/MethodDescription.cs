using System.Collections.Generic;

namespace DSL
{
    public class MethodDescription : Description
    {
        public readonly List<ParameterDescription> ParametersDescription;
        public readonly string ReturnType;
        public readonly bool IsConstructor;
        public readonly string Guid;

        public MethodDescription(string name, List<ModifierDescription> modifiersDescriptions,
            List<ParameterDescription> parametersDescription, string returnType, 
            bool isConstructor)
                : base(name, modifiersDescriptions)
        {
            ParametersDescription = parametersDescription;
            ReturnType = returnType;
            IsConstructor = isConstructor;
            Guid = System.Guid.NewGuid().ToString("N")
                              .Substring(0, 5);
        }
    }
}