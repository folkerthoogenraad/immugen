using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public record Property(string Name, PropertyType Type)
    {
        private string? _nameSingular = null;
        private string? _namePascal = null;
        private string? _namePascalSingular = null;
        public string NameSingular => _nameSingular ??= CreateSingularName(Name);
        public string NamePascalCase => _namePascal ??= CreatePascalCase(Name);
        public string NameSingularPascalCase => _namePascalSingular ??= CreatePascalCase(NameSingular);

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
