# Future of the Project

Thank you all for your opinions and feedback, according to the majority, option 1 will be implemented.

![image](https://user-images.githubusercontent.com/32140579/184701764-c4a0ac49-053b-44f9-8e04-587808b6e355.png)
_____________________________________________________________________________________________________________________________
# HoYoStudio
Check out the [original AssetStudio project](https://github.com/Perfare/AssetStudio) for more information.

This is the release of `HoYoStudio`, Modded AssetStudio that should work on:
```
- Genshin Impact.
- Honkai Impact 3rd.
- Honkai: Star Rail.
- Tears of Themis.
```

Note: Requires Internet connection to fetch asset_index jsons.
_____________________________________________________________________________________________________________________________

Some features are:
```
- Change selected Game by using "Option -> Specify Game".
- Integration with "Radioegor146" repo to load asset_index through "Options -> Specify AI version".
- Exportable Assets (not all of them) with XOR/JSON support for "MiHoYoBinData"
- Togglable debug console.
- Container/filename recovery for Assets.
- Build Asset List of assets inside game files.
- Build CABMap/AssetMap through CLI.
- Integrated CLI Into the main executable, allowing for GUI/CLI while having a single exe.
```
_____________________________________________________________________________________________________________________________
How to use:

```
1. Build Map (Misc. -> Build Map).
2. Load files.
```

CLI Version:
```
AssetStudioGUI 0.16.60
Copyright c Perfare 2018-2022

  -v, --verbose           Show log messages.

  -t, --type              Specify unity type(s).

  -f, --filter            Specify regex filter(s).

  -g, --game              Specify Game.

  -m, --map               Build CABMap/AssetMap.

  --help                  Display this help screen.

  --version               Display version information.

  input_path (pos. 0)     Required. Input file/folder.

  output_path (pos. 1)    Required. Output folder.
```
_____________________________________________________________________________________________________________________________
NOTES:
```
- in case of any "MeshRenderer/SkinnedMeshRenderer" errors, make sure to enable "Disable Renderer" option in "Export Options" before loading assets.
- in case of need to export models/animators without fetching all animations, make sure to enable "Ignore Controller Anim" option in "Options -> Export Options" before loading assets.
```

Looking forward for feedback for issues/bugs to fix and update.
_____________________________________________________________________________________________________________________________
Special Thank to:
- Perfare: Original author.
- Khang06: [genshinblkstuff](https://github.com/khang06/genshinblkstuff) for blk/mhy0 extraction.
- Radioegor146: [gi-asset-indexes](https://github.com/radioegor146/gi-asset-indexes) for recovered/updated asset_index's.
- Ds5678: [AssetRipper](https://github.com/AssetRipper/AssetRipper)[[discord](https://discord.gg/XqXa53W2Yh) at `#genshin` channel] for information about Asset Formats & Parsing.
