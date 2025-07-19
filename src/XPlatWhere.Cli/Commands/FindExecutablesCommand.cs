using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.DotPrimitives.Collections.CachedEnumerables;

using XPlatWhereLib.Abstractions.Executables;

namespace XPlatWhere.Cli.Commands;

public class FindExecutablesCommand : ICliCommand
{
    private readonly IExecutableFileInstancesLocator _executableFileInstancesLocator;
    private readonly IExecutableFileLocator _executableFileLocator;

    public FindExecutablesCommand(IExecutableFileInstancesLocator executableFileInstancesLocator,
        IExecutableFileLocator executableFileLocator)
    {
        _executableFileInstancesLocator = executableFileInstancesLocator;
        _executableFileLocator = executableFileLocator;
    }
    
    public async Task<int> RunAsync(CommandArguments commandArguments)
    {
        
        return 0;
    }
}