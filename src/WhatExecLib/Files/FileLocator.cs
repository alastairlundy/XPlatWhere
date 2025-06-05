/*
    WhatExecLib
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

using WhatExecLib.Files.Abstractions;

namespace WhatExecLib.Files
{
    public class FileLocator : IFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> LocateFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            DriveInfo[] drives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToArray();
            
            // Parallel.ForEachAsync isn't supported by .NET Standard 2.1 or earlier, so this is only run on .NET 5+
#if NET5_0_OR_GREATER
            string output = await LocateFileAsync_Net50_OrNewer(fileName, cancellationToken, drives);
#else
            string output = await LocateFileAsync_NetStandard2X_Fallback(fileName, cancellationToken, drives);
#endif

            return output;
        }
        
#if NET5_0_OR_GREATER
        private async Task<string> LocateFileAsync_Net50_OrNewer(string fileName,
            CancellationToken cancellationToken, DriveInfo[] drives)
        {
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

            if (output.Count > 0)
            {
                return output.First(x => string.IsNullOrEmpty(x) == false);
            }
            else
            {
                return string.Empty;
            }
        }
#endif

        private async Task<string> LocateFileAsync_NetStandard2X_Fallback(string fileName,
            CancellationToken cancellationToken, DriveInfo[] drives)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            
            Task[] tasks = new Task[drives.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(async void () =>
                {
                    bool result = await IsFileWithinDriveAsync(fileName, drives[i].Name, cancellationToken);

                    if (result)
                    {
                        DirectoryInfo rootDir = drives[i].RootDirectory;
                        
                        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                        {
                            bool foundExecutable = await IsFileInDirectoryAsync(fileName,
                                subDir.FullName, cancellationToken);

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
            }

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Start();
            }

            await Task.WhenAll(tasks);

            if (output.Count > 0)
            {
                return output.First(x => string.IsNullOrEmpty(x) == false);
            }
            else
            {
                return string.Empty;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directoryPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public async Task<bool> IsFileInDirectoryAsync(string fileName, string directoryPath, CancellationToken cancellationToken = default)
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
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="driveName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsFileWithinDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default)
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
}