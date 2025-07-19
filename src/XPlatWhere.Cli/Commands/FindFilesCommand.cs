using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhere.Cli.Commands;

public class FindFilesCommand : ICliCommand
{
    private readonly IFileInstancesLocator _fileInstancesLocator;
    private readonly IFileLocator _fileLocator;

    public FindFilesCommand(IFileInstancesLocator fileInstancesLocator, IFileLocator fileLocator)
    {
        _fileInstancesLocator = fileInstancesLocator;
        _fileLocator = fileLocator;
    }
    
    public async Task<int> RunAsync(CommandArguments commandArguments)
    {
        
    }
}