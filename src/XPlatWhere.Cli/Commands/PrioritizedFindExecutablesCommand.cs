using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Executables;

namespace XPlatWhere.Cli.Commands;

public class PrioritizedFindExecutablesCommand : ICliCommand
{
    private readonly IExecutableFileLocator _prioritizedExecutableFileLocator;

    public PrioritizedFindExecutablesCommand(IExecutableFileLocator prioritizedExecutableFileLocator)
    {
        _prioritizedExecutableFileLocator = prioritizedExecutableFileLocator;
    }

    public Task<int> RunAsync(CommandArguments commandArguments)
    {
    }
}