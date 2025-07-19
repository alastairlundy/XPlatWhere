using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhere.Cli.Commands;

public class PrioritizedFindFilesCommand : ICliCommand
{
    private readonly IPrioritizedFileLocator _prioritizedFileLocator;

    public PrioritizedFindFilesCommand(IPrioritizedFileLocator prioritizedFileLocator)
    {
        _prioritizedFileLocator = prioritizedFileLocator;
    }

    public Task<int> RunAsync(CommandArguments commandArguments)
    {
        
    }
}