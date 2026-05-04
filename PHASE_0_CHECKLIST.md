# Phase 0 — Pre-Production Checklist

> Goal: Define everything needed before writing a single line of Unity code.
> All decisions here feed directly into Phase 1 implementation.

---

## 1. Tier Structure

- [ ] Confirm full tier order: **Steam → LV → MV → HV → EV → IV**
- [ ] Define what "unlocks" each tier (research node? milestone submission?)
- [ ] Define recipe step count target per tier:
  - Steam: 2–3 steps
  - LV: 3–5 steps
  - MV: 4–6 steps
  - HV: 6–8 steps
  - EV–IV: 8–12+ steps

---

## 2. Steam Tier — Design

### 2a. Resources
- [ ] Confirm Steam tier ore nodes: **Copper, Tin, Coal**
- [ ] Add a Water source mechanic (Water Pump? Fixed water node? Infinite?)
- [ ] Decide if Stone is a Steam tier resource (used for machine casings)

### 2b. Steam System
- [ ] Define the Boiler machine: Coal + Water → Steam (at a rate of X steam/sec)
- [ ] Decide Steam distribution: pipes (same as items) or a separate Steam pipe layer
- [ ] Define Steam pressure / capacity concept (does steam back up and waste if machines are full?)
- [ ] Define what happens when a steam machine runs without steam (halts? breaks?)

### 2c. Steam Tier Machines (target: 4–6 machines)
- [ ] **Steam Boiler** — Coal + Water → Steam (power source for the tier)
- [ ] **Steam Furnace** — Ore → Ingot (1:1 smelting, basic)
- [ ] **Steam Macerator** — Ore → 2x Dust (doubles ore yield, key early unlock)
- [ ] **Steam Alloy Smelter** — 2x Ingot → Alloy Ingot (makes Bronze from Copper + Tin)
- [ ] **Steam Extractor** — placed on resource node, pulls ore into pipe network
- [ ] *(Optional)* **Steam Compressor** — Items → Compressed items (plates, etc.)
- For each machine define:
  - [ ] Function (inputs → outputs)
  - [ ] Steam consumption rate (steam/tick)
  - [ ] Processing time (seconds per recipe)
  - [ ] Tile size (1×1 or 2×2)
  - [ ] Input port direction(s) and output port direction(s)

### 2d. Steam Tier Recipe Chains
- [ ] **Bronze chain**: Copper Ore → Copper Ingot → | + Tin Ore → Tin Ingot → Bronze Ingot (via Alloy Smelter)
- [ ] **Ore doubling chain**: Copper Ore → (Macerator) → 2x Copper Dust → (Furnace) → 2x Copper Ingot
- [ ] **Coal usage**: Coal → Boiler fuel (direct input, not processed)
- [ ] Write all Steam tier recipes in a spreadsheet with: input items, input counts, output items, output counts, machine, time, steam cost
- [ ] Verify recipe graph has no circular dependencies

---

## 3. LV Tier — Design

### 3a. Resources (new nodes unlocked at LV)
- [ ] Confirm LV introduces: **Iron**
- [ ] Decide if Redstone is an LV node
- [ ] Decide if Gold is an LV node
- [ ] Decide if Stone/Sand becomes available as a node or crafted from existing ores
- [ ] Total LV node slots on floor (including Steam tier nodes carried over)

### 3b. Power System — LV
- [ ] Define EU/t (energy units per tick) as the power unit
- [ ] Define LV voltage ceiling (e.g. 32 EU/t max per cable)
- [ ] Define the LV Generator machine: Coal/fuel → 8 EU/t (example)
- [ ] Define cable: item that connects generator to machines (1 tile wide, limited range?)
- [ ] Define what happens on overvolt (machine damage / explosion / just shuts off — pick one)
- [ ] Define Transformer concept for later tiers (not needed at LV, but spec it now)

### 3c. LV Tier Machines (target: 6–10 machines)
- [ ] **LV Furnace** — Ore/Dust → Ingot (faster than Steam, runs on EU)
- [ ] **LV Macerator** — Ore → 2x Dust
- [ ] **LV Alloy Smelter** — 2x input → Alloy output
- [ ] **LV Electrolyzer** — Compound → Elements (e.g. Water → Hydrogen + Oxygen)
- [ ] **LV Assembler** — Multiple components → Assembled item (circuits, parts)
- [ ] **LV Extractor** — Improved extractor for LV nodes (higher rate than Steam Extractor)
- [ ] **LV Generator** — Fuel → EU/t (coal, charcoal, etc.)
- [ ] *(Optional)* **LV Wiremill** — Ingot → Wire (needed for circuits)
- [ ] *(Optional)* **LV Cutter** — Ingot → Plate (alternative to compressor)
- For each machine define the same specs as Steam tier above

### 3d. LV Tier Recipe Chains
- [ ] **Steel chain**: Iron Ore → Iron Dust (Macerator) → Iron Ingot (Furnace) → Steel (Alloy Smelter + Carbon?)
- [ ] **Circuit chain**: Copper Wire (Wiremill) + Iron Plate (Cutter) + Redstone → Basic Circuit (Assembler)
- [ ] Write all LV recipes in the spreadsheet (same format as Steam tier)
- [ ] Confirm every LV machine has at least 3 useful recipes

---

## 4. Recipe Chain Spreadsheet

- [ ] Create `recipes.csv` or `recipes.md` with columns:
  - Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | EU/t or Steam/t
- [ ] Steam tier: all recipes filled in
- [ ] LV tier: all recipes filled in
- [ ] MV tier: at least 50% of recipes outlined
- [ ] Verify no recipe is a dead end (every output is used somewhere)
- [ ] Identify the "critical path" — the chain a new player will build first
- [ ] Identify 3 items that require the widest machine variety to produce (these become tier goal markers)

---

## 5. Research Tree Design

### 5a. Structure
- [ ] Decide tree layout: flat list of nodes OR branching tree with prerequisites
- [ ] Confirm RP is generated passively as machines produce output items
- [ ] Define if rare/complex items give more RP than simple ones (weighted RP)
- [ ] Define the starting state: what does the player have unlocked with zero research?

### 5b. Steam Branch Nodes (write out each node)
- [ ] Starting unlock (free): Steam Extractor + Boiler + Steam Furnace
- [ ] Node: Unlock Steam Macerator (cost: X RP)
- [ ] Node: Unlock Steam Alloy Smelter (cost: X RP)
- [ ] Node: Unlock first floor expansion 20×20 (cost: X RP, requires: Bronze production)
- [ ] Node: Unlock Steam → LV transition (cost: X RP, requires: completing Steam branch?)
- [ ] *(Optional)* Node: Pipe throughput upgrade — pipes carry 2 items/tick instead of 1
- [ ] *(Optional)* Node: Extraction rate upgrade — Extractor pulls 2x faster

### 5c. LV Branch Nodes
- [ ] Outline all LV unlock nodes (one per machine + key recipes)
- [ ] Assign RP costs (placeholder values, tune in playtesting)
- [ ] Define which LV node unlocks the next floor expansion

### 5d. Tree Diagram
- [ ] Sketch the tree on paper or in a tool (Miro, FigJam, even a text diagram)
- [ ] Ensure no branch is orphaned — every node has a visible path from start
- [ ] Ensure the critical path (most important machines) isn't gated too deep

---

## 6. Art Style Guide

### 6a. Palette
- [ ] Select palette — **recommended: DB32 (DawnBringer 32)** — 32 colors, warm industrial tones, widely used in pixel art
  - Alternative: Endesga 32 (brighter, more modern)
- [ ] Download palette `.png` and `.ase` (Aseprite swatch) and save to `Art/Palette/`
- [ ] Assign palette roles (examples):
  - Dark gray tones → machine casings
  - Copper/orange tones → copper pipes and wires
  - Yellow-green → active/processing state indicator
  - Red → error/no-power state
  - Blue → water/steam pipes

### 6b. Tile Anatomy (32×32 px per tile)
- [ ] Define machine tile anatomy:
  - Casing border (4px frame)
  - Center icon (16×16 functional symbol)
  - Status light (4×4 px corner indicator: green=running, red=stopped, yellow=idle)
  - Input port marker (arrow on edge tile facing inward)
  - Output port marker (arrow on edge tile facing outward)
- [ ] Define pipe tile variants — 4-directional bitmask = 16 combinations:
  - [ ] Straight horizontal
  - [ ] Straight vertical
  - [ ] Corner NE, NW, SE, SW
  - [ ] T-junction N, S, E, W
  - [ ] Cross (all 4 directions)
  - [ ] Single cap N, S, E, W (dead end)
- [ ] Define floor tile (plain + subtle grid line variant)
- [ ] Define node slot tile (inactive: faint ore outline; active with Extractor: glowing)
- [ ] Define cable tile variants (same 16 bitmask as pipes, different color)

### 6c. Animation
- [ ] Define machine idle animation: 0 frames (static) OR subtle 2-frame idle pulse
- [ ] Define machine active animation: 4–6 frame loop (spinning gear, pulsing light, etc.)
- [ ] Define pipe item transit animation: none (items invisible in pipes — GT style)
- [ ] Define node extraction animation: 2-frame pulse on the Extractor

### 6d. UI Style
- [ ] Select pixel font — recommended: **m5x7** (free, clean, highly legible at small sizes) or **04b_03**
- [ ] Download font and save to `Art/Fonts/`
- [ ] Define UI panel style: dark background + 1px light border (simple, readable on mobile)
- [ ] Define button style: 3-state (normal, hover/pressed, disabled)
- [ ] Define icon size for build menu items: 24×24 (fits label below in 32px row)

---

## 7. Mobile UI Wireframes

Draw wireframes (paper or Figma/FigJam) for each screen:

- [ ] **Main Game Screen**
  - Top bar: RP counter, current tier label, settings button
  - Center: scrollable/zoomable factory floor grid
  - Bottom toolbar: [Build] [Research] [Inspect] [Settings]
  - Floating: zoom in/out buttons (or pinch only?)

- [ ] **Build Menu** (appears on tap of empty tile)
  - Scrollable grid of available machines
  - Each item: machine icon + name + status (unlocked / locked / not affordable)
  - Cancel button

- [ ] **Machine Info Panel** (appears on tap of placed machine)
  - Machine name + tier badge
  - Current recipe (inputs → output)
  - Status indicator (processing / idle / error)
  - Power usage (EU/t or steam/t)
  - Progress bar
  - [Change Recipe] button (if machine supports multiple recipes)
  - [Remove Machine] button

- [ ] **Research Tree Screen** (full screen overlay)
  - Scrollable/pannable node tree
  - RP balance shown at top
  - Nodes: locked (grayed) / available (lit) / unlocked (colored)
  - Tap node: show details panel (what it unlocks, RP cost, [Unlock] button)

- [ ] **Settings Screen**
  - Audio sliders (music, SFX)
  - Explosion on overvolt: ON/OFF toggle
  - Reset / wipe save option

---

## 8. Technical Pre-Production

### 8a. Project Folder Structure
- [ ] Define and create `Assets/` folder layout:
  ```
  Assets/
    Scripts/
      Core/         (GridManager, PipeNetwork, PowerNetwork)
      Machines/     (MachineBase, MachineState, RecipeRunner)
      Research/     (ResearchTree, ResearchNode)
      UI/           (BuildMenu, MachinePanel, ResearchUI)
      Data/         (ScriptableObject definitions)
      Save/         (GridState, SaveManager)
    Data/
      Machines/     (MachineData SO assets)
      Recipes/      (RecipeData SO assets)
      Research/     (ResearchNodeData SO assets)
    Art/
      Tiles/        (floor, node slot sprites)
      Machines/     (machine sprites per tier)
      Pipes/        (16 pipe variant sprites)
      Cables/       (16 cable variant sprites)
      UI/           (panels, buttons, icons)
      Fonts/
      Palette/
    Scenes/
      Main.unity
      Bootstrap.unity (optional: load screen)
    Prefabs/
      Machines/
      Pipes/
      UI/
  ```

### 8b. Naming Conventions
- [ ] Scripts: `PascalCase` (e.g. `GridManager.cs`, `MachineData.cs`)
- [ ] SO assets: `[Tier]_[Name]` (e.g. `Steam_Boiler`, `LV_Assembler`)
- [ ] Sprites: `[category]_[name]_[variant]` (e.g. `pipe_corner_NE`, `machine_furnace_active_01`)
- [ ] Scenes: `PascalCase` (e.g. `GameScene.unity`)

### 8c. ScriptableObject Schemas
Define fields for each SO type before building them:

- [ ] **MachineData**
  - `string machineName`
  - `Tier tier` (enum: Steam, LV, MV, HV, EV, IV)
  - `int tileSizeX, tileSizeY` (usually 1, possibly 2 for large machines)
  - `int steamPerTick` or `int euPerTick`
  - `RecipeData[] availableRecipes`
  - `Sprite sprite`
  - `Vector2Int[] inputPortOffsets`
  - `Vector2Int[] outputPortOffsets`

- [ ] **RecipeData**
  - `ItemStack[] inputs` (item + count pairs)
  - `ItemStack[] outputs`
  - `float processingTime` (seconds)
  - `int energyCost` (EU/t or steam/t)

- [ ] **ResearchNodeData**
  - `string nodeName`
  - `int rpCost`
  - `ResearchNodeData[] prerequisites`
  - `UnlockType unlockType` (enum: Machine, Recipe, FloorExpansion, PipeUpgrade)
  - `ScriptableObject unlockTarget` (the MachineData or RecipeData being unlocked)

- [ ] **ItemData**
  - `string itemName`
  - `Sprite icon`
  - `int rpValuePerUnit` (how much RP producing 1 of this item generates)

### 8d. Key Design Decisions Before Phase 1 Code
- [ ] Confirm: does Steam use the same pipe network as items, or a separate network?
- [ ] Confirm: are cables a separate tile layer from pipes or on the same layer?
- [ ] Confirm: does 1 machine occupy exactly 1 tile, or can machines be 2×2?
- [ ] Confirm: is item transport instant (machine → pipe → machine in one tick) or time-based?
- [ ] Confirm: what is a "tick"? (fixed: 0.5s? 1s? configurable?)
- [ ] Confirm: does the Research Tree unlock machines globally or per-save?

---

## Deliverables Checklist

| Deliverable | File/Location | Done |
|---|---|---|
| Tier structure doc | `GDD.md` (update) | [ ] |
| Steam tier machine list | `GDD.md` or `Machines.md` | [ ] |
| LV tier machine list | `GDD.md` or `Machines.md` | [ ] |
| Recipe spreadsheet | `recipes.csv` | [ ] |
| Research tree diagram | `research_tree.md` or image | [ ] |
| Art style guide | `ART_GUIDE.md` | [ ] |
| DB32 palette swatches | `Art/Palette/` | [ ] |
| Pixel font files | `Art/Fonts/` | [ ] |
| Mobile UI wireframes | `Wireframes/` (images or Figma link) | [ ] |
| Asset folder structure | `Assets/` in Unity | [ ] |
| SO schema definitions | `PHASE_0_CHECKLIST.md` (this file) | [x] |
| Key pre-code decisions resolved | `GDD.md` (update) | [ ] |
