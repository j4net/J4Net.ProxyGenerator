using System.Collections.Generic;

namespace ProxyGenerator.SourceGeneration
{
    internal class ProxySource
    {
        public IEnumerable<NamespaceUnit> NamespaceUnits { get; }

        public ProxySource(IEnumerable<NamespaceUnit> namespaceUnits)
        {
            NamespaceUnits = namespaceUnits;
        }
    }
}
