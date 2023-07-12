using Immugen.Converters;
using Immugen.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public partial record class Project
    {
        private static string ImmugenSettingsFile = "immugen.json";
        private static string ImmugenFilePattern = "*.mgn";

        private static JsonSerializerOptions JsonSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static bool IsProjectFolder(string folder)
        {
            return File.Exists(GetProjectSettingsFile(folder));
        }

        public static string GetProjectSettingsFile(string folder)
        {
            return Path.Combine(folder, ImmugenSettingsFile);
        }

        public static string GetProjectInputFolder(string folder, ProjectSettings settings)
        {
            return Path.Combine(folder, settings.InputFolder);
        }
        public static string GetProjectOutputFolder(string folder, ProjectSettings settings)
        {
            return Path.Combine(folder, settings.OutputFolder);
        }


        // ================================================================== //
        // Project reading
        // ================================================================== //
        public static ProjectSettings GetProjectSettings(string folder)
        {
            if(!IsProjectFolder(folder))
            {
                throw new ProjectSettingsFileNotFoundException();
            }

            var text = File.ReadAllText(GetProjectSettingsFile(folder));

            var settings = JsonSerializer.Deserialize<ProjectSettings>(text, JsonSettings);

            if (settings == null) throw new ProjectSettingsInvalidFormatException();

            return settings;
        }

        public static Project LoadFromFolder(string folder)
        {
            var settings = GetProjectSettings(folder);

            var definitions = new List<Definition>();

            if(!Directory.Exists(GetProjectInputFolder(folder, settings)))
            {
                throw new ProjectInputFolderNotFoundException();
            }

            foreach (string file in Directory.EnumerateFiles(GetProjectInputFolder(folder, settings), ImmugenFilePattern, SearchOption.AllDirectories))
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
                    properties.Add(new Property(propertyCommand.Name, propertyCommand.ElementName, new PropertyType(propertyCommand.Type.Type, propertyCommand.Type.ArrayRank)));
                }
            }

            flush();
        }


        // ================================================================== //
        // Project writing
        // ================================================================== //
        public static void CreateProject(ProjectSettings settings, string folder)
        {
            if (IsProjectFolder(folder))
            {
                throw new ProjectAlreadyExistsException();
            }

            // Write the settings json
            var json = JsonSerializer.Serialize(settings, JsonSettings);

            File.WriteAllText(GetProjectSettingsFile(folder), json);

            // Create the input directory
            if (!Directory.Exists(GetProjectInputFolder(folder, settings)))
            {
                Directory.CreateDirectory(GetProjectInputFolder(folder, settings));
            }

            // Create default file
            if(!File.Exists(Path.Combine(GetProjectInputFolder(folder, settings))))
            {
                File.WriteAllText(Path.Combine(GetProjectInputFolder(folder, settings), "main.mgn"), @"namespace test
class Person

name: string
age: number
children: Person[] element child
vehicles: Vehicle[]

class Vehicle
name: string");

            }
        }

        public static void Write(Project project, IDefinitionConverter converter) // Project write settings or somethign? :)
        {
            foreach (var definition in project.Definitions)
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

            // TODO add support for markers, where you can add your own code in between without immugen destroying it. Ex:
            // IMMUGEN Fields
            // IMMUGEN End
        }
    }
}
