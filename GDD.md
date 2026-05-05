# Hyperclocked Factory — Game Design Document

## Concept

A mobile factory/automation game inspired by Nomifactory (GregTech modpack for Minecraft). The player builds increasingly complex production lines on a fixed factory floor, routing resources through machines via item pipes, powered by a voltage-tiered energy system. Progression is milestone-based: producing key items automatically unlocks new machines, floor expansions, and tier transitions — no currency, no points, no timers. Free-to-play with cosmetics-only monetization.

---

## Confirmed Design Decisions

| Decision | Choice |
|---|---|
| Platform | Mobile (iOS + Android) |
| Engine | Unity 2D |
| Perspective | Top-down 2D, pixel art |
| Player control | Pure builder / cursor (no avatar) |
| World | Fixed factory floor (grid-based) |
| Item transfer | Pipes only (no conveyor belts) |
| Power | Full voltage tiers: Steam → LV → MV → HV (v1.0); EV/IV future |
| Progression | Milestone-based — producing gate items unlocks machines, floors, and tier transitions |
| Resource nodes | Fixed input points + Extractor machines placed on them |
| Nodes deplete? | No — infinite (mobile QoL) |
| Monetization | Free with cosmetics only (no pay-to-win, no timers) |
| Grid layers | Item pipes, fluid pipes, cables each occupy a separate layer; switching layers shows machine layer as faint ghost |
| Machine tile size | 1×1; multiblocks to be designed separately |
| Transport | Time-based — items and fluids travel over ticks, not instantly |
| Tick duration | 1 second |
| Save slots | Single save only |

---

## Technical Specs

| Spec | Value |
|---|---|
| Tile size | 32x32 px |
| Starting floor | 16x16 tiles |
| Max floor | 64x64 tiles (expanded via milestones) |
| Base resolution | 360x640 |
| Resource nodes | Start with 2–3; each floor expansion adds 1–2 new node types |
| LV recipe depth | 2–3 steps |
| MV recipe depth | 4–6 steps |
| HV recipe depth | 6–8 steps |
| EV–IV recipe depth | 8–12+ steps |

---

## Core Game Loop

```
Place Extractor on Node
       |
    Raw Resources enter pipe network
       |
   Machines process items through recipe chains
       |
   Producing a milestone item → auto-unlocks new machines / floor expansion / tier transition
       |
   New machines enable deeper recipe chains
       |
   Build generators, upgrade cables, reach next tier
       |
   Repeat at greater complexity
```

---

## Core Systems

### 1. Grid / Tile System
- Fixed NxN tile grid, grows from 16x16 to 64x64 via milestones
- Each tile holds a machine (1×1) on the machine layer; pipes and cables occupy separate overlay layers on the same grid position
- Player switches active layer via toolbar; inactive layers shown as faint ghost for reference
- Tiles snap to grid on placement
- Unity `Tilemap` for floor layer; GameObjects for machines/pipes on top

### 2. Pipe Network
- Pipes connect adjacent machines
- Items routed based on machine output → nearest accepting machine input (BFS)
- Visual: pipe sprites auto-connect to neighbors via 4-directional bitmask
- Items are invisible in transit (GT pipe behavior, not Factorio belts)
- **Color coding:** each pipe/cable segment has an assigned color; a segment only connects to adjacent segments of the same color. Allows multiple independent networks to cross the same area. Machines connect to any color on their assigned port.
- Tap pipe → cycle color (or open color picker panel)
- Each pipe tile stores its color in `GridState`; art uses color tinting over the base bitmask sprites

### 3. Machine System
- Each machine: inputs (item + count), outputs (item + count), voltage tier, processing time
- Defined via `ScriptableObject` assets (`MachineData`, `RecipeData`)
- States: idle | processing | output full | no power | no input
- Tap machine → info panel (recipe, queue, power status, upgrade)

### 4. Power / Voltage System
- Tiers: LV → MV → HV → EV → IV
- Generators consume fuel items, output EU (energy units) to cable network
- Cable network is a separate layer from pipe network
- Machine explodes / shuts down if overpowered (difficulty option)
- Power consumed per processing tick, not per item

### 5. Resource Node + Extractor
- Node slots are pre-placed on map (visible, inactive until Extractor placed)
- Each node: fixed resource type (e.g. Iron Ore, Copper Ore, Coal)
- Extractor pulls resources into connected pipe at a base rate
- New node types added when floor expands via milestone unlock

### 6. Milestone Tree
- No currency — unlocks trigger automatically the first time a gate item is produced
- Tree UI is always visible as a roadmap: locked (gray) / available (lit) / unlocked (green)
- Each milestone unlocks: a machine type, a recipe set, a floor expansion, or a tier transition
- Milestones organized by tier branch; player always sees what they're working toward and why

### 7. Workshop (Multiblock — Steam age)

The Workshop is the only way to craft machines and multiblocks. Available from Steam age; its blueprint library expands via milestones throughout all tiers.

**Structure:** 3×3 multiblock — 1× Workshop Controller + 8× Workshop Frame tiles. All 9 tiles must form a contiguous 3×3 block. Controller detects the pattern on placement and activates.

**Storage:** 54 internal slots in the Controller. Player loads resources manually or routes item pipes into it.

**Crafting UI:** Opened by tapping the Controller. Shows all milestone-unlocked machine blueprints. Each entry displays the recipe cost and a Craft button. If resources are available in storage → tap Craft → instant output → machine placed in first empty hotbar slot, overflow to player inventory. If resources are missing → entry shows what is lacking (greyed/red).

**Scope:** The Workshop crafts all machines (1×1) and all multiblock components (Casing + Controller tiles for Steel Boiler, future multiblocks). It does not produce consumable items (pipes, cables, plates) — those come from factory production lines.

**Progression:** The Workshop itself is built from the Primitive Workbench as the player's first major construction goal. It replaces the Steam Workbench for all machine crafting. The Steam Workbench is demoted to item-only recipes (pipes, wires, components).

---

### 8. Player Inventory + Hotbar

Machines, pipes, and cables live in the player's personal inventory — not the factory floor storage.

**Hotbar:** 8 slots, always visible above the bottom toolbar. Tap a slot → selects that item type → tap a grid tile → places it. The selected slot stays active until the player switches or the slot empties.

**Inventory:** Full grid (4 rows × 9 columns = 36 slots). Opened from the toolbar. Holds machines/pipes/cables not on the hotbar. Tap inventory slot → sends item to first empty hotbar slot (or swaps if hotbar is full).

**Item flow:**
- Workshop craft → first empty hotbar slot → overflow to inventory
- Pick up a placed machine (long-press on machine tile, then confirm) → back to inventory
- Deleting a machine tile (no pick-up) consumes it permanently

**Toolbar (revised):** Milestones | Inventory | Inspect | Settings *(Build replaced by Inventory since hotbar handles placement)*

---

### 9. Touch UI
- Tap empty tile → place selected hotbar item (if one is selected)
- Tap machine → machine info panel
- Tap pipe → color picker / flow info
- Pinch to zoom, drag to pan
- Long-press machine → pick-up confirmation (returns machine to inventory)
- Bottom toolbar: Milestones | Inventory | Inspect | Settings

### 8. Cosmetics
- Machine skins, pipe style packs, floor tile themes, UI themes
- In-app cosmetics store, no gameplay effect

---

## Voltage Tier Plan

| Tier | Name | Key Machines |
|---|---|---|
| T1 | LV — Low Voltage | Furnace, Macerator, Extractor |
| T2 | MV — Medium Voltage | Alloy Smelter, Electrolyzer, Assembler |
| T3 | HV — High Voltage | Chemical Reactor, Distillery, Circuit Assembler |
| T4 | EV — Extreme Voltage | Industrial Blast Furnace, Centrifuge |
| T5 | IV — Insane Voltage | Advanced circuits, exotic materials |

---

## Development Phases

### Phase 0 — Pre-Production
- [ ] Recipe chain spreadsheet (LV + MV tiers minimum)
- [ ] Milestone tree structure (node list, unlock order)
- [ ] All LV machine types defined (target: 5–8 machines)
- [ ] Mobile UI wireframes
- [ ] Art style guide (pixel palette, tile anatomy)

### Phase 1 — Core Prototype
- [ ] Unity project setup (URP 2D, Input System, mobile build target)
- [ ] Grid tile system with tap-to-place
- [ ] Pipe network with basic item routing
- [ ] Extractor + Furnace working end-to-end
- [ ] Resource node → Extractor → Pipe → Furnace → Output
- [ ] Basic milestone tracker UI

### Phase 2 — Systems Complete
- [ ] Power / cable network (LV only)
- [ ] Full machine state machine
- [ ] Milestone tree UI
- [ ] All LV machines and recipes
- [ ] Machine info panel
- [ ] Save / load (JSON, `Application.persistentDataPath`)

### Phase 3 — MV Tier Content
- [ ] MV machines and recipes
- [ ] MV power generation
- [ ] Voltage upgrade flow
- [ ] Milestone tree MV branch

### Phase 4 — Polish
- [ ] Full pixel art pass (machines, pipes, floor, UI)
- [ ] Sound effects + ambient factory audio
- [ ] Particles (smoke, sparks)
- [ ] Tutorial / first-run flow
- [ ] Performance profiling on mid-range Android

### Phase 5 — Monetization & Launch
- [ ] Cosmetics store
- [ ] First cosmetics packs (2–3 machine skins, 1 floor theme)
- [ ] Analytics
- [ ] Beta test
- [ ] App Store + Google Play submission

---

## Unity Technical Notes

- **Tilemap**: Floor rendered via Tilemap. Machines/pipes are separate GameObjects managed by a `GridManager`.
- **ScriptableObjects**: `MachineData`, `RecipeData`, `MilestoneData` — data-driven, easy to add content without code changes.
- **Pipe graph**: Adjacency graph per pipe network. BFS on item output to find valid destination.
- **Save**: Custom `GridState` serialized to JSON. Stored at `Application.persistentDataPath`.
- **Input**: Unity Input System with pointer abstraction for touch + mouse parity in editor.

---

## Balancing — To Be Tuned in Playtesting

- Floor expansion milestone trigger items (what item produced unlocks each expansion)
- Power cable range before transformer needed
- Machine processing times per tier
