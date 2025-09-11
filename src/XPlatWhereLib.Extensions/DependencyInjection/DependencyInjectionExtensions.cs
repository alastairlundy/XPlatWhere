/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using AlastairLundy.XPlatWhereLib.Abstractions.Executables;
using AlastairLundy.XPlatWhereLib.Abstractions.Files;
using AlastairLundy.XPlatWhereLib.Abstractions.Prioritizers;

using AlastairLundy.XPlatWhereLib.Executables;
using AlastairLundy.XPlatWhereLib.Executables.Detectors;
using AlastairLundy.XPlatWhereLib.Files;
using AlastairLundy.XPlatWhereLib.Prioritizers;

using Microsoft.Extensions.DependencyInjection;

namespace XPlatWhereLib.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddXPlatWhere(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddScoped<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddScoped<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddScoped<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddScoped<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddScoped<IFileLocator, FileLocator>();
                services.AddScoped<IMultiFileLocator, MultiFileLocator>();
                services.AddScoped<IFileInstancesLocator, FileInstancesLocator>();
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddSingleton<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddSingleton<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddSingleton<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddSingleton<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddSingleton<IFileLocator, FileLocator>();
                services.AddSingleton<IMultiFileLocator, MultiFileLocator>();
                services.AddSingleton<IFileInstancesLocator, FileInstancesLocator>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddTransient<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddTransient<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddTransient<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddTransient<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddTransient<IFileLocator, FileLocator>();
                services.AddTransient<IMultiFileLocator, MultiFileLocator>();
                services.AddTransient<IFileInstancesLocator, FileInstancesLocator>();
                break;
        }

        return services;
    }
}