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
using System.Threading.Tasks;

using XPlatWhereLib.Abstractions.Files;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace XPlatWhereLib.Files;

public class MultiFileLocator : IMultiFileLocator
{

    /// <summary>
    /// 
    /// </summary>
    public MultiFileLocator()
    {
        
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
    public async Task<IEnumerable<string>> LocateAllFilesWithinDirectoryAsync(string folder)
    {
        ConcurrentBag<string> output = new ConcurrentBag<string>();

        await Task.Run(() =>
        {
            string folderPath = Path.GetFullPath(folder);
        
            if (Directory.Exists(folderPath) == false)
            {
                throw new DirectoryNotFoundException(folder);
            }
        
            string[] directories = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories);
        
            foreach (string directory in directories)
            {
                IEnumerable<string> files = Directory.GetFiles(directory);

                foreach (string file in files)
                {
                    output.Add(file);
                }
            }
        });
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
    public async Task<IEnumerable<string>> LocateAllFilesWithinDriveAsync(DriveInfo driveInfo)
    {
        ConcurrentBag<string> output = new();

        DirectoryInfo rootDir = driveInfo.RootDirectory;

        await Task.Run(() =>
        {
            Parallel.ForEach(rootDir.GetDirectories("*", SearchOption.AllDirectories), async void (subDir) =>
            {
                IEnumerable<string> executables = await LocateAllFilesWithinDirectoryAsync(subDir.FullName);

                foreach (string executable in executables)
                {
                    output.Add(executable);
                }
            });
        });

        return output;
    }
}