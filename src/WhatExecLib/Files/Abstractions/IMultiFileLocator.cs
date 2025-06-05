/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WhatExecLib.Files.Abstractions
{
    public interface IMultiFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        Task<IEnumerable<string>> LocateAllFilesWithinDirectoryAsync(string folder);

        Task<IEnumerable<string>> LocateAllFilesWithinDriveAsync(DriveInfo driveInfo);
    }
}