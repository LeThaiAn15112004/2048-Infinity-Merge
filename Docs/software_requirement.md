# Software Requirement — 2048 Infinity Merge

## Table of Contents

- [1. Context Diagram](#1-context-diagram)
- [2. Screen Flow](#2-screen-flow)
- [3. Use Case Diagram](#3-use-case-diagram)
- [4. Use Case Description](#4-use-case-description)
- [5. Business Rules](#5-business-rules)

---

## 1. Context Diagram

The system is a single-player offline puzzle game. It interacts with only one external actor (the **Player**) and one external resource (the device's **Local Storage**) for persisting settings and high scores.

```mermaid
flowchart LR
    Player["Player"]
    Storage["Local Storage<br/>(on device)"]
    System(("<b>2048 Infinity<br/>Merge</b>"))

    Player -- "swipe / tap / key input" --> System
    System -- "UI updates, score, animations" --> Player
    System -- "write settings, high scores" --> Storage
    Storage -- "read settings, high scores" --> System
```

> **Notation:** following the Yourdon / DeMarco convention — the **circle** in the center is the system (single process at context level), and the **rectangles** around it are external entities that interact with the system.

| # | Entity | Type | Description |
|---|--------|------|-------------|
| 1 | Player | External actor | The end user who plays the game. |
| 2 | 2048 Infinity Merge | System | The MAUI Blazor Hybrid application under development. |
| 3 | Local Storage | External resource | Device-local persistence for settings and high scores. |

---

## 2. Screen Flow

Main screens of the app and the navigation between them.

```mermaid
flowchart TD
    Home["Home<br/>(Main Menu)"]
    Mode["Mode Select<br/>(Classic / Time)"]
    Size["Grid Size Select<br/>(4x4 / 5x5 / 6x6)"]
    Game["Game Screen"]
    Pause["Pause Menu"]
    GameOver["Game Over"]
    Scores["High Scores"]
    Settings["Settings"]

    Home -->|Play| Mode
    Mode --> Size
    Size --> Game
    Game -->|Pause| Pause
    Pause -->|Resume| Game
    Pause -->|Quit| Home
    Game -->|No valid move| GameOver
    GameOver -->|Play Again| Game
    GameOver -->|Home| Home
    Home --> Scores
    Home --> Settings
    Scores --> Home
    Settings --> Home
```

---

## 3. Use Case Diagram

```mermaid
flowchart LR
    Player(("Player"))

    subgraph System["2048 Infinity Merge"]
        UC1(("UC-01<br/>Start New Game"))
        UC2(("UC-02<br/>Play Game<br/>(Make Move)"))
        UC3(("UC-03<br/>Pause / Resume"))
        UC4(("UC-04<br/>View High Scores"))
        UC5(("UC-05<br/>Reset High Scores"))
        UC6(("UC-06<br/>Adjust Settings"))
    end

    Player --- UC1
    Player --- UC2
    Player --- UC3
    Player --- UC4
    Player --- UC5
    Player --- UC6

    UC1 -. "&laquo;include&raquo;" .-> UC2
```

---

## 4. Use Case Description

### UC-01. Start New Game

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | The app is running and the Home screen is displayed. |
| **Main flow** | 1. Player taps **Play** on the Home screen.<br/>2. System shows the **Mode Select** screen.<br/>3. Player chooses a game mode (Classic / Time).<br/>4. System shows the **Grid Size Select** screen.<br/>5. Player chooses a grid size (4x4 / 5x5 / 6x6).<br/>6. System initializes the board with 2 starting tiles and navigates to the Game screen. |
| **Post-condition** | A new game session has started; UC-02 (Play Game) is active. |

### UC-02. Play Game (Make Move)

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | A game session is active and not paused. |
| **Main flow** | 1. Player performs a directional input (swipe / arrow key) — Up / Down / Left / Right.<br/>2. System slides all tiles in that direction.<br/>3. System merges any two adjacent tiles with the same value into a single doubled tile.<br/>4. System updates the score.<br/>5. System spawns one new tile (value `2` or `4`) at a random empty cell.<br/>6. System checks for **Game Over** — if no valid move remains, transitions to the Game Over screen. |
| **Alt flow** | If the input would not change the board (no slide and no merge possible in that direction), the move is **ignored** — no new tile is spawned and no score is added. |
| **Post-condition** | The board state and score are updated; the game continues or ends. |

### UC-03. Pause / Resume

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | A game session is active. |
| **Main flow** | 1. Player taps the **Pause** button on the Game screen.<br/>2. System suspends gameplay (in Time Mode, the timer is also paused) and shows the Pause menu.<br/>3. Player taps **Resume** to continue, or **Quit** to return to Home. |
| **Post-condition** | Gameplay continues from the same state, or the session is abandoned. |

### UC-04. View High Scores

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | The Home screen is displayed. |
| **Main flow** | 1. Player opens the **High Scores** screen.<br/>2. System loads high scores from local storage.<br/>3. System displays the best score for each combination of (game mode × grid size). |
| **Post-condition** | High scores are displayed to the player. |

### UC-05. Reset High Scores

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | The High Scores screen is displayed. |
| **Main flow** | 1. Player taps **Reset**.<br/>2. System asks for confirmation.<br/>3. On confirmation, system clears all high score records in local storage.<br/>4. The list refreshes to show empty / zero scores. |
| **Post-condition** | All high scores are erased from local storage. |

### UC-06. Adjust Settings

| Field | Description |
|-------|-------------|
| **Actor** | Player |
| **Pre-condition** | The Settings screen is displayed. |
| **Main flow** | 1. Player changes a setting (e.g. sound on/off, theme, default time for Time Mode).<br/>2. System validates and applies the change immediately.<br/>3. System persists the new setting to local storage. |
| **Post-condition** | The new settings are active and persisted across app restarts. |

---

## 5. Business Rules

| ID | Rule |
|----|------|
| **BR-01** | The board is a square grid of size `N x N` where `N ∈ {4, 5, 6}`. |
| **BR-02** | A new game starts with exactly **2 tiles** placed at random empty cells. |
| **BR-03** | Every tile value must be a **power of 2** (2, 4, 8, 16, …). New spawned tiles have value `2` or `4`. |
| **BR-04** | Two tiles can **merge** only if they have the **same value** and become adjacent during the same move; the result is one tile with value equal to the **sum** (i.e. doubled). |
| **BR-05** | Within a single move, **a tile can participate in at most one merge** (no chain merges in the same swipe). |
| **BR-06** | After every **valid** move, exactly **one** new tile (value `2` or `4`) is spawned at a random empty cell. |
| **BR-07** | A move is **invalid** (ignored) if it does not change the board — no slide and no merge happens. No new tile is spawned and no score is added. |
| **BR-08** | The score increases by the **value of the newly merged tile** for every merge that occurs in a move. |
| **BR-09** | **Game Over** occurs when the board is full **and** no two adjacent tiles share the same value (no possible merge in any direction). |
| **BR-10** | High scores are stored **per (game mode, grid size)** pair, independently. |
| **BR-11** | In **Time Mode**, the session ends when the timer reaches zero, even if valid moves remain. The final score at that moment is recorded. |
| **BR-12** | Pausing the game (UC-03) **also pauses the timer** in Time Mode. |
| **BR-13** | All persistent data (settings, high scores) is stored **only in local storage** on the device — no network or cloud sync in the current version. |
