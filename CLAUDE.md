# Hyperclocked Factory — Claude Context

## Project

Mobile factory/automation game inspired by Nomifactory (GregTech Minecraft modpack). Built in Unity 2D with pixel art. See `GDD.md` for the full game design document.

## Confirmed Design Decisions

| Decision | Choice |
|---|---|
| Platform | Mobile — iOS + Android |
| Engine | Unity 2D (URP 2D) |
| Perspective | Top-down 2D |
| Art style | Pixel art, 32x32 px per tile |
| Player | Pure builder / cursor — no walking avatar |
| World | Fixed factory floor, 16×16 start → 64×64 max (research-unlocked expansions) |
| Item transfer | Pipes only — no conveyor belts |
| Power | Voltage tiers: LV → MV → HV → EV → IV |
| Progression | Research point tree — producing items generates RP |
| Resource nodes | Fixed slots on floor; player places Extractor machines; nodes never deplete |
| Node count | Start 2–3; each floor expansion adds 1–2 new resource type slots |
| Recipe depth | Deep — 10+ steps at end-game (full Nomifactory complexity) |
| Monetization | Free with cosmetics only — no pay-to-win, no timers |

## Recipe Depth by Tier

| Tier | Steps |
|---|---|
| LV | 2–3 |
| MV | 4–6 |
| HV | 6–8 |
| EV–IV | 8–12+ |

## Architecture Guidelines

- **Tilemap**: Floor via Unity Tilemap. Machines and pipes are GameObjects managed by `GridManager`.
- **ScriptableObjects**: All machine, recipe, and research node data lives in SO assets — no hardcoded content.
- **Pipe graph**: Adjacency graph. BFS on item output to find nearest accepting machine input.
- **Save**: Custom `GridState` serialized to JSON at `Application.persistentDataPath`.
- **Input**: Unity Input System — pointer abstraction supports both touch (device) and mouse (editor).
- **Power network**: Separate layer/graph from pipe network. EU consumed per machine tick.

## Development Phases

- **Phase 0** — Pre-production: recipe spreadsheet, research tree design, LV machine list, UI wireframes, art guide
- **Phase 1** — Core prototype: grid, pipes, Extractor + Furnace end-to-end, RP counter
- **Phase 2** — Systems complete: power/cables, research tree UI, all LV content, save/load
- **Phase 3** — MV tier content
- **Phase 4** — Polish: full art, audio, particles, tutorial, Android profiling
- **Phase 5** — Launch: cosmetics store, beta, App Store + Google Play

## Balancing (tune in playtesting)

- RP accumulation rate per item type
- Floor expansion RP cost / tier gate
- Power cable range before transformer required
- Machine processing times per tier
