# Hyperclocked Factory — Claude Context

## Project

Mobile factory/automation game inspired by Nomifactory (GregTech Minecraft modpack). Built in Unity 2D with pixel art. See `GDD.md` for the full game design document and `Docs/` for detailed design docs.

---

## Confirmed Design Decisions

| Decision | Choice |
|---|---|
| Platform | Mobile — iOS + Android |
| Engine | Unity 2D (URP 2D) |
| Perspective | Top-down 2D |
| Art style | Pixel art, 32x32 px per tile |
| Player | Pure builder / cursor — no walking avatar |
| World | Fixed factory floor, 16×16 start → 64×64 max (research-unlocked expansions) |
| Item transfer | Item pipes only — no conveyor belts |
| Fluid transfer | Fluid pipes (separate type) — carry water, steam, chemicals |
| Power | Voltage tiers: Steam → LV → MV → HV (HV is v1.0 final tier) |
| Progression | Research point tree — producing items generates RP |
| Resource nodes | Fixed slots on floor; player places Extractor machines; nodes never deplete |
| Node count | Start 2–3; each floor expansion adds 1–2 new resource type slots |
| Recipe depth | Deep — 6–8 steps at HV end-game |
| Monetization | Free with cosmetics only — no pay-to-win, no timers |

---

## Tier Structure (v1.0)

| Tier | Unlock Condition | Key Material | New Power | Recipe Depth |
|---|---|---|---|---|
| Steam | Start (tutorial) | Bronze | Steam (coal-fired Boiler) | 2–3 steps |
| LV | Produce Steel in Brick Furnace | Steel | EU — 8 EU/t | 3–5 steps |
| MV | Produce Aluminium in LV Blast Furnace | Stainless Steel | EU — 32 EU/t | 4–6 steps |
| HV | Produce Stainless Steel in MV Alloy Smelter | AE Controller | EU — 128 EU/t | 6–8 steps |

HV endgame victory condition: build and activate the **Applied Energistics Controller**.

---

## Steam Age — Tutorial & Bootstrapping

The Steam Age is the tutorial. It teaches every core mechanic hands-on with no text walls.

### Starting Floor State
- **Starter Chest** — pre-placed, immovable. Contains: 32× Coal, 24× Copper Dust, 8× Tin Dust, 16× Stone
- **Primitive Workbench** — pre-placed, no power, instant crafting. Limited recipe whitelist only
- **Water Node** — pre-placed, infinite, immovable. Provides water via proximity or fluid pipe

### Tutorial Steps
1. Workbench: Copper Dust + Tin Dust → Bronze Dust (teaches recipe selection)
2. Workbench: Stone → Primitive Furnace (teaches machine crafting)
3. Place Furnace; draw item pipe from Workbench → Furnace (teaches pipe routing)
4. Pipe Furnace output → Workbench (teaches closing a loop)
5. Craft Boiler + Alloy Smelter; place Boiler **1 tile adjacent to Water Node** (teaches proximity fluid rule)
6. Workbench: self + 12× Bronze Ingot → Steam Workbench — **Primitive Workbench is consumed** (teaches machine upgrade)

### Water Node — Proximity Rule
- Boiler placed **1 tile adjacent** to Water Node auto-draws water with no pipe
- Same rule applies to steam output: machines adjacent to Boiler receive steam automatically
- Once **Fluid Pipes** are crafted, proximity is no longer required — free placement unlocked
- Fluid Pipes recipe: 4× Bronze Ingot → 4× Fluid Pipe segments (crafted in Steam Workbench)

### Primitive Workbench — Recipe Whitelist
| Recipe | Input | Output |
|---|---|---|
| Bronze Dust | 3× Copper Dust + 1× Tin Dust | 1× Bronze Dust |
| Primitive Furnace | 8× Stone | 1× Primitive Furnace |
| Boiler | 8× Stone + 4× Bronze Ingot | 1× Boiler |
| Alloy Smelter | 6× Bronze Ingot | 1× Alloy Smelter |
| Steam Workbench | 1× Primitive Workbench (self) + 12× Bronze Ingot | 1× Steam Workbench |

### Pipe Types
| Type | Carries | Visual | Unlock |
|---|---|---|---|
| Item Pipe | Solid items (ores, ingots, dusts, components) | Dark metal | Available from start |
| Fluid Pipe | Liquids and gases (water, steam, chemicals) | Blue-tinted, animated | Crafted in Steam Workbench |

---

## Architecture Guidelines

- **Tilemap**: Floor via Unity Tilemap. Machines and pipes are GameObjects managed by `GridManager`.
- **ScriptableObjects**: All machine, recipe, and research node data lives in SO assets — no hardcoded content.
- **Item pipe graph**: Adjacency graph. BFS on item output to find nearest accepting machine input.
- **Fluid pipe graph**: Separate adjacency graph for fluids. Proximity rule (1-tile radius) supplements pipes during Steam tier.
- **Save**: Custom `GridState` serialized to JSON at `Application.persistentDataPath`.
- **Input**: Unity Input System — pointer abstraction supports both touch (device) and mouse (editor).
- **Power network**: Separate layer/graph from both pipe networks. EU consumed per machine tick.

---

## Development Phases

- **Phase 0** — Pre-production: recipe spreadsheet, research tree design, machine lists, UI wireframes, art guide ← *current*
- **Phase 1** — Core prototype: grid, item pipes, Workbench + Furnace end-to-end, RP counter
- **Phase 2** — Systems complete: fluid pipes, power/cables, research tree UI, all Steam+LV content, save/load
- **Phase 3** — MV + HV tier content, Applied Energistics system
- **Phase 4** — Polish: full art, audio, particles, tutorial flow, Android profiling
- **Phase 5** — Launch: cosmetics store, beta, App Store + Google Play

---

## Key Docs

| Doc | Path |
|---|---|
| Game Design Document | `GDD.md` |
| Phase 0 Checklist | `PHASE_0_CHECKLIST.md` |
| Tier Structure | `Docs/TierStructure.md` |
| Steam Tutorial | `Docs/SteamTutorial.md` |

---

## Balancing (tune in playtesting)

- RP accumulation rate per item type
- Floor expansion RP cost / tier gate
- Power cable range before transformer required
- Machine processing times per tier
- Steam/EU consumption rates per machine
