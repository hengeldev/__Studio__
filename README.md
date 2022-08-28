# HoYoStudio
Check out the [original AssetStudio project](https://github.com/Perfare/AssetStudio) for more information.
_____________________________________________________________________________________________________________________________
New Discord Server: 

Join [here](https://discord.gg/JAeB5jGdcn) for more discussions, questions, suggestions and feedback.
_____________________________________________________________________________________________________________________________

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
- Build AssetMap, An asset list of assets inside game files.
- Build CABMap/AssetMap through CLI (with supported `Unity Type` and `Regex` filters).
- Export assets through CLI (with supported `Unity Type` and `Regex` filters and grouping).
```
_____________________________________________________________________________________________________________________________
How to use:

```
1. Build Map (Misc. -> Build Map).
2. Load files.
```
_____________________________________________________________________________________________________________________________
CLI Version:
```
AssetStudioCLI 0.16.65
Copyright (C) 2022 AssetStudioCLI

  -v, --verbose           Show log messages.

  -t, --type              Specify unity type(s).

  -f, --filter            Specify regex filter(s).

  -g, --game              Required. Specify Game.

  -m, --map               Build CABMap/AssetMap.

  -x, --amtype            (Default: XML) AssetMap output type

                          Options:
                          XML
                          JSON

  -p, --group             (Default: 0) Specify how exported assets should be grouped.

                          Options:
                          0 (type name)
                          1 (container path)
                          2 (Source file name)

  -a, --noassetbundle     Exclude AssetBundle from AssetMap/Export

  -i, --noindexobject     Exclude IndexObject/MiHoYoBinData from AssetMap/Export

  -k, --xorkey            XOR key to decrypt MiHoYoBinData

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
_____________________________________________________________________________________________________________________________
Special Thank to:
- Perfare: Original author.
- Khang06: [genshinblkstuff](https://github.com/khang06/genshinblkstuff) for blk/mhy0 extraction.
- Radioegor146: [gi-asset-indexes](https://github.com/radioegor146/gi-asset-indexes) for recovered/updated asset_index's.
- Ds5678: [AssetRipper](https://github.com/AssetRipper/AssetRipper)[[discord](https://discord.gg/XqXa53W2Yh) at `#mihoyo` channel] for information about Asset Formats & Parsing.
