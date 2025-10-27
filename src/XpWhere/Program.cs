
using System;
using System.Globalization;

using Spectre.Console.Cli;

using XpWhere.Commands;

Console.WriteLine("Hello, World!");

CommandApp app = new CommandApp();

app.Configure(config =>
{
    config.CaseSensitivity(CaseSensitivity.Commands);
    config.SetApplicationCulture(CultureInfo.CurrentCulture);
    config.SetApplicationName("xpwhere");
    config.UseAssemblyInformationalVersion();

    config.AddCommand<XpWhereCommand>("file")
        .WithAlias("files");

    config.AddCommand<XpWhereCommand>("license")
        .WithAlias("-l")
        .WithAlias("--license");

    config.AddCommand<WhereWinCompatCommand>("compat")
        .WithAlias("compatibility");

});


return app.Run(args);