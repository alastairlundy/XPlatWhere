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
using AlastairLundy.XPlatWhereLib.Abstractions.Executables;

namespace AlastairLundy.XPlatWhereLib.Executables;

/// <summary>
/// 
/// </summary>
public class ExecutableFileInstancesLocator : IExecutableFileInstancesLocator
{
    private readonly IExecutableFileDetector _executableFileDetector;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executableDetector"></param>
    public ExecutableFileInstancesLocator(IExecutableFileDetector executableDetector)
    {
        _executableFileDetector = executableDetector;
    }

    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public async Task<IEnumerable<string>> LocateExecutableInstancesAsync(string executableName)
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(x => x.IsReady);

        ConcurrentBag<string> output = new ConcurrentBag<string>();

        await Parallel.ForEachAsync(drives, async (drive, token) =>
        {
            IEnumerable<string> executablesWithinDrive = await LocateExecutableInstancesWithinDriveAsync(
                    drive, executableName);

            foreach (string executable in executablesWithinDrive)
            {
                output.Add(executable);
            }
        });

        return output.IsEmpty ? [] : output;
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> LocateExecutableInstancesWithinDriveAsync(DriveInfo driveInfo,
        string executableName)
    {
        ConcurrentBag<string> output = new ConcurrentBag<string>();

        DirectoryInfo rootDir = driveInfo.RootDirectory;

        await Parallel.ForEachAsync(rootDir.GetDirectories("*", SearchOption.AllDirectories),
            async (subDir, token) =>
            {
                IEnumerable<string> executables =
                    await LocateExecutableInstancesWithinDirectory(subDir.FullName, executableName);

                foreach (string executable in executables)
                {
                    output.Add(executable);
                }
            });

        return output;
    }

    /// <summary>
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    public Task<IEnumerable<string>> LocateExecutableInstancesWithinDirectory(string directoryPath,
        string executableName)
    {
        ConcurrentBag<string> output = new ConcurrentBag<string>();

        DirectoryInfo rootDir = new DirectoryInfo(Path.GetFullPath(directoryPath));

        foreach (DirectoryInfo subDir in rootDir.GetDirectories("*", SearchOption.AllDirectories))
        {
            IEnumerable<string> executables = subDir.GetFiles("*", SearchOption.AllDirectories)
                .Where(x => _executableFileDetector.IsFileExecutable(x.Name))
                .Select(x => x.FullName);

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