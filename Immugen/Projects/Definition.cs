using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public record Definition(string Namespace, string Name, string BaseDefinition, Property[] Properties);
}
