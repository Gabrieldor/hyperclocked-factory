# Resources

## Node Slots

Node slots are fixed floor positions that yield a specific resource when an Extractor is placed on them. The number of active slots is intentionally less than the total resource types available per tier, forcing players to prioritize and buffer.

Target: node slots ≈ 60% of primary ore types available for that tier.

| Tier | Node slots (total) | Primary ore types |
|---|---|---|
| Steam | 3 | 5 |
| LV | 6 | 10 |
| MV | TBD | TBD |
| HV | TBD | TBD |

LV floor expansion adds 3 new node slots (total grows from 3 → 6).

---

## Tiny Pile System

Some rare materials cannot be extracted directly as full dusts. They appear as **Tiny Piles** — a sub-unit equal to 1/9 of a full dust.

- Tiny Piles are produced as byproducts of other processing steps
- 9× Tiny Pile of X Dust → 1× X Dust via Compressor
- First introduced at LV with Gallium

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

3 slots for 5 ore types — players choose which 3 to actively extract at any time. Swapping requires stopping an Extractor and redeploying it.

### Special Sources

| Resource | Source | Notes |
|---|---|---|
| Water | Water Node — pre-placed, immovable, infinite | 1-tile proximity = auto-draw; fluid pipes for longer routing |
| Stone | Steam Washer byproduct | 1 Stone per impure dust washed; primary Stone source |

### Fluids

| Fluid | Source | Notes |
|---|---|---|
| Steam | Boiler (Coal + Water → 8 L/s) | Powers all steam machines via fluid pipe layer |
| Sulfuric Acid | Chemical Reactor (4× Sulfur Dust + 500 mL Water → 500 mL) | Etches Copper Boards for circuit chain |

---

## LV Tier

### New Ore Nodes at LV

5 new ore types added; combined with Steam carry-overs = 10 primary ores for 6 node slots.

| Resource | Notes |
|---|---|
| Gold | Wire, circuit components |
| Clay | Electrolyzer → Sodium, Lithium, Aluminium Dust, Silicon Dust; also yields Tiny Pile of Gallium Dust as byproduct |
| Lead (Pb) | Battery Alloy (battery housing); macerates to Impure Lead Dust → processed with Mercury for Arsenic |
| Nickel | Invar alloy (Nickel + Iron) → LV Blast Furnace components |
| Cinnabar | Mercury source; Mercury used to process Impure Lead Dust and in chemical chains |

### Clay Electrolyzer Outputs

Clay is the most complex LV node — one input, multiple outputs depending on the recipe selected in the Electrolyzer.

| Output | Form | Notes |
|---|---|---|
| Sodium Dust | Full dust | Tier II battery material |
| Lithium Dust | Full dust | Tier III battery material |
| Aluminium Dust | Full dust | MV gate chain |
| Silicon Dust | Full dust | LV circuit components; Silicon Boule (Blast Furnace) |
| Gallium Dust | Tiny Pile (byproduct) | Compact 9× → 1× Gallium Dust via Compressor; GaAs Boule |

### Lead Processing Chain

```
Lead Ore ──► Macerator ──► Impure Lead Dust ──► Chemical Reactor + Mercury ──► Lead Dust + Arsenic Dust
```

- Lead Dust → smelt → Lead Ingot → Battery Alloy
- Arsenic Dust → combined with Gallium → Gallium Arsenide → GaAs Boule (Blast Furnace)
- Mercury consumed (mL per batch); sourced from Cinnabar processing

### Cinnabar Processing

Cinnabar ore is processed to extract Mercury fluid. Processing method TBD when LV machines are finalized.

```
Cinnabar Ore ──► [processing] ──► Mercury (mL)
```

### Boules (MV/HV)

High-purity semiconductor crystal ingots. Made in the Blast Furnace.

| Boule | Inputs | Tier use |
|---|---|---|
| Silicon Boule | Silicon Dust (high purity) | Advanced circuits |
| GaAs Boule | Gallium Dust + Arsenic Dust | High-tier electronics |

### LV Fluids

| Fluid | Source | Notes |
|---|---|---|
| Oxygen | Electrolyzer (Water → O₂ + H₂) | TBD uses |
| Hydrogen | Electrolyzer (Water → O₂ + H₂) | TBD uses |
| Nitrogen | TBD | TBD uses |
| Mercury | Cinnabar processing | Lead ore refining; Arsenic extraction |
| Others | TBD | Defined when LV recipe chains are finalized |

---

## MV Tier

TBD — defined when MV design begins.

---

## HV Tier

TBD — defined when HV design begins.
