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
using System.Threading;
using System.Threading.Tasks;

using WhatExecLib.Files.Abstractions;

namespace WhatExecLib.Files
{
    public class FileLocator : IFileLocator
    {
        public async Task<string> LocateFileAsync(string executableName, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsFileInDirectoryAsync(string executableName, string directoryPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsFileWithinDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}