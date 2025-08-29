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
using XPlatWhereLib.Abstractions.Executables;
using XPlatWhereLib.Abstractions.Files;

namespace XPlatWhereLib.Executables;

/// <summary>
/// </summary>
public class MultiExecutableLocator : IMultiExecutableLocator
{
    private readonly IExecutableFileDetector _executableFileDetector;
    private readonly IMultiFileLocator _multiFileLocator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiFileLocator"></param>
    /// <param name="executableFileDetector"></param>
    public MultiExecutableLocator(IMultiFileLocator multiFileLocator,
        IExecutableFileDetector executableFileDetector)
    {
        _multiFileLocator = multiFileLocator;
        _executableFileDetector = executableFileDetector;
    }

    /// <summary>
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public async Task<IEnumerable<string>> LocateAllExecutablesWithinDirectoryAsync(string folder)
    {
        IEnumerable<string> files = await _multiFileLocator.LocateAllFilesWithinDirectoryAsync(folder);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public async Task<IEnumerable<string>> LocateAllExecutablesWithinDriveAsync(DriveInfo driveInfo)
    {
        IEnumerable<string> files = await _multiFileLocator.LocateAllFilesWithinDriveAsync(driveInfo);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }
}