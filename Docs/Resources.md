# Resources

## Node Slots

Node slots are fixed floor positions that yield a specific resource when an Extractor is placed on them. The number of active slots per tier section is intentionally less than the total resource types available, forcing players to prioritize and buffer.

| Tier | Node slots | Resource types available |
|---|---|---|
| Steam | 3 | 5 |
| LV | 5–6 | ~9 |
| MV | TBD | TBD |
| HV | TBD | TBD |

Target: node slots ≈ 60% of resource types available for that tier.

---

## Steam Tier

### Ore Nodes

| Resource | Unlock condition | Notes |
|---|---|---|
| Coal | Start | Boiler fuel; Coal Dust via Macerator (direct, no washing) |
| Copper Ore | Start | Bronze chain; circuit chain (Copper Plate → Etched Board) |
| Tin Ore | Start | Bronze chain |
| Iron Ore | Build Brick Furnace | Steel gate; LV machine crafting |
| Sulfur Ore | Mid-Steam research | Sulfuric Acid → circuit chain |

3 slots for 5 resources — players choose which 3 to actively extract at any time.

### Special Sources

| Resource | Source | Notes |
|---|---|---|
| Water | Water Node — pre-placed, immovable, infinite | 1-tile proximity = auto-draw; fluid pipes needed for longer routing |
| Stone | Steam Washer byproduct | 1 Stone per impure dust washed; primary Stone source in game |

### Fluids

| Fluid | Source | Notes |
|---|---|---|
| Steam | Boiler (Coal + Water → 8 L/s) | Powers all steam machines via fluid pipe layer |
| Sulfuric Acid | Chemical Reactor (4× Sulfur Dust + 500 mL Water → 500 mL) | Etches Copper Boards for circuit chain |

---

## LV Tier

### New Ore Nodes at LV

| Resource | Node slot | Primary uses |
|---|---|---|
| Gold | Yes | Wire, circuit components |
| Clay | Yes | Electrolyzer → Sodium, Lithium, Aluminium Dust, Silicon Dust |
| [Redstone replacement] | Yes | TBD — see design discussion |
| Lead | TBD | Battery Alloy (housing for battery items) |
| Nickel | TBD | Invar alloy → LV Blast Furnace |

### Clay Electrolyzer Outputs

Clay is the LV tier's most complex node — one input, four distinct outputs depending on the recipe run in the Electrolyzer.

| Output | Tier use |
|---|---|
| Sodium Dust | Tier II battery material |
| Lithium Dust | Tier III battery material |
| Aluminium Dust | MV gate chain |
| Silicon Dust | LV circuit components |

### LV Fluids

Produced by LV machines (Electrolyzer, Chemical Reactor). Not extracted from nodes.

| Fluid | Source | Notes |
|---|---|---|
| Oxygen | Electrolyzer (Water → O₂ + H₂) | TBD uses |
| Hydrogen | Electrolyzer (Water → O₂ + H₂) | TBD uses |
| Nitrogen | TBD | TBD uses |
| Others | TBD | Defined when LV recipe chains are finalized |

---

## MV Tier

TBD — defined when MV design begins.

---

## HV Tier

TBD — defined when HV design begins.
