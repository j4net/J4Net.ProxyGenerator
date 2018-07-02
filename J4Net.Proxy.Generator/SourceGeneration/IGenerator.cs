namespace ProxyGenerator.SourceGeneration
{
    internal interface IGenerator<in TIn, out TOut>
    {
        TOut Generate(TIn description);
    }
}
