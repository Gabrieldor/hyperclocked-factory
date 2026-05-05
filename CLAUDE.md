# Hyperclocked Factory — Claude Context

## Project

Mobile factory/automation game inspired by Nomifactory (GregTech Minecraft modpack). Built in Unity 2D with pixel art. See `GDD.md` for the full GDD and `Docs/` for detailed design docs.

---

## Confirmed Design Decisions

| Decision | Choice |
|---|---|
| Platform | Mobile — iOS + Android |
| Engine | Unity 2D (URP 2D) |
| Perspective | Top-down 2D, pixel art 32×32 px/tile |
| Player | Pure builder / cursor — no walking avatar |
| World | Fixed grid, 16×16 start → 64×64 max (research expansions) |
| Item transfer | Item pipes only — no conveyor belts |
| Fluid transfer | Fluid pipes (separate layer) — water, steam, chemicals |
| Power | Voltage tiers: Steam → LV → MV → HV (v1.0 final) |
| Progression | Milestone-based — producing gate items auto-unlocks machines and tier transitions; no currency |
| Resource nodes | Fixed floor slots; Extractor machines placed on them; never deplete |
| Node count | Start 2–3; each floor expansion adds 1–2 new types |
| Recipe depth | 6–8 steps at HV end-game |
| Monetization | Free with cosmetics only — no pay-to-win, no timers |
| Grid layers | 3 overlay layers (item pipes, fluid pipes, cables); switching shows machine layer as ghost |
| Machine tile size | 1×1; multiblocks TBD separately |
| Transport | Time-based; 1 tick = 1 second |
| Save slots | Single save only |

---

## Tier Structure (v1.0)

| Tier | Unlock Condition | Key Material | Power | Recipe Depth |
|---|---|---|---|---|
| Steam | Start (tutorial) | Bronze | Steam — Boiler | 2–3 steps |
| LV | Produce Steel in Brick Furnace | Steel | EU — 8 EU/t | 3–5 steps |
| MV | Produce Aluminium in LV Blast Furnace | Stainless Steel | EU — 32 EU/t | 4–6 steps |
| HV | Produce Stainless Steel in MV Alloy Smelter | AE Controller | EU — 128 EU/t | 6–8 steps |

Victory: build and activate the **Applied Energistics Controller**.

---

## Steam Age

Full tutorial flow: `Docs/SteamTutorial.md` — Full recipe set: `Docs/Recipes_Steam.md`

Key implementation facts:
- Starter Chest (pre-placed, immovable): 32× Coal, 24× Copper Dust, 8× Tin Dust, 32× Stone, 4× Item Pipe
- Primitive Workbench (pre-placed) + Water Node (pre-placed, infinite, immovable)
- Water Node proximity rule: 1 tile adjacent = auto-draw, no pipe needed
- Steam no-power behavior: machine halts, no damage
- Boiler: 1 Coal/8s fuel slot + Water → 8 L/s steam; each steam machine has a 32 L buffer; steam backs up when buffer full (no waste)
- LV gate materials: Steel Ingot (Brick Furnace) + Primitive Circuit (Chemical Reactor chain)
- Macerator outputs **Impure Dust** (except Coal → Coal Dust directly); must be washed before smelting
- Steam Washer: Impure Dust + Water → Pure Dust + Stone; first machine with 2 output ports; primary Stone source
- Two parallel branches after Steam Workbench: A) Impure Dust → Washer + Brick Furnace; B) Bronze Plate → Compressor + Chemical Reactor → Sulfur Ore node → Sulfuric Acid → Primitive Circuit
- LV gate requires both branches: Steel Ingot (Branch A) + Primitive Circuit (Branch B)
- Chemical Reactor intro teaches chemical fluids and non-water fluid pipe routing

---

## Architecture Guidelines

- **Tilemap**: Floor via Unity Tilemap. Machines and pipes are GameObjects managed by `GridManager`.
- **ScriptableObjects**: All machine, recipe, and research data in SO assets — no hardcoded content.
- **Item pipe graph**: Adjacency graph. BFS on output to find nearest accepting input.
- **Port assignment**: No fixed port directions on machines. Any adjacent pipe tile can be assigned input or output by the player. Assignment stored in `GridState` (save data). `MachineData` SO carries no port offset arrays.
- **Color coding**: every pipe/cable segment has a color. A segment only connects to adjacent segments of the same color — allows independent networks to share the same area. Machines accept any color on their ports. Color stored per tile in `GridState`; rendered via tint over bitmask sprites.
- **Fluid pipe graph**: Separate adjacency graph. 1-tile proximity auto-connects during Steam tier.
- **Grid layers**: 3 independent overlay layers (item pipes, fluid pipes, cables). Ghost overlay on switch.
- **Transport**: Time-based; 1 tick = 1 second.
- **Save**: Single slot. `GridState` → JSON at `Application.persistentDataPath`.
- **Input**: Unity Input System — pointer abstraction (touch + mouse parity).
- **Power**: Cable layer/graph separate from pipe networks. EU consumed per machine tick.

---

## Development Phases

- **Phase 0** — Pre-production ← *current*. See `PHASE_0_CHECKLIST.md`
- **Phase 1** — Core prototype: grid, item pipes, Workbench + Furnace end-to-end, milestone tracker UI
- **Phase 2** — Systems: fluid pipes, power/cables, milestone tree UI, all Steam+LV content, save/load
- **Phase 3** — MV + HV tier content, Applied Energistics system
- **Phase 4** — Polish: art, audio, particles, tutorial flow, Android profiling
- **Phase 5** — Launch: cosmetics store, beta, App Store + Google Play

---

## Key Docs

| Doc | Path |
|---|---|
| Game Design Document | `GDD.md` |
| Phase 0 Checklist | `PHASE_0_CHECKLIST.md` |
| Tier Structure | `Docs/TierStructure.md` |
| Steam Tutorial | `Docs/SteamTutorial.md` |
| Steam Tier Recipes | `Docs/Recipes_Steam.md` |
| Steam Milestone Tree | `Docs/Milestones_Steam.md` |

---

## Balancing (tune in playtesting)

- Floor expansion milestone trigger items, cable range before transformer, processing times per tier, steam/EU rates per machine
