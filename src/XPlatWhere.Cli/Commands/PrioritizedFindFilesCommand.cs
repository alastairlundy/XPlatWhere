using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Executables;
using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhere.Cli.Commands;

public class PrioritizedFindFilesCommand : ICliCommand
{
    private readonly IExecutableFileInstancesLocator _prioritizedFileLocator;

    public PrioritizedFindFilesCommand(IExecutableFileInstancesLocator prioritizedFileLocator)
    {
        _prioritizedFileLocator = prioritizedFileLocator;
    }

    public Task<int> RunAsync(CommandArguments commandArguments)
    {
    }
}