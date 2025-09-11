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

using System.Threading.Tasks;

using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Files;

public class FileLocator : IFileLocator
{
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public FileInfo? LocateFile(string fileName)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(drive => drive.IsReady);
        
        ConcurrentBag<FileInfo> output = new();

        FileInfo file = new(fileName);
        
        Parallel.ForEach(drives, (drive, token) =>
        {
            bool result = IsFileWithinDrive(file, drive);

            if (result)
            {
                DirectoryInfo rootDir = drive.RootDirectory;

                foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                {
                    bool foundExecutable = IsFileInDirectory(file, subDir);

                    if (foundExecutable)
                    {
                        foreach (FileInfo executable in subDir.GetFiles("*", SearchOption.AllDirectories))
                        {
                            if (executable.Name.Equals(fileName))
                            {
                                output.Add(executable);
                                return;
                            }
                        }
                    }
                }
            }
        });

        return output.FirstOrDefault(x => !string.IsNullOrEmpty(x.FullName));
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public bool IsFileInDirectory(FileInfo file, DirectoryInfo directory)
    {
        if (Directory.Exists(directory.FullName) == false)
        {
            throw new DirectoryNotFoundException();
        }
            
        IEnumerable<DirectoryInfo> directories = directory.EnumerateDirectories("*",
            SearchOption.AllDirectories);

        foreach (DirectoryInfo dir in directories)
        {
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            
           bool foundFile = files.Any(x => x.Equals(file));
           
           if(foundFile)
               return true;
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="drive"></param>
    /// <returns></returns>
    public bool IsFileWithinDrive(FileInfo file, DriveInfo drive)
    {
        DirectoryInfo rootDir = drive.RootDirectory;
        
        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
        {
            bool foundFile = IsFileInDirectory(file, subDir);

            if (foundFile)
            {
                return true;
            }
        }

        return false;
    }
}