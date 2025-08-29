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
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhereLib.Files;

/// <summary>
/// </summary>
public class FileInstancesLocator : IFileInstancesLocator
{
    public FileInstancesLocator()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public async Task<IEnumerable<string>> LocateFileInstancesAsync(string fileName)
    {
        DriveInfo[] drives = DriveInfo.GetDrives().Where(x => x.IsReady).ToArray();

        IEnumerable<string> output = await LocateFileInstancesAsync_Net50_OrNewer(fileName, drives);

        return output;
    }

    private async Task<IEnumerable<string>> LocateFileInstancesAsync_Net50_OrNewer(string fileName, DriveInfo[] drives)
    {
        ConcurrentBag<string> output = new ConcurrentBag<string>();

        await Parallel.ForEachAsync(drives, async (drive, token) =>
        {
            IEnumerable<string> filesWithinDrive = await LocateFileInstancesWithinDriveAsync(drive, fileName);

            IList<string> filesList = filesWithinDrive.ToList();

            if (filesList.Count > 0)
            {
                foreach (string file in filesList)
                {
                    output.Add(file);
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

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> LocateFileInstancesWithinDriveAsync(DriveInfo driveInfo, string fileName)
    {
        ConcurrentBag<string> output = new ConcurrentBag<string>();

        DirectoryInfo rootDir = driveInfo.RootDirectory;

        Parallel.ForEach(rootDir.GetDirectories("*", SearchOption.AllDirectories), async void (subDir) =>
        {
            IEnumerable<string> executables = await LocateFileInstancesWithinDirectory(subDir.FullName, fileName);

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
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> LocateFileInstancesWithinDirectory(string directoryPath, string fileName)
    {
        List<string> output = new List<string>();

        DirectoryInfo rootDir = new DirectoryInfo(Path.GetFullPath(directoryPath));

        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
        {
            FileInfo[] files = subDir.GetFiles("*", SearchOption.AllDirectories);
            IEnumerable<string> executables = files.Select(x => x.FullName);

            foreach (string executable in executables)
            {
                if (executable.Equals(fileName))
                {
                    output.Add(executable);
                }
            }
        }

        return output;
    }
}