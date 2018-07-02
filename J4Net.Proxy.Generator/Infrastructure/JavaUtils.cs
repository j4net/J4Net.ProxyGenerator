using System.Linq;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using YieldStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.YieldStatementSyntax;

namespace ProxyGenerator.Infrastructure
{
    internal static class JavaUtils
    {
        private static readonly string[] primitiveTypes = 
        {
            "bool",
            "byte",
            "char",
            "short",
            "int",
            "long",
            "float",
            "double"
        };

        public static bool IsPrimitive(string type) => primitiveTypes.Contains(type);

        public static string GetTypeName(string type)
        {
            if (!primitiveTypes.Contains(type)) return "pointerValue";

            if (type == "bool")
                type = "boolean";
            else if (type == "int")
                type = "integer";

            return $"{type}Value";

        }
    }
}
