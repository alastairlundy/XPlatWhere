/*
    XpWhereLib
    Copyright (c) 2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using AlastairLundy.XpWhereLib.Abstractions.Files;
using AlastairLundy.XpWhereLib.Abstractions.Prioritizers;

using AlastairLundy.XpWhereLib.Files;
using AlastairLundy.XpWhereLib.Prioritizers;

using Microsoft.Extensions.DependencyInjection;

namespace XpWhereLib.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddXpWhere(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddScoped<IMultiFileLocator, MultiFileLocator>();
                services.AddScoped<IFileInstancesLocator, FileInstancesLocator>();
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddSingleton<IMultiFileLocator, MultiFileLocator>();
                services.AddSingleton<IFileInstancesLocator, FileInstancesLocator>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IDirectoryListPrioritizer, DirectoryListPrioritizer>();
                services.AddTransient<IMultiFileLocator, MultiFileLocator>();
                services.AddTransient<IFileInstancesLocator, FileInstancesLocator>();
                break;
        }

        return services;
    }
}