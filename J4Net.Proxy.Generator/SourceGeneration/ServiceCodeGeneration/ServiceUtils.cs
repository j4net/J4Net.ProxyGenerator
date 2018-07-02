using System.Text;
using DSL;
using ProxyGenerator.Infrastructure;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal static class ServiceUtils
    {
        public static string GetMethodRefName(MethodDescription description) 
            => $"method{description.Name}Ref_{description.Guid}";

        public static string GetMethodRefProperty(MethodDescription description) =>
            GetMethodRefName(description).UppercaseFirstLetter();

        public static string GetMethodJavaSignature(MethodDescription description)
        {
            var signature = new StringBuilder();

            foreach (var paramDescription in description.ParametersDescription)
                signature.Append(ToJavaSignature(paramDescription.Type));

            return $"({signature}){ToJavaSignature(description.ReturnType)}";
        }

        private static string ToJavaSignature(string type)
        {
            string Sign(string _type)
            {
                switch (_type)
                {
                    case "bool":
                        return "Z";
                    case "byte":
                        return "B";
                    case "char":
                        return "C";
                    case "short":
                        return "S";
                    case "int":
                        return "I";
                    case "long":
                        return "J";
                    case "float":
                        return "F";
                    case "double":
                        return "D";
                    case "void":
                        return "V";
                    case "string":
                        return "Ljava/lang/String;";
                    default:
                        return $"L{type.Replace(".", "/")};";
                }
            }

            return $"{(type.Contains("[]") ? "[" : "")}{Sign(type)}";
        }
    }
}
