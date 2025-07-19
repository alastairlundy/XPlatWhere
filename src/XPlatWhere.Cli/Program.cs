// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerArgs;
using XPlatWhere.Cli;

using XPlatWhere.Cli.Commands;
using XPlatWhere.Cli.Commands.Helpers;

using XPlatWhereLib.Extensions.DependencyInjection;

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

CommandArguments parsedArgs = Args.Parse<CommandArguments>(args);

if (args.Any(x => x == "--version"))
{
    //This is called RunAsync, but VersionCommand isn't async.
    int result = services.GetRequiredService<VersionCommand>()
        .RunAsync(parsedArgs).Result;

    if (result != 0)
    {
        return result;
    }
}

if (parsedArgs.LookForExecutablesOnly)
{
   return await services.GetRequiredService<FindExecutablesCommand>().RunAsync(parsedArgs);
}
else if(args.Any())
{
    return  await services.GetRequiredService<FindFilesCommand>().RunAsync(parsedArgs);
}

return -1;