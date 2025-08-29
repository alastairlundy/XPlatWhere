/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XPlatWhereLib.Abstractions.Files;

/// <summary>
/// </summary>
public interface IFileInstancesLocator
{
    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> LocateFileInstancesAsync(string executableName);

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> LocateFileInstancesWithinDriveAsync(DriveInfo driveInfo, string executableName);

    /// <summary>
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> LocateFileInstancesWithinDirectory(string directoryPath, string executableName);
}