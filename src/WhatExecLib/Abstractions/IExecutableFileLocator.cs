/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;

namespace WhatExecLib.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecutableFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <returns></returns>
        IEnumerable<string> LocateExecutable(string executableName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        IEnumerable<string> LocateAllExecutablesWithinFolder(string folder);
    }
}