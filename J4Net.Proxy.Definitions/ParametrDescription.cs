namespace DSL
{
    public class ParametrDescription : Description
    {
        public string Type { get; }

        public ParametrDescription(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
