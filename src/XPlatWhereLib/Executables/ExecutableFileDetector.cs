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
using System.Runtime.Versioning;

using AlastairLundy.DotPrimitives.IO.Permissions;
using AlastairLundy.DotPrimitives.IO.Permissions.Windows;

using AlastairLundy.Resyslib.IO.Core.Extensions;

using AlastairLundy.XPlatWhereLib.Abstractions.Executables;

namespace AlastairLundy.XPlatWhereLib.Executables;

/// <summary>
/// </summary>
public class ExecutableFileDetector : IExecutableFileDetector
{
    
    /// <summary>
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
#pragma warning disable CA1416
            return DoesFileHaveExecutablePermissions(fullPath) ||
                   //     IsUnixElfFile(fullPath) || 
                   //    IsMachOFile(fullPath) ||
                   Path.GetExtension(fullPath) == ".pkg" ||
                   Path.GetExtension(fullPath) == ".app";
#pragma warning restore CA1416
        }

        if (OperatingSystem.IsFreeBSD())
            return DoesFileHaveExecutablePermissions(fullPath) ||
                   // IsUnixElfFile(fullPath) || 
                   // IsMachOFile(fullPath) ||
                   Path.GetExtension(fullPath) == ".pkg" ||
                   Path.GetExtension(fullPath) == ".appimage";

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    public bool DoesFileHaveExecutablePermissions(string filename)
    {
        string fullPath = Path.GetFullPath(filename);
       
        if (File.Exists(fullPath) == false)
            throw new FileNotFoundException();
        
        if (OperatingSystem.IsWindows())
        {
            WindowsFilePermission filePermission = WindowsFilePermissionManager.GetFilePermission(fullPath);
            
           return filePermission.HasExecutePermission();
        }
        else if (OperatingSystem.IsLinux() ||
                 OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst()
                 || OperatingSystem.IsFreeBSD()
                 || OperatingSystem.IsAndroid())
        {
#pragma warning disable CA1416
            UnixFileMode fileMode = File.GetUnixFileMode(fullPath);
#pragma warning restore CA1416

            return fileMode.HasFlag(UnixFileMode.OtherExecute) ||
                   fileMode.HasFlag(UnixFileMode.GroupExecute) ||
                   fileMode.HasFlag(UnixFileMode.UserExecute);
        }

        return false;
    }
}