/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading;
using System.Threading.Tasks;

namespace AlastairLundy.XPlatWhereLib.Abstractions.Executables;

/// <summary>
/// 
/// </summary>
public interface IExecutableFileLocator
{
    
    /// <summary>
    /// </summary>
    /// <param name="executableName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> LocateExecutableAsync(string executableName, CancellationToken cancellationToken = default);
}