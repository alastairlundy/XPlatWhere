using System.Threading;
using System.Threading.Tasks;

namespace WhatExecLib.Files.Abstractions
{
    public interface IFileLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> LocateFileAsync(string fileName, CancellationToken cancellationToken = default);

        Task<bool> IsFileInDirectoryAsync(string fileName, string directoryPath,
            CancellationToken cancellationToken = default);
    
        Task<bool> IsFileWithinDriveAsync(string fileName, string driveName, CancellationToken cancellationToken = default);
    }
}