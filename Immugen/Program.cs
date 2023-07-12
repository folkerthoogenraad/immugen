// See https://aka.ms/new-console-template for more information
using CommandLine;
using CommandLine.Text;
using Immugen.CLI;
using Immugen.Converters;
using Immugen.Projects;

namespace Immugen;

public class Program
{
    public static void Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<InitOptions, BuildOptions>(args);
        result.WithParsed<InitOptions>(options => RunInit(options));
        result.WithParsed<BuildOptions>(options => RunBuild(options));

    }

    private static void RunInit(InitOptions options)
    {
        try
        {
            Project.CreateProject(new ProjectSettings(), Environment.CurrentDirectory);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void RunBuild(BuildOptions options)
    {
        try
        {
            var project = Project.LoadFromFolder(Environment.CurrentDirectory);

            if(project.Settings.OutputSettings.Language != "ts")
            {
                throw new Exception($"Cannot output to language {project.Settings.OutputSettings.Language}");
            }

            Project.Write(project, new TSDefinitionConverter());
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
