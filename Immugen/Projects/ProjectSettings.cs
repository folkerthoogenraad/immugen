using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public class ProjectSettings
    {
        public string InputFolder { get; set; } = "gen";
        public string OutputFolder { get; set; } = "src";
        public OutputSettings OutputSettings { get; set; } = new OutputSettings();
    }
}
