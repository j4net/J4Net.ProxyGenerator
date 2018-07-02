namespace DSL
{
    public class ParameterDescription : Description
    {
        public readonly string Type;
        public ParameterDescription(string name, string type) : base(name, null) => Type = type;
    }
}
