/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using AlastairLundy.XPlatWhereLib.Abstractions.Executables;
using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Executables;

/// <summary>
/// 
/// </summary>
public class ExecutableFileInstancesLocator : IExecutableFileInstancesLocator
{
    private readonly IExecutableFileDetector _executableFileDetector;
    private readonly IFileInstancesLocator _fileInstancesLocator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executableDetector"></param>
    /// <param name="fileInstancesLocator"></param>
    public ExecutableFileInstancesLocator(IExecutableFileDetector executableDetector, 
        IFileInstancesLocator fileInstancesLocator)
    {
        _executableFileDetector = executableDetector;
        _fileInstancesLocator = fileInstancesLocator;
    }

    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public async Task<IEnumerable<string>> LocateExecutableInstancesAsync(string executableName)
    {
        IEnumerable<string> files = await _fileInstancesLocator.LocateFileInstancesAsync(executableName);
        
        return files.Where(file => _executableFileDetector.IsFileExecutable(file));   
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> LocateExecutableInstancesWithinDriveAsync(DriveInfo driveInfo,
        string executableName)
    {
        IEnumerable<string> files = await _fileInstancesLocator.LocateFileInstancesWithinDriveAsync(driveInfo,
            executableName);
        
        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }

    /// <summary>
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="executableName"></param>
    /// <returns></returns>
    public IEnumerable<string> LocateExecutableInstancesWithinDirectory(string directoryPath,
        string executableName)
    {
        IEnumerable<string> files = _fileInstancesLocator.LocateFileInstancesWithinDirectory(directoryPath,
            executableName);
        
        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }
}