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
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using WhatExecLib.Abstractions;

namespace WhatExecLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutableFileLocator : IExecutableFileLocator
    {
        public ExecutableFileLocator(IExecutableFileDetector executableDetector)
        {
        
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
        public IEnumerable<string> LocateExecutable(string executableName)
        {
            List<string> output = new List<string>();
       
            string[] drives = Directory.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DriveInfo driveInfo = new DriveInfo(drive);
           
                DirectoryInfo rootDir = driveInfo.RootDirectory;

                foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
                {
                    IEnumerable<string> executables = LocateAllExecutablesWithinFolder(subDir.FullName);

                    foreach (string executable in executables)
                    {
                        if (executable.Equals(executableName))
                        {
                            output.Add(executable);
                        }
                    }
                }
            }

            if (output.Count == 0)
            {
                return [];
            }
            else
            {
                return output;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IEnumerable<string> LocateAllExecutablesWithinFolder(string folder)
        {
            List<string> output = new List<string>();

            string folderPath = Path.GetFullPath(folder);
        
            if (Directory.Exists(folderPath) == false)
            {
                throw new DirectoryNotFoundException(folder);
            }
        
            string[] directories = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories);
        
            foreach (string directory in directories)
            {
                IEnumerable<string> files = Directory.GetFiles(directory);

                FileInfo fileInfo = new FileInfo(directory);
            
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    files = files.Where(x => Path.GetExtension(x) == ".exe" ||
                                             x == Path.GetExtension(".appx") ||
                                             x == Path.GetExtension(".msi") ||
                                             x == Path.GetExtension(".jar") ||
                                             x == Path.GetExtension(".bat"));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
#pragma warning disable CA1416
                    files = files.Where(x => File.GetUnixFileMode(x) == UnixFileMode.UserExecute ||
                                             File.GetUnixFileMode(x) == UnixFileMode.GroupExecute ||
                                             File.GetUnixFileMode(x) == UnixFileMode.OtherExecute ||
                                             x == Path.GetExtension(".appimage") ||
                                             x == Path.GetExtension(".deb") ||
                                             x == Path.GetExtension(".rpm"));
#pragma warning restore CA1416
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
#pragma warning disable CA1416
                    files = files.Where(x => File.GetUnixFileMode(x) == UnixFileMode.UserExecute ||
                                             File.GetUnixFileMode(x) == UnixFileMode.GroupExecute ||
                                             File.GetUnixFileMode(x) == UnixFileMode.OtherExecute ||
                                             Path.GetExtension(x) == ".pkg" || Path.GetExtension(x) == ".app");
#pragma warning restore CA1416
                }
            
                output.AddRange(files);
            }
        
            return output;
        }
    }
}