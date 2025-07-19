using System.Threading.Tasks;

namespace XPlatWhere.Cli;

public interface ICliCommand
{
    Task<int> RunAsync(CommandArguments commandArguments);
}