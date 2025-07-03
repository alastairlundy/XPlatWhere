// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhatExecLib.Extensions.DependencyInjection;
using XPlatWhere.Cli;
using XPlatWhere.Cli.Commands;
using XPlatWhere.Cli.Commands.Helpers;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
            {
                services.AddXPlatWhere();
                services.AddSingleton<VersionCommand>();
                services.AddSingleton<FindFilesCommand>();
                services.AddSingleton<FindExecutablesCommand>();
            }
        );

using IHost host = CreateHostBuilder(args).Build();

// Disposes of everything once the application exits.
using IServiceScope scope = host.Services.CreateScope();

IServiceProvider services = scope.ServiceProvider;

if (args.Any(x => x == "--version"))
{
    //This is called RunAsync, but VersionCommand isn't async.
    int result = services.GetRequiredService<VersionCommand>()
        .RunAsync().Result;

    if (result != 0)
    {
        return result;
    }
}

if (args.Any(x => x.ToLower() == "/e"))
{
   return await services.GetRequiredService<FindExecutablesCommand>().RunAsync();
}
else if(args.Any())
{
    return  await services.GetRequiredService<FindFilesCommand>().RunAsync();
}

return -1;