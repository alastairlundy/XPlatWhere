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

namespace AlastairLundy.XPlatWhereLib.Abstractions.Files;

/// <summary>
/// </summary>
public interface IFileInstancesLocator
{
    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <returns></returns>
    IEnumerable<FileInfo> LocateFileInstances(string executableName);

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    IEnumerable<FileInfo> LocateFileInstancesWithinDrive(DriveInfo driveInfo, string executableName);

    /// <summary>
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    IEnumerable<FileInfo> LocateFileInstancesWithinDirectory(DirectoryInfo directory, string executableName);
}