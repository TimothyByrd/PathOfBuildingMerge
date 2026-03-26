using System.CommandLine;
using PathOfBuildingMergeUtils;

namespace PathOfBuildingMergeCLI;

internal static class Program
{
    static int Main(string[] args)
    {
        var mainOption = new Option<string?>("--main", "-m")
        {
            Description = "Main PoB file to merge into. Omit to start from an empty PoB (requires --output).",
        };

        var mergeOption = new Option<string[]>("--merge", "-M")
        {
            Description = "PoB file to merge in. Repeat for multi-merge.",
            Required = true,
        };

        var loadoutOption = new Option<string?>("--loadout", "-l")
        {
            Description = "Name for the new loadout. Defaults to merge filename.",
        };

        var outputOption = new Option<string?>("--output", "-o")
        {
            Description = "Output file. Defaults to overwriting --main.",
        };

        var allItemsOption = new Option<bool>("--all-items")
        {
            Description = "Import all items, not just those used by the loadout.",
        };

        var noReuseOption = new Option<bool>("--no-reuse")
        {
            Description = "Always add items as new copies; don't match duplicates.",
        };

        var noAutoTagOption = new Option<bool>("--no-autotag")
        {
            Description = "Do not automatically give merged loadouts a unique tag like {1}, {2}, etc.",
        };

        var rootCommand = new RootCommand("PobMergeCLI - merge Path of Building loadouts from the command line")
        {
            mainOption, mergeOption, loadoutOption, outputOption, allItemsOption, noReuseOption, noAutoTagOption,
        };

        rootCommand.SetAction(parseResult =>
            {
                var main = parseResult.GetValue(mainOption);
                var mergeFiles = parseResult.GetValue(mergeOption);
                var loadout = parseResult.GetValue(loadoutOption);
                var output = parseResult.GetValue(outputOption);
                var allItems = parseResult.GetValue(allItemsOption);
                var noReuse = parseResult.GetValue(noReuseOption);
                var noAutoTag = parseResult.GetValue(noAutoTagOption);

                if (mergeFiles != null && mergeFiles.Length > 0)
                    ExecuteMerge(main, mergeFiles, loadout, output, allItems, noReuse, noAutoTag);
            });

        return rootCommand.Parse(args).Invoke();
    }

    private static int ExecuteMerge(string? mainPob, string[] mergeFiles, string? loadoutName, string? outputPob, bool allItems, bool noReuse, bool noAutoTag)
    {
        bool onlyAddUsedItems = !allItems;
        bool reuseExistingItems = !noReuse;
        bool autoTag = !noAutoTag;
        
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
                        reuseExistingItems: reuseExistingItems,
                        autoTag: autoTag);
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
                reuseExistingItems: reuseExistingItems,
                autoTag: autoTag);
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
