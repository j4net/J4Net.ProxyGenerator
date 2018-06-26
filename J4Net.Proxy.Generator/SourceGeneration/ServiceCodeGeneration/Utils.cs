using System.Text;
using DSL;

namespace ProxyGenerator.SourceGeneration.ServiceCodeGeneration
{
    internal static class Utils
    {
        public static string GetServiceFieldName(MethodDescription description, string destination)
        {
            return $"method_{description.Guid}_{destination}";
        }

        public static string GetServiceSignature(MethodDescription description)
        {
            var signature = new StringBuilder();

            foreach (var paramDescription in description.ParametersDescription)
                signature.Append(ToServiceSignature(paramDescription.Type));

            return $"({signature}){ToServiceSignature(description.ReturnType)}";
        }

        private static string ToServiceSignature(string type)
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
                        return "java/lang/String;";
                    default:
                        return $"L{type.Replace(".", "/")};";
                }
            }

            return $"{(type.Contains("[]") ? "[" : "")}{Sign(type)}";
        }
    }
}
