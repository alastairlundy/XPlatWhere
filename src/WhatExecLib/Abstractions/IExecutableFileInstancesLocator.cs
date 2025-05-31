/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.IO;

namespace WhatExecLib.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecutableFileInstancesLocator
    {
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <returns></returns>
        IEnumerable<string> LocateExecutableInstances(string executableName);
        
        IEnumerable<string> LocateExecutableInstancesWithinDrive(DriveInfo driveInfo, string executableName);
        
        IEnumerable<string> LocateExecutableInstancesWithinDirectory(string directoryPath, string executableName);

    }
}