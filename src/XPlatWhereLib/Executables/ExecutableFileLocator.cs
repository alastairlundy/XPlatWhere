/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.XPlatWhereLib.Abstractions.Executables;
using AlastairLundy.XPlatWhereLib.Abstractions.Files;

namespace AlastairLundy.XPlatWhereLib.Executables;

/// <summary>
///     A class to help find an executable when you don't know where it is.
/// </summary>
public class ExecutableFileLocator : IExecutableFileLocator
{
    private readonly IExecutableFileDetector _executableFileDetector;
    private readonly IFileLocator _fileLocator;

    /// <summary>
    /// </summary>
    /// <param name="fileLocator"></param>
    /// <param name="executableFileDetector"></param>
    public ExecutableFileLocator(IFileLocator fileLocator, IExecutableFileDetector executableFileDetector)
    {
        _executableFileDetector = executableFileDetector;
        _fileLocator = fileLocator;
    }

    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <returns></returns>
    public async Task<string?> LocateExecutableAsync(string executableName)
    {
        string? file = await _fileLocator.LocateFile(executableName);

        if (file is null)
            return file;
        
        if (_executableFileDetector.IsFileExecutable(file) == false ||
            _executableFileDetector.DoesFileHaveExecutablePermissions(file) == false)
        {
            return string.Empty;
        }
        
        return file;
    }
}