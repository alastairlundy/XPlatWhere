using System.Threading;
using System.Threading.Tasks;

namespace WhatExecLib.Executables.Abstractions
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
    }
}