/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;

namespace WhatExecLib.Abstractions.Prioritizers;

public interface IDirectoryListPrioritizer
{
    IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories);

    IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories,
        string? priorityDirectory);
    
    IList<string> PrioritizeDirectory(string priorityDirectory, IEnumerable<string> directories);
    
    IList<string> PrioritizeUserApplicationDirectories(IEnumerable<string> directories);
    
    IList<string> PrioritizeSystemDirectories(IEnumerable<string> directories);

}