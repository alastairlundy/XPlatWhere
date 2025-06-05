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