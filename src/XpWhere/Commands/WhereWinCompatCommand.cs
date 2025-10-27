using System.Threading;
using Spectre.Console.Cli;
using XpWhere.Settings;

namespace XpWhere.Commands;

public class WhereWinCompatCommand : Command<WinCompatSettings>
{

    public override int Execute(CommandContext context, WinCompatSettings settings, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}