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
<a name="h10" />

### Example: Creating a PoB of a set of snapshots from poe.ninja

Here is an example of creating a PoB with several build snapshots.

1. I looked up the character Ben_Still_Clicking_MERC in the HC SSF Mercenaries League on poe.ninja.
2. I opened the latest snapshot in Path of building and saved it as "Ben_Still_Clicking_MERC".
    - By the way, I am saving all these PoBs together in a new folder to keep things straight and make the snapshots easier to delete later.
3. I opened snapshot "Day 4" in Path of building and saved it as "Day 4".
4. I opened snapshot "Day 5" in Path of building and saved it as "Day 5".
    - Warning: Sometimes when you open an older snapshot in Path of Building the gods in the pantheon are not set, so check that.
5. I opened snapshot "Day 6" in Path of building and saved it as "Day 6".
6. I opened snapshot "Week 1" in Path of building and saved it as "Week 1".
7. I opened snapshot "Week 2" in Path of building and saved it as "Week 2".
8. I opened snapshot "Week 3" in Path of building and saved it as "Week 3".
9. I opened snapshot "Week 4" in Path of building and saved it as "Week 4".
    - After week 3, the build doesn't seem to be changing, so I only did a few more snapshots.
10. I opened snapshot "Week 7" in Path of building and saved it as "Week 7".
11. I opened snapshot "Week 10" in Path of building and saved it as "Week 10".
12. I opened snapshot "Week 13" in Path of building and saved it as "Week 13".
13. I ran the PathOfBuildingMergeTool.
14. For the Main PoB File, I selected Ben_Still_Clicking_MERC.xml.
15. For the PoB file to merge in, I selected "Day 4.xml".
16. I left both the New loadout name and the Output PoB blank.
17. I clicked the 'Merge' button.
18. After the success message, I changed the PoB file to merge in to "Day 5.xml" and then clicked 'Merge' again.
19. Changed to "Day 6.xml" and clicked 'Merge'.
20. Changed to "Week 1.xml" and clicked 'Merge'.
21. Changed to "Week 2.xml" and clicked 'Merge'.
22. Changed to "Week 3.xml" and clicked 'Merge'.
23. Changed to "Week 4.xml" and clicked 'Merge'.
24. Changed to "Week 7.xml" and clicked 'Merge'.
25. Changed to "Week 10.xml" and clicked 'Merge'.
26. Changed to "Week 13.xml" and clicked 'Merge'.
27. Deleted the "Day ?" and "Week ??" saves from Path of Building.
28. Opened "Ben_Still_Clicking_MERC" in Path of Building. It now has the current snapshot as default and loadouts for all the other snapshots I had saved.

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
<a name="h11" />

If this project helped you, you can help me :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=XE5JR3FR458ZE&currency_code=USD)


Note for self on tagging:

git tag -a v1.0.1 -m "Release version 1.0.1"
git push origin v1.0.1