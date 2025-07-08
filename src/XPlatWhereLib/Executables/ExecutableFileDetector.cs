/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System;
using System.IO;
using System.Runtime.InteropServices;

using AlastairLundy.Resyslib.IO.Core.Extensions;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using XPlatWhereLib.Abstractions.Executables;

namespace XPlatWhereLib.Executables;

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
            return filename.IsExecutableExtension();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
#pragma warning disable CA1416
            return DoesFileHaveExecutablePermissions(fullPath) ||
                   //   IsUnixElfFile(fullPath) || 
                   fullPath == Path.GetExtension(".appimage") ||
                   fullPath == Path.GetExtension(".deb") ||
                   fullPath == Path.GetExtension(".rpm");
#pragma warning restore CA1416
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
#pragma warning disable CA1416
            return DoesFileHaveExecutablePermissions(fullPath) ||
                   //     IsUnixElfFile(fullPath) || 
                   //    IsMachOFile(fullPath) ||
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
                       // IsUnixElfFile(fullPath) || 
                       // IsMachOFile(fullPath) ||
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
            throw new FileNotFoundException();
        
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
            UnixFileMode fileMode = File.GetUnixFileMode(fullPath);
#pragma warning restore CA1416
#else
            UnixFileMode fileMode = FilePolyfill.GetUnixFileMode(fullPath);
#endif
            
            return fileMode.HasFlag(UnixFileMode.OtherExecute) ||
                   fileMode.HasFlag(UnixFileMode.GroupExecute) ||
                   fileMode.HasFlag(UnixFileMode.UserExecute);
        }

        return false;
    }
}