/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.IO;

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
    FileInfo? LocateFile(string fileName);

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    bool IsFileInDirectory(FileInfo file, DirectoryInfo directory);

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="drive"></param>
    /// <returns></returns>
    bool IsFileWithinDrive(FileInfo file, DriveInfo drive);
}