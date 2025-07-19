/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using XPlatWhereLib.Abstractions.Prioritizers;

namespace XPlatWhereLib.Abstractions.Files;

/// <summary>
/// 
/// </summary>
public interface IPrioritizedFileLocator : IFileLocator
{
    /// <summary>
    /// 
    /// </summary>
    DirectoryPriority DirectoryPriority { get; }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    void PrioritizeDirectory(string directory);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="directories"></param>
    void PrioritizeDirectories(IEnumerable<string> directories);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="directories"></param>
    void PrioritizeDirectories(DirectoryPriority priority, IEnumerable<string> directories);

}