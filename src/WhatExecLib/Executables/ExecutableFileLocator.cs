/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WhatExecLib.Abstractions.Executables;
using WhatExecLib.Abstractions.Files;

#if NET5_0_OR_GREATER
#endif

namespace WhatExecLib.Executables;

/// <summary>
/// A class to help find an executable when you don't know where it is.
/// </summary>
public class ExecutableFileLocator : IExecutableFileLocator
{
    private readonly IFileLocator _fileLocator;
    private readonly IExecutableFileDetector _executableFileDetector;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileLocator"></param>
    /// <param name="executableFileDetector"></param>
    public ExecutableFileLocator(IFileLocator fileLocator, IExecutableFileDetector executableFileDetector)
    {
        _executableFileDetector = executableFileDetector;
        _fileLocator = fileLocator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executableName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> LocateExecutableAsync(string executableName, CancellationToken cancellationToken = default)
    {
        string file = await _fileLocator.LocateFileAsync(executableName, cancellationToken);

        if (_executableFileDetector.IsFileExecutable(file) && 
            _executableFileDetector.DoesFileHaveExecutablePermissions(file))
        {
            return file;
        }
        else
        {
            return string.Empty;
        }
    }
}