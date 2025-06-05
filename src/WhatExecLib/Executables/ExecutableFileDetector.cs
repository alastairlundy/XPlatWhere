/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using AlastairLundy.DotExtensions.IO.Unix;
using WhatExecLib.Executables.Abstractions;

namespace WhatExecLib.Executables
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutableFileDetector : IExecutableFileDetector
    {
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public bool IsFileExecutable(string filename)
        {
            string fullPath = Path.GetFullPath(filename);
       
            if (File.Exists(fullPath) == false)
            {
                throw new FileNotFoundException();
            }
       
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.GetExtension(fullPath) == ".exe" ||
                       fullPath == Path.GetExtension(".appx") ||
                       fullPath == Path.GetExtension(".msi") ||
                       fullPath == Path.GetExtension(".jar") ||
                       fullPath == Path.GetExtension(".bat");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
#pragma warning disable CA1416
                return DoesFileHaveExecutablePermissions(fullPath) ||
                       IsUnixElfFile(fullPath) || 
                       fullPath == Path.GetExtension(".appimage") ||
                       fullPath == Path.GetExtension(".deb") ||
                       fullPath == Path.GetExtension(".rpm");
#pragma warning restore CA1416
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
#pragma warning disable CA1416
                return DoesFileHaveExecutablePermissions(fullPath) ||
                       IsUnixElfFile(fullPath) || 
                       IsMachOFile(fullPath) ||
                       Path.GetExtension(fullPath) == ".pkg" ||
                       Path.GetExtension(fullPath) == ".app";
#pragma warning restore CA1416
            }
#if NET5_0_OR_GREATER
            else
            {
                if (OperatingSystem.IsFreeBSD())
                {
                    return DoesFileHaveExecutablePermissions(fullPath) ||
                           IsUnixElfFile(fullPath) || 
                           IsMachOFile(fullPath) ||
                           Path.GetExtension(fullPath) == ".pkg" ||
                           Path.GetExtension(fullPath) == ".app";
                }
            }
#endif

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [UnsupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
#endif
        private bool IsUnixElfFile(string filename)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
#endif
        private bool IsMachOFile(string filename)
        {
            
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public bool DoesFileHaveExecutablePermissions(string filename)
        {
            string fullPath = Path.GetFullPath(filename);
       
            if (File.Exists(fullPath) == false)
            {
                throw new FileNotFoundException();
            }
        
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
#if NET5_0_OR_GREATER
#pragma warning disable CA1416
                return File.GetUnixFileMode(fullPath).IsExecutePermission();
#pragma warning restore CA1416
#else
#endif
            }

            return false;
        }
    }
}