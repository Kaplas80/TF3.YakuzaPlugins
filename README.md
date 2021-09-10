# TF3 Yakuza Plugins [![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://choosealicense.com/licenses/mit/) ![Build and release](https://github.com/kaplas80/TF3.YakuzaPlugins/workflows/Build%20and%20release/badge.svg)

This repository contains plugins and scripts compatible with [TF3](https://github.com/kaplas80/TF3).

## Contents

### Common

- **TF3.YarhlPlugins.YakuzaCommon.dll**: Plugin for reading and writing common formats in Yakuza games.

### Yakuza Kiwami 2 PC

- **TF3.YarhlPlugins.YakuzaKiwami2.dll**: Plugin for reading and writing Yakuza Kiwami 2 ARMP archives.
- **TF3.Script.YakuzaKiwami2.json**: Script for extracting and repacking all the needed assets in `Yakuza Kiwami 2 (Steam version)` for translation.
- **TF3.Patch.YakuzaKiwami2VWF.1337**: Patch for the `Yakuza Kiwami 2 (Steam version)` executable to allow the use of a variable width font for extended latin characters.
- **FontSpacingEditor.exe**: App to edit the character spacing table in Yakuza Kiwami 2.

## Usage

### Plugins and script

1. Copy `TF3.YarhlPlugin.YakuzaCommon.dll`, `TF3.YarhlPlugin.YakuzaCommon.deps.json`, `TF3.YarhlPlugin.YakuzaKiwami2.dll` and `TF3.YarhlPlugin.YakuzaKiwami2.deps.json` to the `plugins` directory of TF3.
2. Copy `EPPlus.dll` and `DotNetZip.dll` to the root directory of TF3.
3. Copy the `TF3.Script.YakuzaKiwami2.json` to the `scripts` directory of TF3.

### VWF Patch

1. Create a `patches` directory in the translation directory.
2. Copy `TF3.Patch.YakuzaKiwami2VWF.1337` inside it.

For more information about patches, see TF3 documentation.

### FontSpacingEditor

See [README.md](https://github.com/Kaplas80/TF3.YakuzaPlugins/blob/8a6af2d550fcf19ae5e0c1b3091c03aca1f7f3f2/src/Apps/FontSpacingEditor/README.md)

## Credits

- Thanks to Pleonex for [Yarhl](https://scenegate.github.io/Yarhl/) and [PleOps.Cake](https://www.pleonex.dev/PleOps.Cake/).
- ARMP reader and writer is based on Capitan Retraso's [reARMP](https://github.com/CapitanRetraso/reARMP).
- Other libraries used: [DotNetZip](https://github.com/haf/DotNetZip.Semverd), [EPPlus](https://epplussoftware.com/).
- Icon by [Vecteezy](https://www.vecteezy.com/free-vector/emblem).