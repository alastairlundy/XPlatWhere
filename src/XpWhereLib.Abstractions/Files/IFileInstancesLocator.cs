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
/// Represents a service for locating instances of specific files within a file system.
/// </summary>
public interface IFileInstancesLocator
{
    /// <summary>
    /// Locates all instances of a specific file across all available drives that are ready and not empty.
    /// </summary>
    /// <param name="fileName">The name of the file to search for.</param>
    /// <param name="directorySearchOptions"></param>
    /// <returns>An array of <see cref="FileInfo"/> objects representing the located instances of the file.</returns>
    IEnumerable<FileInfo> LocateFileInstances(string fileName, SearchOption directorySearchOptions);

    /// <summary>
    /// Locates all instances of a specific file within a specified drive.
    /// </summary>
    /// <param name="driveInfo">The drive in which to search for the specified file.</param>
    /// <param name="fileName">The name of the file to search for within the drive.</param>
    /// <param name="directorySearchOption"></param>
    /// <returns>An array of <see cref="FileInfo"/> objects representing the located instances of the file within the specified drive.</returns>
    IEnumerable<FileInfo> LocateFileInstancesWithinDrive(DriveInfo driveInfo, string fileName,
        SearchOption directorySearchOption);

    /// <summary>
    /// Locates all instances of a specific file within the specified directory and its subdirectories.
    /// </summary>
    /// <param name="directory">The directory in which to search for the specified file.</param>
    /// <param name="fileName">The name of the file to search for within the directory.</param>
    /// <param name="directorySearchOption"></param>
    /// <returns>An array of <see cref="FileInfo"/> objects representing the located instances of the file within the specified directory.</returns>
    IEnumerable<FileInfo> LocateFileInstancesWithinDirectory(DirectoryInfo directory, string fileName,
        SearchOption directorySearchOption);
}