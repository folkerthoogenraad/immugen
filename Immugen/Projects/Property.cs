using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public record Property(string Name, string? NameElement, PropertyType Type)
    {
        private string? _nameElement = null;
        private string? _namePascal = null;
        private string? _nameElementPascal = null;
        public string NamePascalCase => _namePascal ??= CreatePascalCase(Name);

        public string GetElementName()
        {
            if (NameElement != null)
            {
                return NameElement;
            }

            return _nameElement ??= CreateSingularName(Name);
        }

        public string GetElementNamePascalCase()
        {
            return _nameElementPascal ??= CreatePascalCase(GetElementName());
        }

        private static string CreateSingularName(string input)
        {
            if (input.EndsWith("s"))
            {
                return input.Substring(0, input.Length - 1);
            }
            return input;
        }
        private static string CreatePascalCase(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
