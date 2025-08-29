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
using System.Threading;
using System.Threading.Tasks;
using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhereLib.Files;

public class FileLocator : IFileLocator
{
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> LocateFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(drive => drive.IsReady);

        ConcurrentBag<string> output = new();

        await Parallel.ForEachAsync(drives, cancellationToken, async (drive, token) =>
        {
            bool result = await IsFileWithinDriveAsync(fileName, drive.Name, token);

            if (result)
            {
                DirectoryInfo rootDir = drive.RootDirectory;

                foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                {
                    bool foundExecutable = await IsFileInDirectoryAsync(fileName, subDir.FullName, token);

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

        if (output.Count > 0) return output.First(x => !string.IsNullOrEmpty(x));

        return string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directoryPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public async Task<bool> IsFileInDirectoryAsync(string fileName, string directoryPath,
        CancellationToken cancellationToken = default)
    {
        directoryPath = Path.GetFullPath(directoryPath);
        
        if (Directory.Exists(directoryPath) == false)
        {
            throw new DirectoryNotFoundException(directoryPath);
        }
            
        string[] directories = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);

        return await Task.Run(() =>
        {
            foreach (string directory in directories)
            {
                IEnumerable<string> files = Directory.GetFiles(directory);

                string? file = files.FirstOrDefault(x => x.Equals(fileName));

                if (file is not null)
                {
                    return true;
                }
            }

            return false;
        }, cancellationToken);
    }

    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <param name="driveName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> IsFileWithinDriveAsync(string executableName, string driveName,
        CancellationToken cancellationToken = default)
    {
        DriveInfo driveInfo = new DriveInfo(driveName);
            
        DirectoryInfo rootDir = driveInfo.RootDirectory;

        return await Task.Run(async() =>
        {
            foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                bool foundFile = await IsFileInDirectoryAsync(executableName, subDir.FullName, cancellationToken);

                if (foundFile)
                {
                    return true;
                }
            }
            return false;
        }, cancellationToken);
    }
}