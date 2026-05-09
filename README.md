# 2048 Infinity Merge

`2048 Infinity Merge` is a cross-platform puzzle game built with **.NET MAUI Blazor Hybrid**.  
It extends classic 2048 with multiple grid sizes and game modes while keeping all gameplay local on device.

## Overview

- Swipe or use arrow keys to move all tiles in one direction.
- Tiles with the same value merge into one tile with doubled value.
- New tiles spawn after valid moves.
- Game ends when no valid move remains.

## Planned Modes and Variants

- **Grid sizes:** `4x4`, `5x5`, `6x6`
- **Modes:** `Classic`, `Time`
- High score is tracked per `(mode, grid size)`.

## Tech Stack

- **Framework:** .NET MAUI Blazor Hybrid (.NET 10)
- **Languages:** C#, Razor, HTML, CSS
- **Platforms:** Windows, Android, iOS, macOS (Mac Catalyst)
- **Persistence:** local device storage for high scores, settings, and save game state

## Run

Clone from GitHub:
```bash
git clone <repo-url>
cd 2048-Infinity-Merge
```

From repository root:

```bash
dotnet build "2048 Infinity Merge/2048 Infinity Merge.App/2048 Infinity Merge.App.csproj" -f net10.0-windows10.0.19041.0
dotnet run "2048 Infinity Merge/2048 Infinity Merge.App/2048 Infinity Merge.App.csproj" -f net10.0-windows10.0.19041.0
```

For hot reload:

```bash
dotnet watch run --project "2048 Infinity Merge/2048 Infinity Merge.App/2048 Infinity Merge.App.csproj" -f net10.0-windows10.0.19041.0
```

## Scope (Current Version)

- In scope: core 2048 gameplay, local high scores/settings/save game state, cross-platform client.
- Out of scope: online multiplayer, cloud sync, global leaderboard, user accounts.

---

Detailed product and architecture docs are in:

- `Docs/project_introduction.md`
- `Docs/software_design.md`