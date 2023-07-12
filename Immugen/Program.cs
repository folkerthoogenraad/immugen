// See https://aka.ms/new-console-template for more information
using Immugen.Converters;
using Immugen.Projects;

var project = ProjectReader.LoadFromFolder(Environment.CurrentDirectory);

ProjectWriter.Write(project, new TSDefinitionConverter());
