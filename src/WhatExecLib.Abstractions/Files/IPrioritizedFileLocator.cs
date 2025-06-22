/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using WhatExecLib.Abstractions.Prioritizers;

namespace WhatExecLib.Abstractions.Files;

public interface IPrioritizedFileLocator : IFileLocator
{
    DirectoryPriority DirectoryPriority { get; }
        
    void PrioritizeDirectory(string directory);
    void PrioritizeDirectories(IEnumerable<string> directories);
    void PrioritizeDirectories(DirectoryPriority priority, IEnumerable<string> directories);

}