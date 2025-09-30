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

## Donation
<a name="h13" />

If this project helped you, you can help me :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=XE5JR3FR458ZE&currency_code=USD)


Note for self on tagging:

git tag -a v1.0.1 -m "Release version 1.0.1"
git push origin v1.0.1