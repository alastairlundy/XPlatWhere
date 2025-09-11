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
using AlastairLundy.XPlatWhereLib.Abstractions.Executables;
using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Executables;

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
    /// <param name="directory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public IEnumerable<FileInfo> LocateAllExecutablesWithinDirectoryAsync(DirectoryInfo directory)
    {
        IEnumerable<FileInfo> files = _multiFileLocator.LocateAllFilesWithinDirectoryAsync(directory);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }

    /// <summary>
    /// </summary>
    /// <param name="driveInfo"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public IEnumerable<FileInfo> LocateAllExecutablesWithinDriveAsync(DriveInfo driveInfo)
    {
        IEnumerable<FileInfo> files = _multiFileLocator.LocateAllFilesWithinDriveAsync(driveInfo);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }
}