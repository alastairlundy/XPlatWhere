/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using XPlatWhereLib.Abstractions.Prioritizers;

namespace XPlatWhereLib.Prioritizers;

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
    public IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories)
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
    public IList<string> Prioritize(DirectoryPriority priority, IEnumerable<string> directories, string? priorityDirectory)
    {
        switch (priority)
        {
            case DirectoryPriority.SystemDirectories:
                return PrioritizeSystemDirectories(directories);
            case DirectoryPriority.UserDirectories:
                return PrioritizeUserApplicationDirectories(directories);
            case DirectoryPriority.SpecifiedDirectory:
                if (priorityDirectory is not null)
                {
                    return PrioritizeDirectory(priorityDirectory, directories);
                }
                else
                {
                    return PrioritizeUserApplicationDirectories(directories);
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
        List<string> output =
        [
            priorityDirectory
        ];

        foreach (string directory in directories)
        {
            if (directory.Equals(priorityDirectory) == false)
            {
                output.Add(directory);
            }
        }
        
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directories"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
    public IList<string> PrioritizeUserApplicationDirectories(IEnumerable<string> directories)
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directories"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
    public IList<string> PrioritizeSystemDirectories(IEnumerable<string> directories)
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