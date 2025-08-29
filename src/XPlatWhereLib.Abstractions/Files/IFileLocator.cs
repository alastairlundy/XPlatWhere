/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading;
using System.Threading.Tasks;

namespace XPlatWhereLib.Abstractions.Files;

/// <summary>
/// 
/// </summary>
public interface IFileLocator
{
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> LocateFileAsync(string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directoryPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsFileInDirectoryAsync(string fileName, string directoryPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="driveName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsFileWithinDriveAsync(string fileName, string driveName, CancellationToken cancellationToken = default);
}