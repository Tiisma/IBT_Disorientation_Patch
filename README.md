# IBT Disorientation Compatibility Patch

A small compatibility patch for **Immersive Body Temperature (IBT)** and **Disorientation** in Vintage Story. It prevents a crash associated with IBT's player-stats GUI when both mods are used together.

The patch is applied on the client. It skips IBT's `PlayerStatRenderer.UpdateFeltTemperature` method; the mod does not replace either mod's gameplay systems.

## Requirements

- Vintage Story
- Immersive Body Temperature
- Disorientation
- .NET 10 SDK (only required to build from source)
- A local Vintage Story installation containing `VintagestoryAPI.dll`, `Mods/VSEssentials.dll`, and `Lib/0Harmony.dll` (required to build)

## Install

1. Download the mod ZIP from the project's GitHub Releases.
2. Place the ZIP in your Vintage Story `Mods` directory.
3. Start the game with IBT and Disorientation installed.

## Build from source

On Windows, run `build.ps1` from PowerShell. The script creates a Release build and packages the DLL, `modinfo.json`, and `modicon.png` into a ZIP under `dist`.

Pass the path to your Vintage Story installation:

```powershell
.\build.ps1 -GamePath "C:\Path\To\VintageStory"
```

Or set `VINTAGE_STORY` and run the script without `-GamePath`:

```powershell
$env:VINTAGE_STORY = "C:\Path\To\VintageStory"
.\build.ps1
```

To build a Debug configuration instead:

```powershell
.\build.ps1 -GamePath "C:\Path\To\VintageStory" -Configuration Debug
```

The project can also be built directly with `dotnet build`; successful builds automatically create the mod ZIP in `dist`.

## Publish a GitHub release

Releases are published automatically when a version tag matching `v*` is pushed (for example, `v1.1.0`). The workflow builds the mod and attaches the ZIP from `dist` to a GitHub Release.

Because the build references Vintage Story DLLs that are not included in this repository, the workflow requires a Windows self-hosted GitHub Actions runner on a machine with Vintage Story installed. Register the runner for this repository with the `self-hosted`, `windows`, and `x64` labels, then define the repository Actions variable `VINTAGE_STORY` as the game's installation directory on that runner. The runner account must be able to read that directory.

Once the runner is online, publish a release by pushing a tag:

```powershell
git tag v1.1.0
git push origin v1.1.0
```

For later releases, update the version in both `modinfo.json` and the project file before creating and pushing the matching tag.

## License

No license file is currently included in this repository. All rights are reserved unless the author specifies otherwise.
