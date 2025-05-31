using System.Collections.Generic;
using WhatExecLib.Models;

namespace WhatExecLib.Abstractions
{
    public interface IDirectoryListPrioritizer
    {
        IList<string> Prioritize(ExecutableDirectoryPriority priority, IEnumerable<string> directories);

        IList<string> Prioritize(ExecutableDirectoryPriority priority, IEnumerable<string> directories,
            string? priorityDirectory);
    
        IList<string> PrioritizeDirectory(string priorityDirectory, IEnumerable<string> directories);
    
        IList<string> PrioritizeUserApplicationExecutables(IEnumerable<string> directories);
    
        IList<string> PrioritizeSystemExecutables(IEnumerable<string> directories);

    }
}