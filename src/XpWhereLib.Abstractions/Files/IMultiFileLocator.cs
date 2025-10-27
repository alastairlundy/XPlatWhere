/*
    XpWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;

namespace AlastairLundy.XpWhereLib.Abstractions.Files;

/// <summary>
/// </summary>
public interface IMultiFileLocator
{
    /// <summary>
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    IEnumerable<FileInfo> LocateAllFilesWithinDirectoryAsync(DirectoryInfo directory);

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
    IEnumerable<FileInfo> LocateAllFilesWithinDriveAsync(DriveInfo driveInfo);
}