/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WhatExecLib.Abstractions.Executables;
using WhatExecLib.Abstractions.Files;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace WhatExecLib.Executables;

public class MultiExecutableLocator : IMultiExecutableLocator
{
    private readonly IMultiFileLocator _multiFileLocator;
    private readonly IExecutableFileDetector _executableFileDetector;

    public MultiExecutableLocator(IMultiFileLocator multiFileLocator,
        IExecutableFileDetector executableFileDetector)
    {
        _multiFileLocator = multiFileLocator;
        _executableFileDetector = executableFileDetector;
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
    public async Task<IEnumerable<string>> LocateAllExecutablesWithinDirectoryAsync(string folder)
    {
        IEnumerable<string> files = await _multiFileLocator.LocateAllFilesWithinDirectoryAsync(folder);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }

    
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
    public async Task<IEnumerable<string>> LocateAllExecutablesWithinDriveAsync(DriveInfo driveInfo)
    {
        IEnumerable<string> files = await _multiFileLocator.LocateAllFilesWithinDriveAsync(driveInfo);

        return files.Where(file => _executableFileDetector.IsFileExecutable(file));
    }
}