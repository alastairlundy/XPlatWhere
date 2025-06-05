/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WhatExecLib.Executables.Abstractions;

using WhatExecLib.Executables;

using WhatExecLib.Prioritizers.Abstractions;

namespace WhatExecLib.Executables
{
    /// <summary>
    /// 
    /// </summary>
    public class PrioritizedExecutableFileLocator : IPrioritizedExecutableFileLocator
    {
        private readonly IExecutableFileDetector _executableFileDetector;
        private readonly IDirectoryListPrioritizer _directoryListPrioritizer;
    
        /// <summary>
        /// 
        /// </summary>
        public DirectoryPriority DirectoryPriority { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> PrioritizedDirectories { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableFileDetector"></param>
        /// <param name="directoryListPrioritizer"></param>
        public PrioritizedExecutableFileLocator(IExecutableFileDetector executableFileDetector, IDirectoryListPrioritizer directoryListPrioritizer)
        {
            _executableFileDetector = executableFileDetector;
            _directoryListPrioritizer = directoryListPrioritizer;
            DirectoryPriority = DirectoryPriority.SystemDirectories;
            PrioritizedDirectories = [];
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> LocateExecutableAsync(string executableName, CancellationToken cancellationToken = default)
        {
            DriveInfo[] drives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToArray();
            
#if NET5_0_OR_GREATER
            string output = await LocateExecutableAsync_Net50_OrNewer(executableName, cancellationToken, drives);            
#else
            string output = await LocateExecutableAsync_NetStandard21Fallback(executableName, cancellationToken, drives);
#endif
            return output;
        }

#if NET5_0_OR_GREATER
        private async Task<string> LocateExecutableAsync_Net50_OrNewer(string executableName,
            CancellationToken cancellationToken, DriveInfo[] drives)
        {
            ConcurrentBag<string> strings = new ConcurrentBag<string>();
            
            await Parallel.ForEachAsync(drives, cancellationToken, async (drive, token) =>
            {
                bool result = await IsExecutableWithinDriveAsync(executableName, drive.Name, token);

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
                                    strings.Add(file.FullName);
                                    return;
                                }
                            }
                        }
                    }
                        
                        
                }
            });

            if (strings.Count > 0)
            {
                return strings.First();
            }
            else
            {
                return string.Empty;
            }
        }
#endif

        private async Task<string> LocateExecutableAsync_NetStandard21Fallback(string executableName,
            CancellationToken cancellationToken, DriveInfo[] drives)
        {
            ConcurrentBag<string> strings = new ConcurrentBag<string>();

            Task[] tasks = new Task[drives.Length];

            for (int i = 0; i < drives.Length; i++)
            {
                tasks[i] = new Task(async void () =>
                {
                    try
                    {
                        bool result = await IsExecutableWithinDriveAsync(executableName, drives[i].Name, cancellationToken);

                        if (result)
                        {
                            DirectoryInfo rootDir = drives[i].RootDirectory;

                            IEnumerable<string> directories = rootDir.GetDirectories("*", SearchOption.AllDirectories)
                                .Select(x => x.FullName);

                            IEnumerable<string> prioritizedDirectories =
                                _directoryListPrioritizer.Prioritize(DirectoryPriority, directories);
                        
                            foreach (string directory in prioritizedDirectories)
                            {
                                bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, directory, cancellationToken);

                                if (foundExecutable)
                                {
                                    foreach (string file in Directory.GetFiles(directory,"*", SearchOption.AllDirectories))
                                    {
                                        if (file.Equals(executableName))
                                        {
                                            strings.Add(file);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw; // TODO handle exception
                    }
                });
            }

            for (int i = 0; i < drives.Length; i++)
            {
                tasks[i].Start();
            }

            await Task.WhenAll(tasks);

            if (strings.Count > 0)
            {
                return strings.First();
            }
            else
            {
                return string.Empty;
            }
        }


        public async Task<bool> IsExecutableInDirectoryAsync(string executableName, string directoryPath,
            CancellationToken cancellationToken = default)
        {
            directoryPath = Path.GetFullPath(directoryPath);
        
            if (Directory.Exists(directoryPath) == false)
            {
                throw new DirectoryNotFoundException(directoryPath);
            }
            
            return await Task.Run(() =>
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);
                
              //  if(PrioritizedDirectories.Count < )
                
                IEnumerable<string> prioritizedSubDirectories = _directoryListPrioritizer.Prioritize(DirectoryPriority, subDirectories,
                    PrioritizedDirectories.First());
                
                foreach (string subDirectory in prioritizedSubDirectories)
                {
                    IEnumerable<string> files = Directory.GetFiles(subDirectory)
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

        
        
        public async Task<bool> IsExecutableWithinDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default)
        {
            DriveInfo driveInfo = new DriveInfo(driveName);
            
            DirectoryInfo rootDir = driveInfo.RootDirectory;

            return await Task.Run<bool>(async() =>
            {
                string[] directories = rootDir.GetDirectories("*", SearchOption.AllDirectories).Select(x => x.FullName).ToArray();
                
               IEnumerable<string> prioritizedDirectories = _directoryListPrioritizer.Prioritize(DirectoryPriority, directories, directories.First());
                
                foreach (string directory in prioritizedDirectories)
                {
                    bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, directory, cancellationToken);

                    if (foundExecutable)
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
        /// <param name="priority"></param>
        /// <param name="directories"></param>
        public void PrioritizeDirectories(DirectoryPriority priority, IEnumerable<string> directories)
        {
            DirectoryPriority = priority;
            PrioritizeDirectories(directories);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priorityDirectory"></param>
        public void PrioritizeDirectory(string priorityDirectory)
        {
            PrioritizedDirectories.Clear();
           PrioritizedDirectories.Add(priorityDirectory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directories"></param>
        public void PrioritizeDirectories(IEnumerable<string> directories)
        {
            PrioritizedDirectories.Clear();

            foreach (string directory in directories)
            {
                PrioritizedDirectories.Add(directory);
            }
        }
    }
}