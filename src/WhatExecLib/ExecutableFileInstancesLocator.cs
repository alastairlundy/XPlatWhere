/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using WhatExecLib.Abstractions;

namespace WhatExecLib
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
        public IEnumerable<string> LocateExecutableInstances(string executableName)
        {
            ConcurrentBag<string> output = new ConcurrentBag<string>();
       
            string[] drives = Directory.GetLogicalDrives();

            Parallel.ForEach(drives, drive =>
            {
                IList<string> executablesWithinDrive = LocateExecutableInstancesWithinDrive(new DriveInfo(drive), executableName).ToList();
                
                if (executablesWithinDrive.Count > 0)
                {
                    foreach (string executable in executablesWithinDrive)
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

        public IEnumerable<string> LocateExecutableInstancesWithinDrive(DriveInfo driveInfo, string executableName)
        {
            List<string> output = new List<string>();
            
            DirectoryInfo rootDir = driveInfo.RootDirectory;

            foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                IEnumerable<string> executables = LocateExecutableInstancesWithinDirectory(subDir.FullName, executableName);

                foreach (string executable in executables)
                {
                    if (executable.Equals(executableName))
                    {
                        output.Add(executable);
                    }
                }
            }
            
            return output;
        }

        public IEnumerable<string> LocateExecutableInstancesWithinDirectory(string directoryPath, string executableName)
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
            
            return output;
        }
    }
}