# Project Introduction — 2048 Infinity Merge

## 1. Project Name

**2048 Infinity Merge**

## 2. Overview

**2048 Infinity Merge** is an extended, cross-platform reimagination of the classic puzzle game **2048**. The player swipes (Up / Down / Left / Right) to slide all tiles on a square grid; whenever two tiles with the **same value** collide, they **merge** into a single tile whose value is **doubled**.

The core mechanic is built around **powers of 2** (a geometric progression with common ratio 2):

> 2 → 4 → 8 → 16 → 32 → 64 → 128 → 256 → 512 → 1024 → 2048 → 4096 → 8192 → ...

The goal of this project is not only to faithfully recreate the original 2048 experience, but also to expand it with **multiple game variants** — making the experience truly *"infinity"*, both in the way it is played and in the values that can be reached.

## 3. Project Goals

- Build a **cross-platform** application using **.NET MAUI Blazor Hybrid**, targeting Windows, Android, iOS and macOS from a single codebase.
- Faithfully reproduce the classic 2048 gameplay based on powers of 2.
- Extend the game with multiple **grid sizes** and multiple **game modes** to increase replayability.
- Provide a modern, smooth and responsive UI that supports both touch input (mobile) and keyboard input (desktop).
- Persist user progress and high scores **locally** on the device.

## 4. Gameplay

### 4.1. Core Rules

- The board is an `N x N` square grid of tiles.
- On each turn, the player chooses one direction: **Up / Down / Left / Right**.
- All tiles slide as far as possible in that direction until they hit either the wall or another tile.
- When two tiles with the **same value** collide, they **merge** into one tile with the **sum** of their values (i.e. double).
- After every valid move, a new tile (value `2` or `4`) spawns at a random empty cell.
- The game ends (**Game Over**) when the board is full and **no valid move** remains (no two adjacent tiles share the same value).

### 4.2. Scoring

- Each merge increases the score by the **value of the newly created tile**.
- High scores are stored **per game mode** and **per grid size** independently.

## 5. Variants & Game Modes

### 5.1. By Grid Size

- **Classic 4x4** — The original 2048 experience on a 4x4 grid.
- **5x5** — A larger 5x5 grid; more space and the chance to reach higher tile values.
- **6x6** — A 6x6 grid; high challenge in spatial management.
- *(Future versions may add 3x3, 7x7, 8x8, etc.)*

### 5.2. By Game Mode

- **Classic Mode** — Traditional, untimed mode. The player has unlimited time to think until no valid move is possible.
- **Time Mode (Time Attack)** — Time-limited mode. The player must score as high as possible (or reach a target tile) within a fixed amount of time.
- *(Future modes may include: **Move-limit Mode** — limited number of moves; **Zen Mode** — no Game Over; **Challenge Mode** — special objectives.)*

## 6. Target Audience

- Players who enjoy light, brain-training puzzle games.
- Existing 2048 fans who want a fresh experience beyond the standard 4x4 board.
- All ages — no fast reflexes required in Classic Mode.

## 7. Platform & Technology

- **Framework:** .NET MAUI Blazor Hybrid (.NET 10).
- **Languages:** C#, Razor, HTML, CSS, JavaScript.
- **Supported platforms:** Windows, Android, iOS, macOS (Mac Catalyst).
- **Storage:** Local storage on the device for settings and high scores.

## 8. Scope

- ✅ **In scope:** core 2048 gameplay, multiple grid sizes, Classic / Time game modes, local high-score persistence, cross-platform UI.
- ❌ **Out of scope (current version):** online multiplayer, global online leaderboards, user accounts, in-app purchases.
