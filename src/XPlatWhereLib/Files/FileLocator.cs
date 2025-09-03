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
    public Task<string?> LocateFile(string fileName)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(drive => drive.IsReady);

        ConcurrentBag<string> output = new();

        Parallel.ForEach(drives, (drive, token) =>
        {
            bool result = IsFileWithinDrive(fileName, drive.Name);

            if (result)
            {
                DirectoryInfo rootDir = drive.RootDirectory;

                foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                {
                    bool foundExecutable = IsFileInDirectory(fileName, subDir.FullName);

                    if (foundExecutable)
                    {
                        foreach (FileInfo file in subDir.GetFiles("*", SearchOption.AllDirectories))
                        {
                            if (file.Name.Equals(fileName))
                            {
                                output.Add(file.FullName);
                                return;
                            }
                        }
                    }
                }
            }
        });

        return Task.FromResult(output.FirstOrDefault(x => !string.IsNullOrEmpty(x)));
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public bool IsFileInDirectory(string fileName, string directoryPath)
    {
        directoryPath = Path.GetFullPath(directoryPath);
        
        if (Directory.Exists(directoryPath) == false)
        {
            throw new DirectoryNotFoundException(directoryPath);
        }
            
        string[] directories = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);

       IEnumerable<string?> results = (from dir in directories
            select Directory.GetFiles(dir).FirstOrDefault(x => x.Equals(fileName)));

       return results.Any(x => x is not null);
    }

    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <param name="driveName"></param>
    /// <returns></returns>
    public bool IsFileWithinDrive(string executableName, string driveName)
    {
        DriveInfo driveInfo = new DriveInfo(driveName);

        DirectoryInfo rootDir = driveInfo.RootDirectory;
        
        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
        {
            bool foundFile = IsFileInDirectory(executableName, subDir.FullName);

            if (foundFile)
            {
                return true;
            }
        }

        return false;
    }
}