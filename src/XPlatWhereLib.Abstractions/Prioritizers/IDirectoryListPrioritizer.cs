/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;

namespace AlastairLundy.XPlatWhereLib.Abstractions.Prioritizers;

/// <summary>
/// 
/// </summary>
public interface IDirectoryListPrioritizer
{
    /// <summary>
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="directories"></param>
    /// <returns></returns>
    IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories);

    /// <summary>
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="directories"></param>
    /// <param name="priorityDirectory"></param>
    /// <returns></returns>
    IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories,
        string? priorityDirectory);

    /// <summary>
    /// </summary>
    /// <param name="priorityDirectory"></param>
    /// <param name="directories"></param>
    /// <returns></returns>
    IList<string> PrioritizeDirectory(string priorityDirectory, IEnumerable<string> directories);

    /// <summary>
    /// </summary>
    /// <param name="directories"></param>
    /// <returns></returns>
    IList<string> PrioritizeUserApplicationDirectories(IEnumerable<string> directories);

    /// <summary>
    ///     
    /// </summary>
    /// <param name="directories"></param>
    /// <returns></returns>
    IList<string> PrioritizeSystemDirectories(IEnumerable<string> directories);
}