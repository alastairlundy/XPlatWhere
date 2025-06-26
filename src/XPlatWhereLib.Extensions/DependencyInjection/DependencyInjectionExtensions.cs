/*
    XPlatWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using Microsoft.Extensions.DependencyInjection;

using XPlatWhereLib.Abstractions.Executables;
using XPlatWhereLib.Abstractions.Prioritizers;

using XPlatWhereLib.Executables;

using XPlatWhereLib.Prioritizers;

namespace WhatExecLib.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddWhatExecLib(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                    
                services.AddScoped<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddScoped<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddScoped<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddScoped<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddScoped<IPrioritizedExecutableFileLocator, PrioritizedExecutableFileLocator>();
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                    
                services.AddSingleton<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddSingleton<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddSingleton<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddSingleton<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddSingleton<IPrioritizedExecutableFileLocator, PrioritizedExecutableFileLocator>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                    
                services.AddTransient<IExecutableFileDetector, ExecutableFileDetector>();
                services.AddTransient<IExecutableFileLocator, ExecutableFileLocator>();
                services.AddTransient<IExecutableFileInstancesLocator, ExecutableFileInstancesLocator>();
                services.AddTransient<IMultiExecutableLocator, MultiExecutableLocator>();
                services.AddTransient<IPrioritizedExecutableFileLocator, PrioritizedExecutableFileLocator>();
                break;
        }
            
        return services;
    }
}