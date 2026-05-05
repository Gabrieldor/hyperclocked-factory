# Phase 0 — Pre-Production Checklist

> Goal: Define everything needed before writing a single line of Unity code.
> All decisions here feed directly into Phase 1 implementation.

---

## 1. Tier Structure

- [x] Confirm full tier order: **Steam → LV → MV → HV** (v1.0; EV/IV post-launch) → `CLAUDE.md`
- [x] Define what "unlocks" each tier → producing tier-gate item (e.g. Steel = LV) → `CLAUDE.md` Tier Structure
- [x] Define recipe step count target per tier → `CLAUDE.md` Tier Structure

---

## 2. Steam Tier — Design

### 2a. Resources
- [x] Confirm Steam tier ore nodes: **Copper, Tin, Coal** → `Docs/Recipes_Steam.md`
- [x] Add a Water source mechanic → Fixed Water Node, pre-placed, infinite, immovable → `Docs/SteamTutorial.md`
- [x] Decide if Stone is a Steam tier resource → **Yes**, used for machine casings → `Docs/Recipes_Steam.md`

### 2b. Steam System
- [x] Define the Boiler machine → 1 Coal/8s + Water → 8 steam/t → `Docs/Recipes_Steam.md`
- [x] Decide Steam distribution → **Separate fluid pipe layer** → `CLAUDE.md`
- [x] Define Steam pressure / capacity concept → **32 L buffer per machine; steam backs up (no waste) when buffer full** → `Docs/Recipes_Steam.md`
- [x] Define what happens when a steam machine runs without steam → **halts, no damage** → `Docs/Recipes_Steam.md`

### 2c. Steam Tier Machines
- [x] **Boiler** — Coal + Water → 8 steam/t → `Docs/Recipes_Steam.md`
- [x] **Primitive Furnace** *(bootstrap furnace, coal-fueled, no steam)* — Dust/Ore → Ingot → `Docs/Recipes_Steam.md`
- [x] **Steam Macerator** — Ore → 2× Dust → `Docs/Recipes_Steam.md`
- [x] **Alloy Smelter** — 2 inputs → Alloy Ingot → `Docs/Recipes_Steam.md`
- [x] **Steam Extractor** — placed on node, 1 ore/4t, 2 steam/t → `Docs/Recipes_Steam.md`
- [x] **Steam Compressor** — Ingot → Plate → `Docs/Recipes_Steam.md`
- [x] **Steam Workbench** — upgraded crafting station, 2 steam/t → `Docs/Recipes_Steam.md`
- [x] **Brick Furnace** — 16 Iron + 8 Coal → 4 Steel; LV gate machine → `Docs/Recipes_Steam.md`
- [x] **Steam Washer** — Impure Dust + Water → Pure Dust + Stone; 2 output ports; primary Stone source → `Docs/Recipes_Steam.md`
- [x] **Chemical Reactor** — fluid+solid reactions; Sulfuric Acid + Etched Copper Board; intro to chemistry → `Docs/Recipes_Steam.md`
- For each machine:
  - [x] Function (inputs → outputs) → `Docs/Recipes_Steam.md`
  - [x] Steam consumption rate → `Docs/Recipes_Steam.md`
  - [x] Processing time → `Docs/Recipes_Steam.md`
  - [x] Tile size → **1×1 for all** → `CLAUDE.md`
  - [x] Input/output port directions → **any adjacent tile with a pipe can be assigned input or output by the player** → `CLAUDE.md`

### 2d. Steam Tier Recipe Chains
- [x] **Bronze chain**: Copper Ore → Macerator → Dust → Alloy Smelter → Bronze Ingot → `Docs/Recipes_Steam.md`
- [x] **Ore doubling chain**: Ore → Macerator → 2× Dust → Furnace → 2× Ingot → `Docs/Recipes_Steam.md`
- [x] **Coal usage**: Coal → Boiler fuel + Brick Furnace input → `Docs/Recipes_Steam.md`
- [x] **LV gate chain**: Iron Ore → Macerator → Dust → Furnace → Ingot → Brick Furnace → Steel → `Docs/Recipes_Steam.md`
- [x] Write all Steam tier recipes → `Docs/Recipes_Steam.md`
- [x] Verify recipe graph has no circular dependencies

---

## 3. LV Tier — Design

### 3a. Resources (new nodes unlocked at LV)
- [x] Confirm LV introduces: Gold, Clay, Lead (Pb), Nickel, Cinnabar (Iron was late-Steam, not new at LV) → `Docs/Resources.md`
- [x] Decide if Redstone is an LV node → **No** — Silicon (from Clay) and Gold cover circuit needs
- [x] Decide if Gold is an LV node → **Yes** → `Docs/Resources.md`
- [x] Stone/Sand node → Stone remains Washer byproduct only; no dedicated node needed
- [x] Total LV node slots: **6** (3 Steam carry-over + 3 new); 10 ore types → ~60% rule → `Docs/Resources.md`
- [x] Tiny Pile system: 9× Tiny Pile → 1× Dust via Compressor; first used for Gallium → `Docs/Resources.md`
- [x] Gallium source: Tiny Pile byproduct of Clay electrolysis → `Docs/Resources.md`
- [x] Arsenic source: Impure Lead Dust + Mercury → Chemical Reactor → Lead Dust + Arsenic Dust → `Docs/Resources.md`

### 3b. Power System — LV
- [x] Define power unit → **W (Watts = V × A)**; not EU/t → `Docs/Energy.md`
- [x] Define voltage tiers → LV 32V / MV 128V / HV 512V (×4 per tier) → `Docs/Energy.md`
- [x] Define cables → 1A / 4A / 8A per tier; lossless → `Docs/Energy.md`
- [x] Define overvolt → Generator V > cable V → cable destroyed; cable V > machine V → machine destroyed → `Docs/Energy.md`
- [x] Define overcurrent → W demand > cable capacity → cable burns (contained; remove to replace) → `Docs/Energy.md`
- [x] Define overclocking → higher-tier machine runs same recipe; 4× W, 2× speed per tier step → `Docs/Energy.md`
- [x] Define Transformer → passive 1×1; steps voltage one tier up or down → `Docs/Energy.md`
- [x] Define Battery Buffer → 8 slots, voltage-locked, 1A per slot, charges + buffers → `Docs/Energy.md`
- [x] Define battery tiers → Tier I: 8K Ws / Tier II: 16K Ws / Tier III: 32K Ws → `Docs/Energy.md`
- [ ] Define the LV Generator machine: fuel type(s), V output, A output → TBD

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
- For each machine: define function, EU/t, processing time, tile size (1×1), port directions

### 3d. LV Tier Recipe Chains
- [ ] **Steel chain**: Iron Ore → Dust (Macerator) → Ingot (Furnace) → Steel (LV Alloy Smelter + Carbon)
- [ ] **Circuit chain**: Copper Wire (Wiremill) + Iron Plate (Cutter) + Redstone → Basic Circuit (Assembler)
- [ ] Write all LV recipes (same format as Steam tier, in `Docs/Recipes_LV.md`)
- [ ] Confirm every LV machine has at least 3 useful recipes

---

## 4. Recipe Chain Spreadsheet

- [x] Define format: Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | Steam/t or EU/t
- [x] Steam tier: all recipes filled in → `Docs/Recipes_Steam.md`
- [ ] LV tier: all recipes filled in → `Docs/Recipes_LV.md`
- [ ] MV tier: at least 50% of recipes outlined
- [ ] Verify no recipe is a dead end (every output is used somewhere)
- [x] Identify the "critical path" — Bronze path + LV gate path → `Docs/Recipes_Steam.md`
- [ ] Identify 3 items that require the widest machine variety to produce (tier goal markers)

---

## 5. Milestone Tree Design

No currency. Unlocks trigger automatically the first time a gate item is produced or a gate machine is built.

### 5a. Structure
- [ ] Decide tree layout: linear chain per tier OR branching tree with parallel tracks
- [ ] Define starting state: what is available at floor generation with zero milestones hit?
- [ ] Define floor expansion triggers: what item/machine unlocks each floor size step?

### 5b. Steam Milestones
- [x] Steam milestone tree written → `Docs/Milestones_Steam.md`
- [ ] Fill in OPT1 Bronze Ingot count threshold (tune in playtesting)

### 5c. LV Milestones
- [ ] Write LV milestone tree → `Docs/Milestones_LV.md`
- [ ] Define which LV milestone unlocks the next floor expansion (toward 64×64)

### 5d. Tree Diagram
- [x] Steam tree diagram included in `Docs/Milestones_Steam.md`
- [ ] LV+ tree diagram → `Docs/Milestones_LV.md`
- [ ] Ensure no milestone is orphaned — every node has a visible path from start

---

## 6. Art Style Guide

### 6a. Palette
- [ ] Select palette — **recommended: DB32 (DawnBringer 32)** — 32 colors, warm industrial tones
  - Alternative: Endesga 32 (brighter, more modern)
- [ ] Download palette `.png` and `.ase` (Aseprite swatch) → `Art/Palette/`
- [ ] Assign palette roles: dark gray = casings, copper/orange = pipes, yellow-green = active, red = error, blue = fluid pipes

### 6b. Tile Anatomy (32×32 px)
- [ ] Machine tile: 4px casing border, 16×16 center icon, 4×4 status light corner, input/output port arrows
- [ ] Pipe tile variants — 16 combinations (4-directional bitmask): straights, corners, T-junctions, cross, caps
- [ ] Floor tile (plain + subtle grid line variant)
- [ ] Node slot tile (inactive: faint ore outline; active with Extractor: glowing)
- [ ] Cable tile variants (same 16 bitmask as pipes, different color)

### 6c. Animation
- [ ] Machine idle: static OR 2-frame pulse
- [ ] Machine active: 4–6 frame loop (spinning gear, pulsing light)
- [ ] Pipe item transit: none (items invisible — GT style)
- [ ] Node extraction: 2-frame pulse on Extractor

### 6d. UI Style
- [ ] Select pixel font — recommended: **m5x7** or **04b_03** → `Art/Fonts/`
- [ ] UI panel style: dark background + 1px light border
- [ ] Button style: 3-state (normal, pressed, disabled)
- [ ] Build menu icon size: 24×24 px

---

## 7. Mobile UI Wireframes

- [ ] **Main Game Screen** — top bar (tier, settings), center (factory grid), bottom toolbar (Milestones/Inventory/Inspect/Settings)
- [ ] **Hotbar** — 8 slots above toolbar; tap slot to select item type, tap grid to place
- [ ] **Inventory Screen** — 4×9 grid of machines/pipes/cables; tap slot → moves to hotbar
- [ ] **Machine Info Panel** — name, tier, recipe, status, power, progress bar, Change Recipe, Remove
- [ ] **Milestone Tree Screen** — full screen, scrollable/pannable, node states (locked/available/unlocked)
- [ ] **Settings Screen** — audio sliders, overvolt toggle, reset save

---

## 8. Technical Pre-Production

### 8a. Project Folder Structure
- [ ] Create `Assets/` layout in Unity:
  - `Scripts/` → Core/, Machines/, Milestones/, UI/, Data/, Save/
  - `Data/` → Machines/, Recipes/, Milestones/ (SO assets)
  - `Art/` → Tiles/, Machines/, Pipes/, Cables/, UI/, Fonts/, Palette/
  - `Scenes/` → Main.unity, Bootstrap.unity
  - `Prefabs/` → Machines/, Pipes/, UI/

### 8b. Naming Conventions
- [x] Scripts: `PascalCase` (e.g. `GridManager.cs`)
- [x] SO assets: `[Tier]_[Name]` (e.g. `Steam_Boiler`, `LV_Assembler`)
- [x] Sprites: `[category]_[name]_[variant]` (e.g. `pipe_corner_NE`, `machine_furnace_active_01`)
- [x] Scenes: `PascalCase` (e.g. `GameScene.unity`)

### 8c. ScriptableObject Schemas
- [x] **MachineData** — machineName, tier (enum), tileSizeX/Y, steamPerTick/euPerTick, availableRecipes[], sprite *(no port offset arrays — ports are player-assigned per pipe connection at runtime)*
- [x] **RecipeData** — inputs (ItemStack[]), outputs (ItemStack[]), processingTime, energyCost
- [x] **MilestoneData** — milestoneName, triggerItem (ItemData ref) or triggerMachine (MachineData ref), unlockType (enum: Machine, Recipe, FloorExpansion, TierTransition), unlockTarget (SO ref), prerequisites[]
- [x] **ItemData** — itemName, icon (Sprite), stackSize

### 8d. Key Design Decisions Before Phase 1 Code
- [x] Steam uses separate fluid pipe layer → `CLAUDE.md`
- [x] Cables are a separate layer (3 total: item pipes, fluid pipes, cables) → `CLAUDE.md`
- [x] All machines are 1×1; multiblocks TBD → `CLAUDE.md`
- [x] Pipes and cables are color-coded; segment only connects to same-color neighbors; machines accept any color → `CLAUDE.md`
- [x] Item and fluid transport is time-based → `CLAUDE.md`
- [x] 1 tick = 1 second → `CLAUDE.md`
- [x] Single save slot; milestone state is per-save → `CLAUDE.md`

---

## Deliverables Checklist

| Deliverable | File/Location | Done |
|---|---|---|
| Tier structure doc | `Docs/TierStructure.md` + `CLAUDE.md` | [x] |
| Energy system doc | `Docs/Energy.md` | [x] |
| Resources doc | `Docs/Resources.md` | [x] |
| Steam tier machine list | `Docs/Recipes_Steam.md` | [x] |
| Steam tier recipes | `Docs/Recipes_Steam.md` | [x] |
| LV tier machine list | `Docs/Recipes_LV.md` | [ ] |
| LV tier recipes | `Docs/Recipes_LV.md` | [ ] |
| MV+ recipes (50%) | `Docs/Recipes_MV.md` | [ ] |
| Steam milestone tree | `Docs/Milestones_Steam.md` | [x] |
| LV milestone tree | `Docs/Milestones_LV.md` | [ ] |
| Art style guide | `Docs/ArtGuide.md` | [ ] |
| DB32 palette swatches | `Art/Palette/` | [ ] |
| Pixel font files | `Art/Fonts/` | [ ] |
| Mobile UI wireframes | `Wireframes/` (images or Figma link) | [ ] |
| Asset folder structure | `Assets/` in Unity | [ ] |
| SO schema definitions | `PHASE_0_CHECKLIST.md` §8c | [x] |
| Key pre-code decisions resolved | `CLAUDE.md` | [x] |
