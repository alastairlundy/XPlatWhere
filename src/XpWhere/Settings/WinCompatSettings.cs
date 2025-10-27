using System.ComponentModel;
using Spectre.Console.Cli;

namespace XpWhere.Settings;

public class WinCompatSettings : CommandSettings
{
    [CommandOption("/q")]
    [DefaultValue(false)]
    public bool ShowExitCodeOnly { get; init; }
    
    [CommandOption("/f")]
    [DefaultValue(false)]
    public bool DisplayResultsInQuotes { get; init; }
    
    [CommandOption("/t")]
    [DefaultValue(false)]
    public bool DisplayFileSizeAndLastModified { get; init; }
}