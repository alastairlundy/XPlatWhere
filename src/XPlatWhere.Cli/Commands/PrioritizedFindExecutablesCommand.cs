using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Executables;

namespace XPlatWhere.Cli.Commands;

public class PrioritizedFindExecutablesCommand : ICliCommand
{
    private readonly IPrioritizedExecutableFileLocator _prioritizedExecutableFileLocator;

    public PrioritizedFindExecutablesCommand(IPrioritizedExecutableFileLocator prioritizedExecutableFileLocator)
    {
        _prioritizedExecutableFileLocator = prioritizedExecutableFileLocator;
    }
    
    public Task<int> RunAsync(CommandArguments commandArguments)
    {
        
    }
}