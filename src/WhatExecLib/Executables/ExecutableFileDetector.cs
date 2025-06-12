/*
    WhatExecLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

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
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
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
                           Path.GetExtension(fullPath) == ".appimage";
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
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
#endif
        private bool IsUnixElfFile(string filename)
        {
            try
            {
                if (File.Exists(filename) == false || OperatingSystem.IsLinux() == false
                    && OperatingSystem.IsMacOS() == false && OperatingSystem.IsFreeBSD() == false)
                {
                    return false;
                }

                using (FileStream fs = new FileStream(Path.GetFullPath(filename), FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        char[] buffer;

                        if (Environment.Is64BitOperatingSystem)
                        {
                           buffer = new char[64];
                        }
                        else
                        {
                            buffer = new char[52];
                        }

                        sr.Read(buffer, 0, buffer.Length);

                        string readData = Encoding.Default.
                    }
                }
                
                //TODO: Finish working on Unix Elf file detection.
                
                
            }
            catch
            {
                return false;
            }
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
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
#endif
        private bool IsMachOFile(string filename)
        {
            try
            {
                if (File.Exists(filename) == false || OperatingSystem.IsLinux() == false
                    && OperatingSystem.IsMacOS() == false && OperatingSystem.IsFreeBSD() == false)
                {
                    return false;
                }
                
                //TODO: Implement Mach-O file detection
            }
            catch
            {
                return false;
            }
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
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                     #if NET5_0_OR_GREATER
                     || OperatingSystem.IsFreeBSD()
                     || OperatingSystem.IsAndroid()
                     #endif
                     )
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