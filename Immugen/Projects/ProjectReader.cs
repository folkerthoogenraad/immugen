using Immugen.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public class ProjectReader
    {
        public static string ImmugenSettingsFile = "immugen.json";

        public static ProjectSettings LoadProjectSettingsFromFolder(string folder)
        {
            if(File.Exists(Path.Combine(folder, ImmugenSettingsFile)))
            {
                var text = File.ReadAllText(Path.Combine(folder, ImmugenSettingsFile));

                var settings = JsonSerializer.Deserialize<ProjectSettings>(text, new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,

                });

                if (settings == null) throw new ApplicationException("Invalid immungen project settings file"); 

                return settings;
            }

            return new ProjectSettings();
        }

        public static Project LoadFromFolder(string path)
        {
            var settings = LoadProjectSettingsFromFolder(path);

            var definitions = new List<Definition>();

            foreach (string file in Directory.EnumerateFiles(Path.Combine(path, settings.InputFolder), "*.mgn", SearchOption.AllDirectories))
            {
                GatherDefinitions(definitions, file);
            }

            return new Project(settings, definitions.ToArray());
        }

        private static void GatherDefinitions(List<Definition> definitions, string file)
        {
            var text = File.ReadAllText(file);
            var commands = MGNParser.Parse(text);

            string clsName = string.Empty;
            string namespaceName = string.Empty;
            string baseclsName = string.Empty;

            List<Property> properties = new List<Property>();

            Action flush = () =>
            {
                if (clsName.Length > 0)
                {
                    definitions.Add(new Definition(namespaceName, clsName, baseclsName, properties.ToArray()));
                }

                clsName = string.Empty;

                properties.Clear();
            };

            foreach(var command in commands)
            {
                if (command is MGNNamespaceCommand nmsCommand)
                {
                    flush();

                    namespaceName = nmsCommand.Namespace;
                }
                if (command is MGNClassCommand clsCommand)
                {
                    flush();

                    clsName = clsCommand.Name;
                }
                // This one is a bit ugly tbh but doesn't matter.
                if (command is MGNExtendsCommand extendsCommand)
                {
                    baseclsName= extendsCommand.BaseClass;
                }

                if(command is MGNPropertyCommand propertyCommand)
                {
                    properties.Add(new Property(propertyCommand.Name, new PropertyType(propertyCommand.Type.Type, propertyCommand.Type.ArrayRank)));
                }
            }

            flush();
        }
    }
}
