using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.DotExtensions.IO;
using AlastairLundy.DotPrimitives.Collections.Enumerables.Cached;

using XPlatWhereLib.Abstractions.Executables;

namespace XPlatWhere.Cli.Commands;

public class FindExecutablesCommand : ICliCommand
{
    private readonly IExecutableFileInstancesLocator _executableFileInstancesLocator;
    private readonly IExecutableFileLocator _executableFileLocator;

    public FindExecutablesCommand(IExecutableFileInstancesLocator executableFileInstancesLocator,
        IExecutableFileLocator executableFileLocator)
    {
        _executableFileInstancesLocator = executableFileInstancesLocator;
        _executableFileLocator = executableFileLocator;
    }

    public async Task<int> RunAsync(CommandArguments commandArguments)
    {
        IRefreshableCachedEnumerable<string> results;

        if (commandArguments.NumberOfFilesToLookFor > 1)
        {
           IEnumerable<string> enumerable = await _executableFileInstancesLocator.LocateExecutableInstancesAsync(commandArguments.SearchPattern);
           results = new RefreshableCachedEnumerable<string>(enumerable, EnumerableMaterializationMode.Lazy);
        }
        else
        {
            string result = await _executableFileLocator.LocateExecutableAsync(commandArguments.SearchPattern);

            results = new RefreshableCachedEnumerable<string>([result], EnumerableMaterializationMode.Lazy);
        }

        if (commandArguments.DisplayExitCodeOnly)
        {
            if (results.Any())
            {
                Console.WriteLine(0);
            }
            else
            {
                Console.WriteLine(-1);
            }
        }

        if (commandArguments.DisplayOutputInQuotationMarks)
        {
            results.RefreshCache(results.Select(x => x.Insert(0, '"'.ToString())
                .Insert(x.Length - 1, '"'.ToString())));
        }

        if (commandArguments.DisplayFileSizeAndLastModifiedForFiles)
        {
            foreach (string result in results)
            {
                string actual = Path.GetFullPath(result.Replace('"'.ToString(), string.Empty));

                var file = new FileInfo(actual);

               string fileSize = file.GetFileSizeString();
                DateTime lastModified = File.GetLastWriteTime(actual);

                string output = $"{result} {fileSize} {lastModified.ToShortDateString()} {lastModified.ToShortTimeString()}";
                
                Console.WriteLine(output);
            }
        }
        else
        {
            foreach (string result in results)
            {
                Console.WriteLine(result);
            }
        }
        
        return 0;
    }
}