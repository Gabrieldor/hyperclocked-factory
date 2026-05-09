# Hyperclocked Factory — Claude Context
> Keep this file ≤ 100 lines. Full detail lives in `Docs/`. This file is always in context — be ruthless about what earns a line here.

## Project
Mobile factory/automation game inspired by Nomifactory (GregTech). Unity 2D, pixel art. See `GDD.md` for full GDD and `Docs/` for design docs.

---

## Confirmed Design Decisions

| Decision | Choice |
|---|---|
| Platform | Mobile — iOS + Android |
| Engine | Unity 2D (URP 2D) |
| Perspective | Top-down 2D, pixel art 32×32 px/tile |
| Player | Pure builder / cursor — no walking avatar |
| World | Fixed grid, 16×16 start → 64×64 max (milestone unlocks) |
| Item transfer | Item pipes only — no conveyor belts |
| Fluid transfer | Fluid pipes (separate layer) |
| Power | W (Watts = V × A); LV 32V → MV 128V → HV 512V |
| Progression | Milestone-based — gate item produced → auto-unlocks; no currency |
| Resource nodes | Fixed floor slots; Extractor placed on them; never deplete; ~60% slots vs types |
| Monetization | Free with cosmetics only — no pay-to-win, no timers |
| Grid layers | 3 overlay layers (item pipes, fluid pipes, cables) |
| Machine tile size | 1×1; multiblocks TBD |
| Transport | Time-based; 1 tick = 1 second |
| Save | Single slot. `GridState` → JSON |

---

## Tier Structure (v1.0)

| Tier | Unlock | Key Material | Power | Nodes |
|---|---|---|---|---|
| Steam | Start | Bronze | Steam (Boiler) | 3 slots / 5 types |
| LV | Steel in Brick Furnace | Steel | 32V | 6 slots / 10 types |
| MV | Aluminium in LV Blast Furnace | Stainless Steel | 128V | TBD |
| HV | Stainless Steel in MV Alloy Smelter | AE Controller | 512V | TBD |

Victory: build and activate the **Applied Energistics Controller**.

---

## Power System (see `Docs/Energy.md`)
- Cables: 1A / 4A / 8A per tier, lossless. Generator V > cable V → cable destroyed. Cable V > machine V → machine destroyed.
- Overcurrent (W demand > cable capacity) → cable burns out (contained; remove tile to replace).
- Overclocking: run recipe in higher-tier machine → 4× W, 2× speed per tier step.
- Battery Buffer: 8 slots, voltage-locked, 1A per slot. Battery tiers: 8K / 16K / 32K Ws.

---

## Steam Age (see `Docs/SteamTutorial.md`, `Docs/Recipes_Steam.md`)
- Starter Chest: 32× Coal, 24× Copper Dust, 8× Tin Dust, 32× Stone, 4× Item Pipe
- Water Node: pre-placed, immovable, infinite; 1-tile proximity = auto-draw
- Boiler: 1 Coal/8s + Water → 16 L/s steam; 32 L buffer per machine; halts (no damage) when dry
- LV gate: Steel Ingot (Brick Furnace) + Primitive Circuit (Chemical Reactor chain)
- Macerator → Impure Dust (except Coal → Coal Dust direct); Washer mandatory for ore doubling
- **Workshop (multiblock 3×3):** sole machine/multiblock crafter; player's first major build goal; Primitive Workbench consumed on activation; blueprint library expands via milestones
- **Player inventory:** Hotbar (8 slots, always visible) + Inventory (4×9 grid); Workshop output → hotbar → overflow inventory; long-press placed machine → pick up to inventory

---

## LV Resources (see `Docs/Resources.md`)
- New nodes: Gold, Clay, Lead (Pb), Nickel, Cinnabar — 6 total slots / 10 types
- Clay → Electrolyzer → Na, Li, Al Dust, Si Dust + Tiny Pile of Ga (compact 9:1 via Compressor)
- Lead Ore → Macerator → Impure Lead Dust + Mercury → Lead Dust + Arsenic Dust
- Cinnabar Dust + O₂ → Mercury + SO₂; SO₂ + O₂ + H₂O → H₂SO₄ (2× yield vs Steam recipe)

---

## LV Machines (see `Docs/Recipes_LV.md`)
- **Generators:** Steam Turbine (32W, 64 L/s steam), Solar Panel (8W, passive)
- **Steel Boiler (multiblock):** 3×3 (Controller + 8 Casings); 288 L/s; same coal rate as Boiler
- **Electric Furnace:** replaces Primitive Furnace; dust → ingot, 16W, 2s
- **Electric Blast Furnace:** replaces Brick Furnace; Steel 4× faster (4s); also Boule production; 64W
- **LV versions:** Macerator, Compressor, Extractor, Assembler, Alloy Smelter, Chemical Reactor, Ore Washer
- **New machines:** Lathe (ingot → 2× rod), Wiremill (ingot → 8× wire, 2× yield), Electrolyzer, Magnetizer, Centrifuge

---

## Architecture Guidelines
- Floor: Unity Tilemap with Random Tile, 2 asymmetric variants, rotation enabled (yields 8 apparent variants). Machines/pipes: GameObjects via `GridManager`. All data in ScriptableObjects — no hardcoded content.
- Item pipes: adjacency graph, BFS. Port assignment: player-assigned per pipe at runtime, stored in `GridState`. No port arrays in `MachineData`.
- Color coding: segment connects only to same-color neighbors; machines accept any color.
- Save: single slot, `GridState` → JSON at `Application.persistentDataPath`. Input: Unity Input System (touch + mouse parity).

---

## Development Phases
- **Phase 0** — Pre-production ← *current*. See `PHASE_0_CHECKLIST.md`
- **Phase 1** — Core prototype: grid, item pipes, Workbench + Furnace, milestone tracker UI
- **Phase 2** — Fluid pipes, power/cables, milestone tree UI, all Steam+LV content, save/load
- **Phase 3** — MV + HV content, Applied Energistics system
- **Phase 4** — Polish: art, audio, particles, tutorial flow, Android profiling
- **Phase 5** — Launch: cosmetics store, beta, App Store + Google Play

---

## Key Docs

| Doc | Path |
|---|---|
| Game Design Document | `GDD.md` |
| Phase 0 Checklist | `PHASE_0_CHECKLIST.md` |
| Energy System | `Docs/Energy.md` |
| Resources | `Docs/Resources.md` |
| Tier Structure | `Docs/TierStructure.md` |
| Steam Tutorial | `Docs/SteamTutorial.md` |
| Steam Recipes | `Docs/Recipes_Steam.md` |
| Steam Milestones | `Docs/Milestones_Steam.md` |
| LV Recipes | `Docs/Recipes_LV.md` |
| Machine Crafting (Workshop blueprints) | `Docs/Machine_Crafting.md` |
| Sprite Specs (per-machine frame counts) | `Docs/SpriteSpecs.md` |
