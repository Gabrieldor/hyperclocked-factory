# Hyperclocked Factory — Game Design Document

## Concept

A mobile factory/automation game inspired by Nomifactory (GregTech modpack for Minecraft). The player builds increasingly complex production lines on a fixed factory floor, routing resources through machines via item pipes, powered by a voltage-tiered energy system. Progression is driven by a research tree unlocked by producing items. Free-to-play with cosmetics-only monetization.

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
| Power | Full voltage tiers: LV → MV → HV → EV → IV |
| Progression | Research point tree (items produced = RP) |
| Resource nodes | Fixed input points + Extractor machines placed on them |
| Nodes deplete? | No — infinite (mobile QoL) |
| Monetization | Free with cosmetics only (no pay-to-win, no timers) |

---

## Technical Specs

| Spec | Value |
|---|---|
| Tile size | 32x32 px |
| Starting floor | 16x16 tiles |
| Max floor | 64x64 tiles (expanded via research) |
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
   Output items --> Research Points (RP) generated
       |
   RP spent on Research Tree (new machines / recipes / expansions)
       |
   New recipes require higher voltage tier
       |
   Build generators, upgrade cables, reach next tier
       |
   Repeat at greater complexity
```

---

## Core Systems

### 1. Grid / Tile System
- Fixed NxN tile grid, grows from 16x16 to 64x64 via research
- Each tile holds: machine, pipe segment, node slot, or empty floor
- Tiles snap to grid on placement
- Unity `Tilemap` for floor layer; GameObjects for machines/pipes on top

### 2. Pipe Network
- Pipes connect adjacent machines
- Items routed based on machine output → nearest accepting machine input (BFS)
- Visual: pipe sprites auto-connect to neighbors via 4-directional bitmask
- Items are invisible in transit (GT pipe behavior, not Factorio belts)
- Late unlock: pipe priority / filter configuration

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
- Extraction rate upgradeable via research

### 6. Research Tree
- Producing output items generates RP passively
- RP spent on tree nodes organized by voltage tier branch
- Each node unlocks: machine type, recipe, pipe upgrade, or floor expansion
- Tree always visible — player always knows what they're working toward

### 7. Touch UI
- Tap empty tile → build menu
- Tap machine → machine panel
- Tap pipe → flow info
- Pinch to zoom, drag to pan
- Bottom toolbar: Research Tree | Build | Inspect | Settings
- Tap-to-select + tap-to-place (no drag-and-drop)

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
- [ ] Research tree structure (node list, costs, unlock order)
- [ ] All LV machine types defined (target: 5–8 machines)
- [ ] Mobile UI wireframes
- [ ] Art style guide (pixel palette, tile anatomy)

### Phase 1 — Core Prototype
- [ ] Unity project setup (URP 2D, Input System, mobile build target)
- [ ] Grid tile system with tap-to-place
- [ ] Pipe network with basic item routing
- [ ] Extractor + Furnace working end-to-end
- [ ] Resource node → Extractor → Pipe → Furnace → Output
- [ ] Basic RP counter UI

### Phase 2 — Systems Complete
- [ ] Power / cable network (LV only)
- [ ] Full machine state machine
- [ ] Research tree UI
- [ ] All LV machines and recipes
- [ ] Machine info panel
- [ ] Save / load (JSON, `Application.persistentDataPath`)

### Phase 3 — MV Tier Content
- [ ] MV machines and recipes
- [ ] MV power generation
- [ ] Voltage upgrade flow
- [ ] Research tree MV branch

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
- **ScriptableObjects**: `MachineData`, `RecipeData`, `ResearchNodeData` — data-driven, easy to add content without code changes.
- **Pipe graph**: Adjacency graph per pipe network. BFS on item output to find valid destination.
- **Save**: Custom `GridState` serialized to JSON. Stored at `Application.persistentDataPath`.
- **Input**: Unity Input System with pointer abstraction for touch + mouse parity in editor.

---

## Balancing — To Be Tuned in Playtesting

- RP accumulation rate per item type
- Floor expansion RP cost / tier requirement
- Power cable range before transformer needed
- Machine processing times per tier
