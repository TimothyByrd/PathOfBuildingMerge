# PathOfBuildingMerge

This tool is to merge a build from one Path of Building save file into
another.

To use, run the tool and specify the following:

- The main PoB file to merge into.
- The merge PoB file to take one loadout from and copy into the main PoB.
- The name of the new loadout. If blank, the new loadout will be given the name of the merge file.
- The name of the PoB file to save the result to. If blank, the main PoB file will be saved on top of.

If the main PoB file already has sets (item, skill, config, tree) with
the same name as the new loadout name, those existing sets will be
deleted.

If the PoB being merged has more than one set of items, skills, etc.,
it will try to merge in the currently active ones. So if you select
the loadout you want it import and then save to merge PoB, that
loadout should be the one that gets imported.

The tool checks for items that already exist in the main PoB and tries
to use them instead of importing a new copy. This should help reduce
duplicates when importing multiple snapshots of a build from
poe.ninja.

Path of Building is written in Lua, but I haven't worked with Lua so
this is in C#. I built it using Visual Studio Community Edition.

## Sample Usage

### Example: Creating a PoB of a set of snapshots from poe.ninja

Here is an example of creating a PoB with several build snapshots.

1. Look up a character on poe.ninja.
2. For each snapshot on poe.ninja:
    - Open the snapshot in Path of Building
    - Make sure the config has the pantheon and bandit choice set correctly for the snapshot.
    - Save the snapshot with a good name. For example, for the "Hour 3" snapshot, I save the PoB as "Hour 3". (I'm clever that way.)
3. Create a new, empty build in Path of Building and immediately save it. (We'll pretend I've saved it as "All the Snapshots".)
4. Run the PathOfBuildingMergeTool.
5. For the Main PoB File, select the snapshots build you saved in step 3.
6. For each snapshot ("Hour 3", etc.) that you saved from poe.ninja:
    - Select that snapshot file as the PoB file to merge in.
    - Click the "Merge" button.
7. Open "All the Snapshots" in Path of Building.
8. Optionally clean things up by deleting the "Default" sets (tree, items, skills and config).

### Example: Copying a loadout from one PoB to another.

1. Open the PoB with the loadout you want to copy.
2. Make sure the loadout you want to copy is currently selected and save the PoB.
3. Run the PathOfBuildingMergeTool.
4. Select the PoB you want the loadout into as the Main PoB file.
5. Select the PoB with the loadout you want to copy as the PoB file to merge in.
6. Set the New loadout name to something.
7. Leave the Output PoB blank.
8. Click the Merge button.

## Donation

If this project helped you, you can help me :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=XE5JR3FR458ZE&currency_code=USD)

## Notes for self on tagging for releases:

git tag -a v1.0.1 -m "Release version 1.0.1"
git push origin v1.0.1
