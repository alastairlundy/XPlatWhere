using System.Threading;
using Spectre.Console.Cli;
using XpWhere.Settings;

namespace XpWhere.Commands;

public class XpWhereCommand : Command<XpWhereCommandSettings>
{
    public override int Execute(CommandContext context, XpWhereCommandSettings settings, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}