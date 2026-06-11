# Project Introduction - 2048 Infinity Merge

## Table of Contents

- [1. Project Name](#1-project-name)
- [2. Overview](#2-overview)
- [3. Project Goals](#3-project-goals)
- [4. Gameplay](#4-gameplay)
  - [4.1. Core Rules](#41-core-rules)
  - [4.2. Scoring](#42-scoring)
- [5. Variants and Game Modes](#5-variants-and-game-modes)
  - [5.1. By Grid Size](#51-by-grid-size)
  - [5.2. By Game Mode](#52-by-game-mode)
- [6. Target Audience](#6-target-audience)
- [7. Platform and Technology](#7-platform-and-technology)
- [8. Scope](#8-scope)

## 1. Project Name

**2048 Infinity Merge**

## 2. Overview

**2048 Infinity Merge** is an extended mobile version of the classic puzzle game **2048**. The player swipes **Up / Down / Left / Right** to slide all tiles on a square grid. Whenever two tiles with the same value collide, they merge into a single tile whose value is doubled.

The core mechanic is built around powers of 2:

> 2 -> 4 -> 8 -> 16 -> 32 -> 64 -> 128 -> 256 -> 512 -> 1024 -> 2048 -> 4096 -> ...

The project recreates the familiar 2048 loop while expanding it with multiple grid sizes and game modes. The "Infinity" idea comes from replayability, higher reachable tile values, and future room for new challenges.

## 3. Project Goals

- Build a cross-platform mobile application using **React Native** and **TypeScript**.
- Target **Android** and **iOS** from a shared codebase.
- Faithfully reproduce the classic 2048 gameplay based on powers of 2.
- Extend the game with multiple grid sizes and multiple game modes.
- Provide a smooth touch-first UI with responsive layouts, animations, safe-area support, and clear mobile interactions.
- Persist user data locally on the device, including high scores, settings, and resumable game state.
- Keep the gameplay rules separated from React Native UI code so the game engine can be tested independently.

## 4. Gameplay

### 4.1. Core Rules

- The board is an `N x N` square grid of tiles.
- On each turn, the player chooses one direction: **Up / Down / Left / Right**.
- All tiles slide as far as possible in that direction until they hit either the wall or another tile.
- When two tiles with the same value collide, they merge into one tile with the sum of their values.
- After every valid move, a new tile with value `2` or `4` spawns at a random empty cell.
- The game ends when the board is full and no valid move remains.

### 4.2. Scoring

- Each merge increases the score by the value of the newly created tile.
- High scores are stored independently per game mode and grid size.

## 5. Variants and Game Modes

### 5.1. By Grid Size

- **Classic 4x4** - The original 2048 board size.
- **5x5** - A larger board with more room and higher potential scores.
- **6x6** - A larger board focused on long-term spatial planning.
- Future versions may add 3x3, 7x7, 8x8, or special challenge boards.

### 5.2. By Game Mode

- **Classic Mode** - Traditional untimed mode. The player continues until no valid move remains.
- **Time Mode** - Time-limited mode. The player tries to score as high as possible before the countdown reaches zero.
- Future modes may include move-limit mode, zen mode, challenge mode, or daily puzzles.

## 6. Target Audience

- Players who enjoy light puzzle and brain-training games.
- Existing 2048 fans who want more variety than the standard 4x4 board.
- Mobile players who prefer short, replayable sessions.
- All ages; the game does not require fast reflexes in Classic Mode.

## 7. Platform and Technology

- **Framework:** React Native.
- **Language:** TypeScript.
- **UI:** React Native components, StyleSheet, safe-area handling, touch gestures, and animation APIs.
- **Supported platforms:** Android and iOS.
- **Storage:** Device-local key/value storage through a React Native storage adapter such as AsyncStorage or MMKV.
- **Testing:** Jest and React Native Testing Library for game logic and UI behavior.

## 8. Scope

- **In scope:** core 2048 gameplay, 4x4 / 5x5 / 6x6 grids, Classic / Time modes, local high-score persistence, settings, pause/resume, mobile UI, and optional ads / remove-ads purchase flow.
- **Out of scope for the current version:** online multiplayer, global leaderboards, user accounts, cloud sync, and server-side gameplay.
