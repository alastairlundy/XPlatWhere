using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WhatExecLib.Abstractions
{
    public interface IExecutableFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> LocateExecutableAsync(string executableName, CancellationToken cancellationToken = default);

        Task<bool> IsExecutableInDirectoryAsync(string executableName, string directoryPath,
            CancellationToken cancellationToken = default);
    
        Task<bool> IsExecutableInDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default);

        //   IEnumerable<string> LocateAllExecutablesWithinFolder(string folder);

        //   IEnumerable<string> LocateAllExecutablesWithinDrive(DriveInfo driveInfo);
    }
}