/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AlastairLundy.XpWhereLib.Abstractions.Files;

namespace AlastairLundy.XpWhereLib.Files;

/// <summary>
/// Provides functionality for locating file instances based on a specified file name. This class implements the <see cref="IFileInstancesLocator"/> interface.
/// </summary>
public class FileInstancesLocator : IFileInstancesLocator
{
    /// <summary>
    /// Locates all instances of a file with the specified name across all available drives in the system.
    /// </summary>
    /// <param name="fileName">The name of the file to locate.</param>
    /// <param name="directorySearchOptions"></param>
    /// <returns>An array of <see cref="FileInfo"/> objects representing the located file instances across available drives.</returns>
    public IEnumerable<FileInfo> LocateFileInstances(string fileName, SearchOption directorySearchOptions)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives()
            .Where(x => x.IsReady);

        ParallelQuery<FileInfo> result = drives.AsParallel()
            .SelectMany(drive => LocateFileInstancesWithinDrive(drive, fileName, directorySearchOptions));
    
        return result;
    }

    /// <summary>
    /// Locates all instances of a file with the specified name within the given drive and its subdirectories.
    /// </summary>
    /// <param name="driveInfo">The drive to search within.</param>
    /// <param name="fileName">The name of the file to locate.</param>
    /// <param name="directorySearchOption"></param>
    /// <returns>An array of <see cref="FileInfo"/> objects representing the located file instances within the specified drive.</returns>
    public IEnumerable<FileInfo> LocateFileInstancesWithinDrive(DriveInfo driveInfo, string fileName, SearchOption directorySearchOption)
    {
        ParallelQuery<FileInfo> results = driveInfo.RootDirectory
                .EnumerateDirectories("*", directorySearchOption)
            .AsParallel()
            .SelectMany(dir => LocateFileInstancesWithinDirectory(dir, fileName,  directorySearchOption));

        return results;
    }

    /// <summary>
    /// Locates all instances of a file with the specified name within a given directory and its subdirectories.
    /// </summary>
    /// <param name="directory">The root directory to search within.</param>
    /// <param name="fileName">The name of the file to locate.</param>
    /// <param name="directorySearchOption"></param>
    /// <returns>A sequence of <see cref="FileInfo"/> objects representing the located file instances.</returns>
    public IEnumerable<FileInfo> LocateFileInstancesWithinDirectory(DirectoryInfo directory, string fileName, SearchOption directorySearchOption)
    {
        ParallelQuery<FileInfo> results = directory
            .EnumerateDirectories("*", directorySearchOption)
            .AsParallel()
            .SelectMany(dir =>
            {
               IEnumerable<string> files = Directory.EnumerateFiles(dir.FullName, "*")
                   .Where(x => x.Equals(fileName) || Path.GetFileName(x).Equals(fileName));

                return files.Select(x => new FileInfo(x));
            });
        

        return results;
    }
}