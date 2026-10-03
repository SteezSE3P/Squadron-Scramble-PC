# Squadron Scramble — PC port

A PC (Windows / Linux / macOS) port of **Squadron Scramble**, the 1–8 player couch
dogfighting game by DepthCharge Software. It was released on Xbox Live Indie Games on
2 Jan 2013.

The original Xbox 360 package is `2A548A997D0E697947E55E6FA0DA9AA2C75D9EF858` in this
repo. It was an XNA 4.0 game. The port is the original game code, decompiled and rebuilt
on [MonoGame](https://monogame.net/) (DesktopGL). A thin compatibility layer replaces the
Xbox-only APIs.

## Running

Requirements: the [.NET 8 SDK](https://dotnet.microsoft.com/download) (or newer).

```sh
dotnet run --project src/SquadronScramble -c Release
# fullscreen:
dotnet run --project src/SquadronScramble -c Release -- --fullscreen
```

To make a standalone build you can copy to another PC (no .NET install needed):

```sh
dotnet publish src/SquadronScramble -c Release -r win-x64   --self-contained -o publish/win-x64
dotnet publish src/SquadronScramble -c Release -r linux-x64 --self-contained -o publish/linux-x64
dotnet publish src/SquadronScramble -c Release -r osx-x64   --self-contained -o publish/osx-x64
```

Then run `SquadronScramble.exe` (Windows) or `./SquadronScramble` (Linux/macOS) in that
folder. The GitHub Actions workflow in `.github/workflows/build.yml` builds the same three
packages and uploads them as artifacts.

## Controls

**Gamepads:** Xbox (XInput) and other SDL-supported controllers work just as on the Xbox,
for up to 4 controllers. Two pilots can share one pad, one on the left half and one on the
right half.

**Keyboard:** the keyboard acts as an extra controller. It takes the first controller slot
that has no gamepad plugged in. Two pilots can share it, like a pad:

| Action                         | Left pilot (left half of pad) | Right pilot (right half of pad) |
|--------------------------------|-------------------------------|---------------------------------|
| Move / turn / navigate menus   | `W` `A` `S` `D`               | Arrow keys                      |
| Fire (trigger)                 | `Left Shift`                  | `Right Ctrl`                    |
| Fire (bumper)                  | `Q`                           | `Right Shift`                   |

| Pad button | Key         |
|------------|-------------|
| A          | `Space`     |
| B          | `Escape`    |
| X          | `E`         |
| Y          | `Tab`       |
| Start      | `Enter`     |
| Back       | `Backspace` |

Other keys:

- `F11` or `Alt+Enter`: toggle fullscreen. The window can also be resized freely; the game
  is drawn at 1280x720 and letterboxed to fit.
- When renaming pilots, type the name, then press `Enter` to accept or `Esc` to cancel.
  With a gamepad, `A` accepts and `B` cancels.

## Save data

Squadron/pilot names and options are saved to:

- Windows: `%APPDATA%\SquadronScramble\`
- Linux / macOS: `~/.config/SquadronScramble/`

## What was changed for PC

The game logic is the original code from the Xbox build (`src/SquadronScramble/Game`).
Changes for PC:

- **`Compat/`** replaces the Xbox-only XNA APIs:
  - `GamerServices.cs`: `Guide` is always the full game (no trial mode). The Xbox
    on-screen keyboard and message boxes are emulated with an in-game overlay.
  - `Storage.cs`: `StorageDevice` and `StorageContainer` save to the user's
    application-data folder.
  - `GamePad.cs`: adds the keyboard as a virtual controller (see above).
  - `CaseInsensitiveContentManager.cs`: the game's asset names don't always match the
    file names' case, which matters on Linux and macOS.
  - `Video.cs`: stub. The Xbox build referenced `VideoPlayer` but never played a video.
- **`Game/Dogfight.cs`** (the `Game` class): the game renders to a fixed 1280x720 target
  that is scaled to the window. It adds a fullscreen toggle and `--fullscreen`, and drops
  the Xbox trial-mode timer.
- **`Game/SafeArea.cs`**: the screen border now defaults to the full screen area. The Xbox
  default was a TV-safe border, which you can still pick in Options.

## Content conversion

`src/SquadronScramble/Content` holds the game's assets, already converted for PC. You only
need the converter if you want to regenerate them from the Xbox package:

```sh
# needs ffmpeg on the PATH (used to decode the Xbox XMA2 audio)
dotnet run --project tools/XboxContentConverter -c Release -- \
    2A548A997D0E697947E55E6FA0DA9AA2C75D9EF858 src/SquadronScramble/Content
```

The converter:

1. extracts the files from the Xbox 360 STFS (`LIVE`) package (`--extract <dir>` also
   dumps the raw package contents);
2. decompresses the LZX (XMemCompress) XNB files;
3. byte-swaps texture data (the Xbox 360 is big-endian), for both color and DXT3 textures;
4. decodes the XMA2 sound effects with ffmpeg and re-encodes them as MS-ADPCM (or 16-bit
   PCM with `--pcm`, which is about 4x larger);
5. writes uncompressed Windows XNB files that MonoGame loads directly.

## Known limitations

- MonoGame needs a working audio output device. On a machine with no sound device at all,
  the game exits at startup with `NoAudioHardwareException`.
- Online/Xbox Live features did not exist in this game, so nothing is missing there.
