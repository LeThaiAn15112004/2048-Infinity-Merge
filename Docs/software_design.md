# Software Design Specification (SDS) — 2048 Infinity Merge

## Table of Contents

- [1. Introduction](#1-introduction)
- [2. Software Architecture](#2-software-architecture)
  - [2.1. Architectural Style](#21-architectural-style)
  - [2.2. High-Level Architecture Diagram](#22-high-level-architecture-diagram)
  - [2.3. Technology Stack](#23-technology-stack)
  - [2.4. Deployment View](#24-deployment-view)
- [3. Package Diagram](#3-package-diagram)
  - [3.1. Frontend (Presentation Layer)](#31-frontend-presentation-layer)
  - [3.2. Backend (Application & Domain Layer)](#32-backend-application--domain-layer)
  - [3.3. Combined Package Diagram](#33-combined-package-diagram)
- [4. Data Design](#4-data-design)
  - [4.1. In-Memory Domain Model](#41-in-memory-domain-model)
  - [4.2. Persistence (No Database)](#42-persistence-no-database)
- [5. Detailed Design](#5-detailed-design)
  - [5.1. Sequence — Start New Game (UC-01)](#51-sequence--start-new-game-uc-01)
  - [5.2. Sequence — Make Move (UC-02)](#52-sequence--make-move-uc-02)
  - [5.3. Activity — Slide & Merge Algorithm](#53-activity--slide--merge-algorithm)
  - [5.4. Sequence — Save High Score (Game Over)](#54-sequence--save-high-score-game-over)
- [6. Class Specification](#6-class-specification)

---

## 1. Introduction

### 1.1. Purpose

This document describes the **internal software design** of the **2048 Infinity Merge** application: how the system is decomposed into layers, packages, and classes, and how the major use cases are implemented.

### 1.2. Scope

The design covers the entire application — there is **no separate backend service** in the current version (see §2.4). All gameplay logic and persistence run on the client device.

### 1.3. References

- `project_introduction.md` — overall project goals and gameplay description.
- `software_requirement.md` — Use Cases (UC-01…UC-05) and Business Rules (BR-01…BR-13) that this design implements.

---

## 2. Software Architecture

### 2.1. Architectural Style

The application follows a **layered (Clean-Architecture-inspired)** style, combined with a lightweight **MVVM-like** pattern inside the Razor UI:

| Layer | Responsibility | Depends on |
|-------|----------------|-----------|
| **Presentation** *(FE)* | Razor components, layouts, CSS, JS interop. Renders state and dispatches user actions. | Application |
| **Application** *(BE)* | Use-case orchestration, services (`GameSessionService`, `HighScoreService`, `SettingsService`). Holds in-memory state. | Domain, Infrastructure |
| **Domain** *(BE)* | Pure game rules: `Board`, `Tile`, `GameEngine`, `MoveResult`, enums (`GameMode`, `GridSize`, `Direction`). No I/O. | — |
| **Infrastructure** *(BE)* | Adapters to platform: `IStorage` implementation backed by **MAUI `Preferences`**, timer wrapper for Time Mode, RNG wrapper. | Domain |

> **Note on "FE" vs "BE":** because the app is a single MAUI Blazor Hybrid client (no network split), *FE* and *BE* in this document refer to **logical layers inside the same process**, not separate deployable services.

### 2.2. High-Level Architecture Diagram

```mermaid
flowchart TB
    subgraph Presentation["Presentation Layer (FE) — Razor / MAUI Blazor"]
        UI_Pages["Pages<br/>(Home, Game, Help, Settings)"]
        UI_Comp["Components<br/>(Board, Tile, ModeCard, PauseModal, BannerAds)"]
        UI_Layout["Layouts & CSS<br/>(MainLayout, app.css, scoped CSS)"]
    end

    subgraph Application["Application Layer (BE)"]
        AppSvc["Services<br/>(GameSessionService, HighScoreService, SettingsService)"]
    end

    subgraph Domain["Domain Layer (BE)"]
        DomainCore["Game Engine<br/>(GameEngine, Board, Tile, MoveResult, enums)"]
    end

    subgraph Infrastructure["Infrastructure Layer (BE)"]
        Infra["Adapters<br/>(IStorage → MAUI Preferences,<br/>ITimer, IRandom)"]
    end

    Presentation --> Application
    Application --> Domain
    Application --> Infrastructure
    Infrastructure --> Domain
```

### 2.3. Technology Stack

| Concern | Choice |
|---------|--------|
| Application framework | **.NET MAUI Blazor Hybrid** (.NET 10) |
| UI | **Razor** components, **CSS** (in `wwwroot/css/`), minimal **JavaScript** for input handling (touch swipe, key events) |
| State management | Plain C# services registered as **singletons / scoped** in MAUI `MauiProgram` DI container |
| Persistence | **`Microsoft.Maui.Storage.Preferences`** (key/value) — see §4.2 |
| Telemetry (dev only) | **.NET Aspire** (`AppHost` + `ServiceDefaults`) — OpenTelemetry instrumentation during local development; not shipped in release builds. |
| Target platforms | Windows, Android, iOS, macOS (Mac Catalyst) |

### 2.4. Deployment View

```mermaid
flowchart LR
    subgraph Device["End-user Device (Windows / Android / iOS / macOS)"]
        App["InfinityMergeApp<br/>(MAUI Blazor Hybrid binary)"]
        Prefs[("MAUI Preferences<br/>(key-value local store)")]
        App <--> Prefs
    end
    Player(("Player")) -- swipe / tap / keys --> App
```

- The application is a **single self-contained binary** per platform.
- **No network calls**, **no server**, **no cloud sync**.
- The `AppHost` and `ServiceDefaults` projects are used **only at development time** for orchestration and telemetry; they are **not deployed** to end users.

---

## 3. Package Diagram

> The "FE / BE" split here is logical (see §2.1). All packages live under the same `InfinityMergeApp` assembly.

### 3.1. Frontend (Presentation Layer)

```mermaid
flowchart TB
    subgraph FE["📦 InfinityMergeApp.Components (FE)"]
        Pages["📦 Pages<br/>• Home.razor<br/>• Game.razor<br/>• Help.razor<br/>• Settings.razor"]
        Items["📦 Items<br/>• Board.razor<br/>• Tile.razor<br/>• ModeCard.razor<br/>• PauseModal.razor<br/>• GameOverModal.razor<br/>• BannerAds.razor"]
        Layout["📦 Layout<br/>• MainLayout.razor<br/>• Routes.razor"]
        WwwRoot["📦 wwwroot<br/>• app.css<br/>• css/*.css<br/>• js/input-handler.js<br/>• fonts/"]
    end

    Pages --> Items
    Pages --> Layout
    Pages -. "use CSS" .-> WwwRoot
    Items -. "use CSS" .-> WwwRoot
```

### 3.2. Backend (Application & Domain Layer)

```mermaid
flowchart TB
    subgraph BE["📦 InfinityMergeApp.Core (BE — logical)"]
        AppPkg["📦 Application<br/>• GameSessionService<br/>• HighScoreService<br/>• SettingsService"]
        DomainPkg["📦 Domain<br/>• GameEngine<br/>• Board, Tile<br/>• MoveResult<br/>• GameMode, GridSize, Direction (enums)"]
        InfraPkg["📦 Infrastructure<br/>• IStorage / PreferencesStorage<br/>• ITimer / MauiTimer<br/>• IRandom / SystemRandom"]
    end

    AppPkg --> DomainPkg
    AppPkg --> InfraPkg
    InfraPkg --> DomainPkg
```

### 3.3. Combined Package Diagram

```mermaid
flowchart TB
    subgraph App["InfinityMergeApp"]
        FE["📦 Components (FE)<br/>Pages • Items • Layout • wwwroot"]
        AppL["📦 Application<br/>GameSessionService • HighScoreService • SettingsService"]
        Dom["📦 Domain<br/>GameEngine • Board • Tile • MoveResult • Enums"]
        Inf["📦 Infrastructure<br/>PreferencesStorage • Timer • Random"]
    end
    FE --> AppL
    AppL --> Dom
    AppL --> Inf
    Inf --> Dom
```

---

## 4. Data Design

> The application **does not use a database** (no SQLite, no remote DB) in the current version. All data is either **transient (in-memory)** or persisted as **key-value pairs** in MAUI `Preferences`.

### 4.1. In-Memory Domain Model

| Entity | Kind | Key Fields | Notes |
|--------|------|-----------|-------|
| `Tile` | record struct | `Value : int`, `Id : Guid` | Value is always a power of 2 (BR-03). `Id` enables UI animation tracking across moves. |
| `Board` | class | `Size : int`, `Cells : Tile?[,]` | `null` cell = empty. |
| `MoveResult` | record | `Moved : bool`, `Merges : List<MergeInfo>`, `Score : int`, `IsGameOver : bool` | Returned by `GameEngine.Move(...)` (BR-07, BR-08). |
| `GameState` | class | `Board`, `Mode : GameMode`, `Score : int`, `RemainingTime : TimeSpan?`, `IsPaused : bool` | Live state of the current session. `RemainingTime` only used for Time Mode. |
| `HighScoreKey` | record | `Mode : GameMode`, `Size : GridSize` | Used to look up high scores per (mode × size) — BR-10. |

Enums: `GameMode { Classic, Time }`, `GridSize { S4 = 4, S5 = 5, S6 = 6 }`, `Direction { Up, Down, Left, Right }`.

### 4.2. Persistence (No Database)

All persistent data is stored via **`Microsoft.Maui.Storage.Preferences`** — a thin OS-backed key/value store (`NSUserDefaults` on iOS/macOS, `SharedPreferences` on Android, registry on Windows).

**Key schema:**

| Key | Value type | Description |
|-----|-----------|-------------|
| `highscore.classic.4` | `int` | High score for Classic 4x4. |
| `highscore.classic.5` | `int` | High score for Classic 5x5. |
| `highscore.classic.6` | `int` | High score for Classic 6x6. |
| `highscore.time.4` | `int` | High score for Time 4x4. |
| `highscore.time.5` | `int` | High score for Time 4x4. |
| `highscore.time.6` | `int` | High score for Time 6x6. |
| `settings.sound` | `bool` | Sound on/off. |
| `settings.theme` | `string` | Selected theme ID. |
| `settings.timeMode.duration` | `int` (seconds) | Default round duration for Time Mode. |

**Why no DB:**
- Total data footprint is < 1 KB and only key-value access pattern.
- `Preferences` is built into MAUI on every target platform — no extra dependency, no schema migration.
- If richer needs arise later (replays, achievements), the `IStorage` interface can be re-implemented with **SQLite** (`sqlite-net-pcl`) without touching the Application or Domain layer.

---

## 5. Detailed Design

### 5.1. Sequence — Start New Game (UC-01)

```mermaid
sequenceDiagram
    actor P as Player
    participant Home as Home.razor
    participant Sess as GameSessionService
    participant HS as HighScoreService
    participant Eng as GameEngine
    participant Game as Game.razor

    P->>Home: tap a Mode card (e.g. Classic 4x4)
    Home->>HS: GetHighScore(mode, size)
    HS-->>Home: int (rendered on each card)
    P->>Home: select card → navigate
    Home->>Sess: StartNewGame(mode, size)
    Sess->>Eng: CreateBoard(size) + spawn 2 random tiles
    Eng-->>Sess: GameState
    Sess-->>Game: navigate("/game")
    Game->>Sess: Subscribe(StateChanged)
    Game-->>P: render board
```

### 5.2. Sequence — Make Move (UC-02)

```mermaid
sequenceDiagram
    actor P as Player
    participant Game as Game.razor
    participant Sess as GameSessionService
    participant Eng as GameEngine

    P->>Game: swipe / arrow key
    Game->>Sess: Move(direction)
    Sess->>Eng: Move(state.Board, direction)
    Eng-->>Sess: MoveResult { Moved, Merges, Score, IsGameOver }

    alt Moved == false (BR-07)
        Sess-->>Game: ignore (no state change)
    else Moved == true
        Sess->>Eng: SpawnRandomTile(board)
        Eng-->>Sess: updated Board
        Sess->>Sess: state.Score += result.Score
        Sess-->>Game: StateChanged event
        Game-->>P: animate slides & merges
        opt result.IsGameOver
            Sess->>HighScoreService: SaveIfBetter(mode, size, score)
            Sess-->>Game: navigate to GameOver modal
        end
    end
```

### 5.3. Activity — Slide & Merge Algorithm

The same routine works for any of the 4 directions by **rotating** the board so the move direction always points "left", running a 1-D pass on each row, then rotating back.

```mermaid
flowchart TD
    Start([Move direction d]) --> Rot[Rotate board so d → 'Left']
    Rot --> ForRow[For each row]
    ForRow --> Compact[Compact: drop nulls, keep order]
    Compact --> Pass[Walk left → right:<br/>if a[i] == a[i+1] →<br/>merge into 2*a[i],<br/>add to score,<br/>mark a[i] as 'merged this turn']
    Pass --> Note[BR-05: a tile that was<br/>just merged cannot merge again<br/>in the same move]
    Note --> Pad[Pad row with nulls on the right]
    Pad --> NextRow{More rows?}
    NextRow -- yes --> ForRow
    NextRow -- no --> RotBack[Rotate board back to original orientation]
    RotBack --> Compare{Did the board change?}
    Compare -- no --> Invalid[Return MoveResult { Moved=false } — BR-07]
    Compare -- yes --> Spawn[Spawn one tile<br/>(value 2 or 4)]
    Spawn --> CheckOver{Any valid move left?}
    CheckOver -- no --> Over[IsGameOver = true — BR-09]
    CheckOver -- yes --> Done[Return MoveResult]
    Over --> Done
    Invalid --> Done
    Done([End])
```

### 5.4. Sequence — Save High Score (Game Over)

```mermaid
sequenceDiagram
    participant Sess as GameSessionService
    participant HS as HighScoreService
    participant St as IStorage<br/>(PreferencesStorage)

    Sess->>HS: SaveIfBetter(mode, size, score)
    HS->>St: Get<int>($"highscore.{mode}.{size}")
    St-->>HS: previous (or 0)
    alt score > previous
        HS->>St: Set($"highscore.{mode}.{size}", score)
        St-->>HS: ok
    else
        HS-->>Sess: no-op
    end
```

---

## 6. Class Specification

> Only the non-trivial classes are detailed below. Razor components have UI-only logic and are described by their `.razor` files; only their **public bindable properties** are listed.

### 6.1. `GameEngine` (Domain)

| Aspect | Detail |
|--------|--------|
| **Responsibility** | Pure game rules: slide, merge, spawn, game-over detection. **No state, no I/O.** |
| **Public methods** | `Board CreateBoard(GridSize size)`<br/>`Board SpawnRandomTile(Board board, IRandom rng)`<br/>`MoveResult Move(Board board, Direction direction)`<br/>`bool HasAnyValidMove(Board board)` |
| **Implements** | BR-01, BR-02, BR-03, BR-04, BR-05, BR-06, BR-07, BR-08, BR-09 |
| **Notes** | Stateless and side-effect-free → trivially unit-testable. |

### 6.2. `Board` (Domain)

| Aspect | Detail |
|--------|--------|
| **Properties** | `int Size { get; }`<br/>`Tile?[,] Cells { get; }` |
| **Methods** | `Tile? this[int r, int c]`<br/>`bool IsFull()`<br/>`IEnumerable<(int r, int c)> EmptyCells()`<br/>`Board Clone()` |

### 6.3. `Tile` (Domain)

| Aspect | Detail |
|--------|--------|
| **Kind** | `record struct` |
| **Fields** | `int Value` (power of 2), `Guid Id` (animation tracking) |

### 6.4. `MoveResult` (Domain)

| Aspect | Detail |
|--------|--------|
| **Kind** | `record` |
| **Fields** | `bool Moved`<br/>`IReadOnlyList<MergeInfo> Merges`<br/>`int Score` (delta gained this move)<br/>`bool IsGameOver` |

### 6.5. `GameSessionService` (Application)

| Aspect | Detail |
|--------|--------|
| **Responsibility** | Owns the live `GameState`, drives the game loop, raises `StateChanged` for the UI. Coordinates `GameEngine`, `ITimer`, `HighScoreService`. |
| **Lifetime** | Singleton (registered in `MauiProgram`). |
| **Public API** | `event Action StateChanged`<br/>`GameState Current { get; }`<br/>`void StartNewGame(GameMode mode, GridSize size)`<br/>`void Move(Direction direction)`<br/>`void Pause()` / `void Resume()`<br/>`void QuitToHome()` |
| **Implements** | UC-01, UC-02, UC-03 |

### 6.6. `HighScoreService` (Application)

| Aspect | Detail |
|--------|--------|
| **Responsibility** | Read & write high scores. |
| **Public API** | `int GetHighScore(GameMode mode, GridSize size)`<br/>`void SaveIfBetter(GameMode mode, GridSize size, int score)` |
| **Depends on** | `IStorage` |
| **Implements** | BR-10 |

### 6.7. `SettingsService` (Application)

| Aspect | Detail |
|--------|--------|
| **Responsibility** | Read & write user settings. |
| **Public API** | `bool SoundEnabled { get; set; }`<br/>`string Theme { get; set; }`<br/>`TimeSpan TimeModeDuration { get; set; }`<br/>`event Action SettingsChanged` |
| **Depends on** | `IStorage` |
| **Implements** | UC-04 |

### 6.8. `IStorage` & `PreferencesStorage` (Infrastructure)

| Aspect | Detail |
|--------|--------|
| **Interface** | `T? Get<T>(string key)`<br/>`void Set<T>(string key, T value)`<br/>`void Remove(string key)` |
| **Implementation** | `PreferencesStorage` — wraps `Microsoft.Maui.Storage.Preferences`. |
| **Notes** | Abstracted so that a future SQLite-backed implementation can swap in transparently. |

### 6.9. `ITimer` & `MauiTimer` (Infrastructure)

| Aspect | Detail |
|--------|--------|
| **Interface** | `void Start(TimeSpan duration)` / `void Pause()` / `void Resume()` / `void Stop()`<br/>`event Action<TimeSpan> Tick`<br/>`event Action Elapsed` |
| **Used by** | `GameSessionService` for **Time Mode** countdown. Pausing the game also pauses the timer (BR-12). |

### 6.10. UI Components (Presentation)

| Component | Purpose | Key bindings |
|-----------|---------|--------------|
| `Home.razor` | Renders the 6 mode cards. | Reads `HighScoreService` for each card. |
| `ModeCard.razor` | One card on Home. | `[Parameter] GameMode Mode`, `[Parameter] GridSize Size`, `[Parameter] int HighScore`. |
| `Game.razor` | Hosts `Board`, score, timer (if Time Mode), Pause button. | Subscribes to `GameSessionService.StateChanged`. |
| `Board.razor` | Renders the grid of tiles + animations. | `[Parameter] Board Board`, `[Parameter] EventCallback<Direction> OnMove`. |
| `Tile.razor` | Renders one tile with color/value. | `[Parameter] Tile Tile`. |
| `PauseModal.razor` | Modal overlay shown when paused (UC-03). | `[Parameter] EventCallback OnResume`, `[Parameter] EventCallback OnQuit`. |
| `GameOverModal.razor` | Modal shown after Game Over. | `[Parameter] int FinalScore`, `[Parameter] int HighScore`, callbacks for *Play Again* / *Home*. |
| `Help.razor` | Static gameplay instructions (UC-05). | — |
| `Settings.razor` | Read & write settings (UC-04). | Two-way bound to `SettingsService`. |
| `BannerAds.razor` | Optional in-app banner (in-scope per project intro). | — |
