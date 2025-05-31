using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using WhatExecLib.Abstractions;
using WhatExecLib.Models;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace WhatExecLib
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryListPrioritizer : IDirectoryListPrioritizer
    {
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="directories"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IList<string> Prioritize(ExecutableDirectoryPriority priority, IEnumerable<string> directories)
        {
            return Prioritize(priority, directories, null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="directories"></param>
        /// <param name="priorityDirectory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IList<string> Prioritize(ExecutableDirectoryPriority priority, IEnumerable<string> directories, string? priorityDirectory)
        {
            switch (priority)
            {
                case ExecutableDirectoryPriority.SystemDirectories:
                    return PrioritizeSystemExecutables(directories);
                case ExecutableDirectoryPriority.UserDirectories:
                    return PrioritizeUserApplicationExecutables(directories);
                case ExecutableDirectoryPriority.SpecifiedDirectory:
                    if (priorityDirectory is not null)
                    {
                        return PrioritizeDirectory(priorityDirectory, directories);
                    }
                    else
                    {
                        return PrioritizeUserApplicationExecutables(directories);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, "Priority not supported");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priorityDirectory"></param>
        /// <param name="directories"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IList<string> PrioritizeDirectory(string priorityDirectory, IEnumerable<string> directories)
        {
            List<string> output = new();
        
            output.Add(priorityDirectory);

            foreach (string directory in directories)
            {
                if (directory.Equals(priorityDirectory) == false)
                {
                    output.Add(directory);
                }
            }
        
            return output;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IList<string> PrioritizeUserApplicationExecutables(IEnumerable<string> directories)
        {
            List<string> output = new List<string>();
        
            foreach (string directory in directories)
            {
                string fullPath = Path.GetFullPath(directory);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.Programs)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))
                    {
                    
                        output.Insert(0, fullPath);
                    }
                    else
                    {
                        output.Add(fullPath);
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                
                }
            }
        
            return output;
        }

    
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public IList<string> PrioritizeSystemExecutables(IEnumerable<string> directories)
        {
            List<string> output = new List<string>();
        
            foreach (string directory in directories)
            {
                string fullPath = Path.GetFullPath(directory);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.System)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.AdminTools)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) ||
                        fullPath.Equals(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)))
                    {
                    
                        output.Insert(0, fullPath);
                    }
                    else
                    {
                        output.Add(fullPath);
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                
                }
            }
        
            return output;
        }
    }
}