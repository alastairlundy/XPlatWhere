using System.Collections.Generic;
using WhatExecLib.Executables;

namespace WhatExecLib.Prioritizers.Abstractions
{
    public interface IDirectoryListPrioritizer
    {
        IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories);

        IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories,
            string? priorityDirectory);
    
        IList<string> PrioritizeDirectory(string priorityDirectory, IEnumerable<string> directories);
    
        IList<string> PrioritizeUserApplicationDirectories(IEnumerable<string> directories);
    
        IList<string> PrioritizeSystemDirectories(IEnumerable<string> directories);

    }
}