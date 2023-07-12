using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public record Project(ProjectSettings Settings, Definition[] Definitions)
    {
        
    }
}
