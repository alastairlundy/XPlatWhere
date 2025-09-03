/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.XPlatWhereLib.Abstractions.Files;

/// <summary>
/// 
/// </summary>
public interface IFileLocator
{
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<string?> LocateFile(string fileName);

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    bool IsFileInDirectory(string fileName, string directoryPath);

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="driveName"></param>
    /// <returns></returns>
    bool IsFileWithinDrive(string fileName, string driveName);
}