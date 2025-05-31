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

using AlastairLundy.DotExtensions.Collections.Generic.ICollections;

using WhatExecLib.Abstractions;
using WhatExecLib.Models;

namespace WhatExecLib
{
    public class PrioritizedExecutableFileLocator : IPrioritizedExecutableFileLocator
    {
        private readonly IExecutableFileDetector _executableFileDetector;
        private readonly IDirectoryListPrioritizer _directoryListPrioritizer;
    
        public ExecutableDirectoryPriority DirectoryPriority { get; private set; }
        
        public ICollection<string> PrioritizedDirectories { get; private set; }

        public PrioritizedExecutableFileLocator(IExecutableFileDetector executableFileDetector, IDirectoryListPrioritizer directoryListPrioritizer)
        {
            _executableFileDetector = executableFileDetector;
            _directoryListPrioritizer = directoryListPrioritizer;
            DirectoryPriority = ExecutableDirectoryPriority.SystemDirectories;
            PrioritizedDirectories = [];
        }
    
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
                        
                        string[] directories = rootDir.GetDirectories("*", SearchOption.AllDirectories).Select(x => x.FullName).ToArray();
                        
                        IEnumerable<string> prioritizedDirectories = _directoryListPrioritizer.
                        
                        foreach (string directory in prioritizedDirectories)
                        {
                            bool foundExecutable = await IsExecutableInDirectoryAsync(executableName, directory, token);

                            if (foundExecutable)
                            {
                                foreach (string file in Directory.GetFiles(directory,"*", SearchOption.AllDirectories))
                                {
                                    if (file.Equals(executableName))
                                    {
                                        output = file;
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

        
        
        public async Task<bool> IsExecutableInDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default)
        {
            DriveInfo driveInfo = new DriveInfo(driveName);
            
            DirectoryInfo rootDir = driveInfo.RootDirectory;

            return await Task.Run(async() =>
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
        public void PrioritizeDirectories(ExecutableDirectoryPriority priority, IEnumerable<string> directories)
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
            PrioritizedDirectories.AddRange(directories);
        }
    }
}