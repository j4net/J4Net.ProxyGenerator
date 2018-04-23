namespace DSL
{
    public class ParametrDescription : Description
    {
        public readonly string Type;
        public ParametrDescription(string name, string type) : base(name, null) => Type = type;
    }
}
