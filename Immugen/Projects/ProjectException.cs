using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public class ProjectException : Exception
    {
    }

    public class ProjectSettingsFileNotFoundException : ProjectException { }
    public class ProjectSettingsInvalidFormatException : ProjectException { }
    public class ProjectInputFolderNotFoundException : ProjectException { }
    public class ProjectAlreadyExistsException : ProjectException { }
}
