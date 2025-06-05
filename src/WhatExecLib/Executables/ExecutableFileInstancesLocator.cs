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
using System.Runtime.Versioning;
using System.Threading.Tasks;
using WhatExecLib.Executables.Abstractions;

namespace WhatExecLib.Executables
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutableFileInstancesLocator : IExecutableFileInstancesLocator
    {
        private readonly IExecutableFileDetector _executableFileDetector;
        private readonly IExecutableFileLocator _executableFileLocator;
        
        public ExecutableFileInstancesLocator(IExecutableFileDetector executableDetector,
            IExecutableFileLocator executableFileLocator)
        {
            _executableFileDetector = executableDetector;
            _executableFileLocator = executableFileLocator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public async Task<IEnumerable<string>> LocateExecutableInstancesAsync(string executableName)
        {
            DriveInfo[] drives = DriveInfo.GetDrives().Where(x => x.IsReady).ToArray();

            #if NET5_0_OR_GREATER
                IEnumerable<string> output = await LocateExecutableInstancesAsync_Net50_OrNewer(executableName, drives);
            #else
                IEnumerable<string> output = await LocateExecutableInstancesAsync_NetStandard2XFallback(executableName, drives);
            #endif

            return output;
        }

        private async Task<IEnumerable<string>> LocateExecutableInstancesAsync_NetStandard2XFallback(
            string executableName, DriveInfo[] drives)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            
            Task<IEnumerable<string>>[] tasks = new Task<IEnumerable<string>>[drives.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = LocateExecutableInstancesWithinDriveAsync(drives[i], executableName);
            }

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Start();
            }
            
            await Task.WhenAll(tasks);

            foreach (Task<IEnumerable<string>> task in tasks)
            {
                foreach (string s in task.Result)
                {
                    output.Add(s);
                }
            }

            return output;
        }

#if NET5_0_OR_GREATER
        private async Task<IEnumerable<string>> LocateExecutableInstancesAsync_Net50_OrNewer(string executableName, DriveInfo[] drives)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            
            await Parallel.ForEachAsync(drives, async (drive, token) =>
            {
                IEnumerable<string> executablesWithinDrive = await LocateExecutableInstancesWithinDriveAsync(drive, executableName);

                IList<string> executablesDriveList = executablesWithinDrive.ToList();
                
                if (executablesDriveList.Count > 0)
                {
                    foreach (string executable in executablesDriveList)
                    {
                        output.Add(executable);
                    }
                }
            });

            if (output.Count == 0)
            {
                return [];
            }
            else
            {
                return output;
            }
        }
#endif

        public async Task<IEnumerable<string>> LocateExecutableInstancesWithinDriveAsync(DriveInfo driveInfo, string executableName)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
            
            DirectoryInfo rootDir = driveInfo.RootDirectory;

                Parallel.ForEach(rootDir.GetDirectories("*", SearchOption.AllDirectories), async void (subDir) =>
                {
                    IEnumerable<string> executables = await LocateExecutableInstancesWithinDirectory(subDir.FullName, executableName);

                    foreach (string executable in executables)
                    {
                        output.Add(executable);
                    }
                });
            
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="executableName"></param>
        /// <returns></returns>
        public Task<IEnumerable<string>> LocateExecutableInstancesWithinDirectory(string directoryPath, string executableName)
        {
            List<string> output = new List<string>();
            
            DirectoryInfo rootDir = new DirectoryInfo(Path.GetFullPath(directoryPath));
            
            foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                FileInfo[] files = subDir.GetFiles("*", SearchOption.AllDirectories);
                IEnumerable<string> executables = files.Where(x => _executableFileDetector.IsFileExecutable(x.Name)).Select(x => x.FullName);

                foreach (string executable in executables)
                {
                    if (executable.Equals(executableName))
                    {
                        output.Add(executable);
                    }
                }
            }

            return Task.FromResult<IEnumerable<string>>(output);
        }
    }
}