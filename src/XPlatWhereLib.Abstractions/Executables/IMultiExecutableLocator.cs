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

namespace AlastairLundy.XPlatWhereLib.Abstractions.Executables;

/// <summary>
/// </summary>
public interface IMultiExecutableLocator
{
    /// <summary>
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    Task<IEnumerable<string>> LocateAllExecutablesWithinDirectoryAsync(string folder);

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> LocateAllExecutablesWithinDriveAsync(DriveInfo driveInfo);
}