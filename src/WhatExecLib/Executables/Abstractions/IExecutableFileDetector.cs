/*
    WhatExecLib
    Copyright (c) 2024-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace WhatExecLib.Executables.Abstractions
{
    /// <summary>
    /// Provides an interface for detecting executable files based on their file type and system permissions.
    /// </summary>
    public interface IExecutableFileDetector
    {
        
        /// <summary>
        /// Checks if a given file is executable by verifying its file type.
        /// </summary>
        /// <param name="filename">The path to the file to check for executability.</param>
        /// <returns>True if the file can be executed, false otherwise.</returns>
        bool IsFileExecutable(string filename);
    
        /// <summary>
        /// Determines whether a specified file has executable permissions.
        /// </summary>
        /// <param name="filename">The path and file name of the file to check.</param>
        /// <returns>True if the file has execute permissions, false otherwise.</returns>
        bool DoesFileHaveExecutablePermissions(string filename);
    }
}