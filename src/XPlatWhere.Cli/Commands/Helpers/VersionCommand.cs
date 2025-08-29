using System;
using System.Reflection;
using System.Threading.Tasks;
using AlastairLundy.DotExtensions.Versions;
using XPlatWhere.Cli.Localizations;

namespace XPlatWhere.Cli.Commands.Helpers;
 
public class VersionCommand : ICliCommand
{
    public Task<int> RunAsync(CommandArguments commandArguments)
    {
        Version? version = Assembly.GetExecutingAssembly().GetName().Version;

        if (version is not null)
        {
            string output = $"{Resources.App_Name} v{version.ToHumanReadableString()} {Resources.Labels_Versions_RunningOn} ";
        
            Console.WriteLine(output);

            return Task.FromResult(0);
        }

        return Task.FromResult(-1);
    }
}