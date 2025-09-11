/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using AlastairLundy.DotExtensions.IO.Drives;
using AlastairLundy.EnhancedLinq.Immediate;
using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Files;

/// <summary>
/// </summary>
public class FileInstancesLocator : IFileInstancesLocator
{
    public FileInstancesLocator()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public IEnumerable<FileInfo> LocateFileInstances(string fileName)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(x => x.IsReady && x.IsDriveEmpty() == false);

        IEnumerable<FileInfo> results = (from drive in drives
                from filesInDrive in LocateFileInstancesWithinDrive(drive, fileName)
                select filesInDrive);

        return results;
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public IEnumerable<FileInfo> LocateFileInstancesWithinDrive(DriveInfo driveInfo, string fileName)
    {
        DirectoryInfo rootDir = driveInfo.RootDirectory;
        
        IEnumerable<FileInfo> results = (from directory in rootDir.EnumerateDirectories("*", SearchOption.AllDirectories)
            from filesInDirectory in LocateFileInstancesWithinDirectory(directory, fileName)
            select filesInDirectory);
        
        return results;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public IEnumerable<FileInfo> LocateFileInstancesWithinDirectory(DirectoryInfo directory, string fileName)
    {
        IEnumerable<FileInfo> results = (from file in directory.EnumerateFiles("*", SearchOption.AllDirectories)
            where file.FullName.Equals(fileName)
            select file);
        
        return results;
    }
}