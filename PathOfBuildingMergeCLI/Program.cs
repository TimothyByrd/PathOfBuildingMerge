using System.CommandLine;
using System.CommandLine.Parsing;
using PathOfBuildingMergeUtils;

namespace PathOfBuildingMergeCLI;

internal static class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand("PobMergeCLI - merge Path of Building loadouts from the command line");

        var mainOption = new Option<string?>(
            new[] { "--main", "-m" },
            "Main PoB file to merge into. Omit to start from an empty PoB (requires --output).");

        var mergeOption = new Option<string[]>(
            new[] { "--merge", "-M" },
            "PoB file to merge in. Repeat for multi-merge.");
        mergeOption.IsRequired = true;

        var loadoutOption = new Option<string?>(
            new[] { "--loadout", "-l" },
            "Name for the new loadout. Defaults to merge filename.");

        var outputOption = new Option<string?>(
            new[] { "--output", "-o" },
            "Output file. Defaults to overwriting --main.");

        var allItemsOption = new Option<bool>(
            new[] { "--all-items" },
            "Import all items, not just those used by the loadout.");

        var noReuseOption = new Option<bool>(
            new[] { "--no-reuse" },
            "Always add items as new copies; don't match duplicates.");

        rootCommand.Add(mainOption);
        rootCommand.Add(mergeOption);
        rootCommand.Add(loadoutOption);
        rootCommand.Add(outputOption);
        rootCommand.Add(allItemsOption);
        rootCommand.Add(noReuseOption);

        rootCommand.SetHandler((main, mergeFiles, loadout, output, allItems, noReuse) =>
        {
            Environment.Exit(ExecuteMerge(main, mergeFiles, loadout, output, allItems, noReuse));
        }, mainOption, mergeOption, loadoutOption, outputOption, allItemsOption, noReuseOption);

        return rootCommand.Invoke(args);
    }

    private static int ExecuteMerge(string? mainPob, string[] mergeFiles, string? loadoutName, string? outputPob, bool allItems, bool noReuse)
    {
        bool onlyAddUsedItems = !allItems;
        bool reuseExistingItems = !noReuse;

        bool startingWithEmptyPoB = false;
        if (string.IsNullOrWhiteSpace(mainPob))
        {
            mainPob = Path.Combine(AppContext.BaseDirectory, "empty.xml");
            startingWithEmptyPoB = true;
        }

        if (!File.Exists(mainPob))
        {
            Console.Error.WriteLine($"Error: Main PoB file not found: {mainPob}");
            return 1;
        }

        foreach (var file in mergeFiles)
        {
            if (!File.Exists(file))
            {
                Console.Error.WriteLine($"Error: Merge file not found: {file}");
                return 1;
            }
        }

        if (string.IsNullOrWhiteSpace(outputPob))
        {
            outputPob = startingWithEmptyPoB ? null : mainPob;
        }

        if (string.IsNullOrWhiteSpace(outputPob))
        {
            Console.Error.WriteLine("Error: --output is required when no --main is specified.");
            return 1;
        }

        if (mergeFiles.Length > 1)
        {
            foreach (var file in mergeFiles)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                try
                {
                    PobMergeUtils.Merge(mainPob, file, name, outputPob,
                        onlyAddUsedItems: onlyAddUsedItems,
                        reuseExistingItems: reuseExistingItems);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error merging '{file}': {ex.Message}");
                    return 1;
                }
                mainPob = outputPob;
            }
            Console.WriteLine($"Merged {mergeFiles.Length} PoB(s) into '{Path.GetFileName(outputPob)}'.");
            return 0;
        }

        var singleMerge = mergeFiles[0];
        if (string.IsNullOrWhiteSpace(loadoutName))
            loadoutName = Path.GetFileNameWithoutExtension(singleMerge);

        try
        {
            PobMergeUtils.Merge(mainPob, singleMerge, loadoutName, outputPob,
                onlyAddUsedItems: onlyAddUsedItems,
                reuseExistingItems: reuseExistingItems);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }

        Console.WriteLine($"Merged '{Path.GetFileName(singleMerge)}' into '{Path.GetFileName(outputPob)}' as loadout '{loadoutName}'.");
        return 0;
    }
}
