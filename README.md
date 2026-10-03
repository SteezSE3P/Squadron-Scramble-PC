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

When the game starts you get a menu: **Local Game**, **Host Online Game**,
**Join Online Game** or **Quit**. To skip it, start with `--local`.

## Online play

Players on different PCs can play together over a LAN or the internet.

1. One player picks **Host Online Game**. The lobby shows the IP addresses the others
   should use.
2. Everyone else picks **Join Online Game** and enters the host's address, for example
   `192.168.1.20`. Add `:port` if the host isn't using the default port 24642.
3. When everyone is in the lobby, the host presses `Enter` to start.

From there it plays like the console version, with the menus, player select and matches
shared between all PCs.

- **Controllers:** the game has 4 controller slots, shared out between the PCs in the
  order they joined. Each PC gets one slot per gamepad plugged into it, or one slot for
  the keyboard if it has no gamepad. As on the Xbox, two pilots can share a slot (left and
  right half of the pad), so up to 8 people can play. The lobby shows who has which
  controller number.
- **Internet play:** the host has to forward TCP port 24642 on their router to their PC
  and give the others their public IP address. A virtual LAN such as Tailscale, ZeroTier
  or Hamachi also works, with no port forwarding needed.
- **Input delay:** the host can change it in the lobby with `Left`/`Right` (default 4
  frames, about 67 ms). Raise it if the game stutters or keeps showing "Waiting for...".
  Lower it on a fast LAN for snappier controls.
- **Saves:** everyone uses the host's pilot names and options. Changes made during the
  game are saved on the host's PC only.
- **Same build:** all players need the same version of the game.

Command-line shortcuts:

```sh
SquadronScramble --host [port]            # host straight away
SquadronScramble --join 192.168.1.20      # join straight away (host[:port])
SquadronScramble --name Alice --delay 6   # lobby name / input delay (host)
```

How it works: every PC runs the whole game, and only controller input goes over the
network ("deterministic lockstep"). Each frame, every PC sends its controllers' state to
the host. The host combines the input from all PCs and sends it back, and each PC
simulates the frame only once it has that combined input. All PCs start with the same
random seed and the host's save data, so the games stay identical. The PCs compare a
checksum every second and show a warning if they ever drift apart.

- If a player leaves, their controllers count as unplugged and the game carries on.
- If the host leaves, the game ends for everyone.
- Choosing **Exit** from the main menu closes the game on every PC.

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
  - `Net/`: online play. It contains the start-up menu and lobby (`NetLauncher.cs`), the
    session and lockstep loop (`NetSession.cs`), and the wire format (`NetProtocol.cs`).
- **`Game/Dogfight.cs`** (the `Game` class): the game renders to a fixed 1280x720 target
  that is scaled to the window. It adds a fullscreen toggle and `--fullscreen`, drops
  the Xbox trial-mode timer, shows the start-up menu, and runs the lockstep loop in
  online games.
- **`Game/General.cs`**: the random number generator can be seeded, so all PCs in an
  online game generate the same numbers.
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
- Online play assumes every PC computes the game identically. That holds for the same
  build on x64 PCs. Mixing a PC with an Apple Silicon (ARM) Mac has not been tested and
  could drift out of sync; the game warns if that happens.
