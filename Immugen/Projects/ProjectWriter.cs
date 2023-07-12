using Immugen.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public class ProjectWriter
    {
        public static void Write(Project project, IDefinitionConverter converter) // Project write settings or somethign? :)
        {
            foreach(var definition in  project.Definitions)
            {
                WriteDefinition(project, definition, converter);
            }
        }

        private static string GetDirectory(Project project, Definition definition)
        {
            string root = project.Settings.OutputFolder;
            string[] namespacePaths = definition.Namespace.Split('.');

            return Path.Combine(namespacePaths.Prepend(root).ToArray());
        }

        private static void WriteDefinition(Project project, Definition definition, IDefinitionConverter converter)
        {
            string directory = GetDirectory(project, definition);
            string file = Path.ChangeExtension(Path.Combine(directory, definition.Name), converter.FileExtension);

            string content = converter.Convert(definition);

            Directory.CreateDirectory(directory);
            File.WriteAllText(file, content);
        }
    }
}
