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
using System.Runtime.Versioning;
using System.Threading.Tasks;

using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Files;

public class MultiFileLocator : IMultiFileLocator
{
    /// <summary>
    /// </summary>
    public MultiFileLocator()
    {
        
    }

    /// <summary>
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public IEnumerable<FileInfo> LocateAllFilesWithinDirectoryAsync(DirectoryInfo directory)
    {
        if (Directory.Exists(Path.GetFullPath(directory.FullName)) == false)
        {
            throw new DirectoryNotFoundException();
        }
        
        DirectoryInfo[] directories = directory.GetDirectories("*", SearchOption.AllDirectories);
        
        foreach (DirectoryInfo subDirectory in directories)
        {
            IEnumerable<FileInfo> files = subDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
               yield return file;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public IEnumerable<FileInfo> LocateAllFilesWithinDriveAsync(DriveInfo driveInfo)
    {
        ConcurrentBag<FileInfo> output = new();

        DirectoryInfo rootDir = driveInfo.RootDirectory;
        
            Parallel.ForEach(rootDir.GetDirectories("*", SearchOption.AllDirectories),  void (subDir) =>
            {
                IEnumerable<FileInfo> executables = LocateAllFilesWithinDirectoryAsync(subDir);

                foreach (FileInfo executable in executables)
                {
                    output.Add(executable);
                }
            });

            return output;
    }
}