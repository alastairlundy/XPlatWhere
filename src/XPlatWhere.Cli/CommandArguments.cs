using PowerArgs;

namespace XPlatWhere.Cli;

public class CommandArguments
{
    [ArgShortcut("/r")]
    public string? PriorityDirectory { get; set; }
    
    [ArgDefaultValue(false)]
    public bool DisplayOutputInQuotationMarks { get; set; }
    
    [ArgShortcut("/f")]
    [ArgDefaultValue(false)]
    public bool DisplayExitCodeOnly { get; set; }
    
    [ArgShortcut("/t")]
    [ArgDefaultValue(false)]
    public bool DisplayFileSizeAndLastModifiedForFiles { get; set; }
    
    [ArgShortcut("/e")]
    [ArgDefaultValue(false)]
    public bool LookForExecutablesOnly { get; set; }
    
    [ArgShortcut("/n")]
    [ArgDefaultValue(int.MaxValue)]
    public int NumberOfFilesToLookFor { get; set; }
    
    [HelpHook]
    [ArgShortcut("/?")]
    [ArgDefaultValue(false)]
    public bool HelpCommandRequested { get; set; }
    
    [ArgRequired(PromptIfMissing=true)]
    [ArgPosition(0)]
    public string SearchPattern { get; set; } = string.Empty;

    public bool SearchPatternIncludesWildCards => SearchPattern.Contains('*') || SearchPattern.Contains('?');
    
    [ArgCantBeCombinedWith("/r")]
    [ArgShortcut("$")]
    public string PathToSearch { get; set; }
}