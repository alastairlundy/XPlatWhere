/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using WhatExecLib.Abstractions;

namespace WhatExecLib
{
    /// <summary>
    /// A class to help find an executable when you don't know where it is.
    /// </summary>
    public class ExecutableFileLocator : IExecutableFileLocator
    {
        private readonly IExecutableFileDetector _executableFileDetector;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableFileDetector"></param>
        public ExecutableFileLocator(IExecutableFileDetector executableFileDetector)
        {
            _executableFileDetector = executableFileDetector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> LocateExecutableAsync(string executableName, CancellationToken cancellationToken = default)
        {
            string output = string.Empty;
            
            DriveInfo[] drives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToArray();
            
            // Parallel.ForEachAsync isn't supported by .NET Standard 2.1 or earlier, so this is only run on .NET 5+
#if NET5_0_OR_GREATER
            await Parallel.ForEachAsync(drives, cancellationToken, async (drive, token) =>
            {
                    bool result = await IsExecutableInDriveAsync(executableName, drive.Name, token);

                    if (result)
                    {
                        DirectoryInfo rootDir = drive.RootDirectory;
                        
                        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                        {
                            bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, subDir.FullName, token);

                            if (foundExecutable)
                            {
                                foreach (FileInfo file in subDir.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    if (file.Name.Equals(executableName))
                                    {
                                        output = file.FullName;
                                        return;
                                    }
                                }
                            }
                        }
                        
                        
                    }
            });
#else
            Task[] tasks = new Task[drives.Length];

            for (int i = 0; i < drives.Length; i++)
            {
                tasks[i] = new Task(async void () =>
                {
                    bool result = await IsExecutableInDriveAsync(executableName, drives[i].Name, cancellationToken);

                    if (result)
                    {
                        DirectoryInfo rootDir = drives[i].RootDirectory;
                        
                        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                        {
                            bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, subDir.FullName, cancellationToken);

                            if (foundExecutable)
                            {
                                foreach (FileInfo file in subDir.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    if (file.Name.Equals(executableName))
                                    {
                                        output = file.FullName;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                });
            }

            for (int i = 0; i < drives.Length; i++)
            {
                tasks[i].Start();
            }

            await Task.WhenAll(tasks);
#endif

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="directoryPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public async Task<bool> IsExecutableInDirectoryAsync(string executableName, string directoryPath, CancellationToken cancellationToken = default)
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
                    IEnumerable<string> files = Directory.GetFiles(directory)
                        .Where(file => _executableFileDetector.IsFileExecutable(file) &&
                                       _executableFileDetector.DoesFileHaveExecutablePermissions(file));

                    string? file = files.FirstOrDefault(x => x.Equals(executableName));

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
        /// <returns></returns>
        public async Task<bool> IsExecutableInDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default)
        {
            DriveInfo driveInfo = new DriveInfo(driveName);
            
            DirectoryInfo rootDir = driveInfo.RootDirectory;

            return await Task.Run(async() =>
            {
                foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                {
                    bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, subDir.FullName);

                    if (foundExecutable)
                    {
                        return true;
                    }
                }
                return false;
            }, cancellationToken);
        }
    }
}