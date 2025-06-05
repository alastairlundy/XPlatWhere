using System.Threading;
using System.Threading.Tasks;

namespace WhatExecLib.Files.Abstractions
{
    public interface IFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> LocateFileAsync(string executableName, CancellationToken cancellationToken = default);

        Task<bool> IsFileInDirectoryAsync(string executableName, string directoryPath,
            CancellationToken cancellationToken = default);
    
        Task<bool> IsFileWithinDriveAsync(string executableName, string driveName, CancellationToken cancellationToken = default);
    }
}