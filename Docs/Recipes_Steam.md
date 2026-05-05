# Steam Tier — Recipe Reference

All recipes for the Steam tier. Balance values (times, steam rates, quantities) are placeholder — tune in playtesting.

Column key: **Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s**
L/s = Liters of steam consumed per second. 0 = no steam required (manual or coal-fueled).
Fluid ingredient quantities are in mL (e.g. 500 mL Water, 250 mL Sulfuric Acid).

**Steam capacity rules:**
- Each steam machine has a **32 L internal buffer**
- Boiler produces **8 L/s** (continuous while fueled + watered)
- If a machine's buffer is full (idle or output blocked), it stops drawing steam
- 1 Boiler (8 L/s) can sustain machines whose total draw ≤ 8 L/s

---

## Items & Resources

| Category | Items |
|---|---|
| Raw nodes | Copper Ore, Tin Ore, Coal, Water *(fluid)* |
| Post-expansion nodes | Iron Ore *(1st expansion)*, Sulfur Ore *(mid-Steam research unlock)* |
| Impure dusts | Impure Copper Dust, Impure Tin Dust, Impure Iron Dust, Impure Sulfur Dust *(Macerator output)* |
| Pure dusts | Copper Dust, Tin Dust, Iron Dust, Bronze Dust, Coal Dust, Sulfur Dust |
| Ingots | Copper Ingot, Tin Ingot, Iron Ingot, Bronze Ingot, Steel Ingot |
| Plates | Bronze Plate, Iron Plate, Copper Plate |
| Byproducts | Stone *(primary source: Washer; bootstrap supply in Starter Chest)* |
| Storage | Stone Chest *(placeable storage; 8× Stone → 1× Stone Chest)* |
| Fluids | Water, Steam, Sulfuric Acid *(mL)* |
| Components | Copper Wire, Etched Copper Board, Primitive Circuit |
| Pipes | Item Pipe, Fluid Pipe |

---

## Machines

| Machine | Power Source | L/s | Notes |
|---|---|---|---|
| Primitive Workbench | None (manual, instant) | 0 | Pre-placed; whitelist only; consumed on upgrade to Steam Workbench |
| Primitive Furnace | Coal fuel slot (1 coal = 8s) | 0 | Pre-steam bootstrap; no steam needed |
| Boiler | Coal fuel slot (1 coal = 8s) + Water | +8 | Generator; produces 8 L/s; 32 L output buffer |
| Alloy Smelter | Steam (32 L buffer) | 4 | Core Steam machine |
| Steam Workbench | Steam (32 L buffer) | 2 | Upgraded crafting station |
| Steam Macerator | Steam (32 L buffer) | 4 | Ore doubling; research unlock |
| Steam Compressor | Steam (32 L buffer) | 2 | Makes plates; research unlock |
| Steam Extractor | Steam (32 L buffer) | 2 | Placed on node; continuous pull, not recipe-based |
| Brick Furnace | Steam (32 L buffer) | 4 | Late Steam; only produces Steel; gates LV tier |
| Chemical Reactor | Steam (32 L buffer) | 4 | Handles fluid+solid reactions; intro to chemistry; research unlock |
| Steam Washer | Steam (32 L buffer) | 2 | Impure Dust + Water → Pure Dust + Stone; first machine with 2 output ports; research unlock |

---

## Boiler — Generator Specs

Not a recipe machine. Converts fuel + water into steam.

- **Fuel slot:** 1 Coal = 8 seconds of operation
- **Water input:** 1 water/t continuous (proximity or fluid pipe)
- **Steam output:** 8 L/s continuous while fueled and watered; 32 L output buffer
- **Idle:** No coal or no water → output stops, no damage; buffer drains as machines consume

> 1 Boiler (8 L/s) can sustain one 4 L/s machine + one Extractor (2 L/s) simultaneously.
> Running two 4 L/s machines requires a second Boiler.

---

## Steam Extractor — Continuous Pull Specs

Not a recipe machine. Placed on a resource node tile.

- **Pull rate:** 1 ore per 4 ticks (base)
- **Steam consumption:** 2 steam/t continuous while running
- **Research upgrades:** ×2 rate, ×4 rate (defined in research tree)

---

## Recipes

### Primitive Workbench (instant, no power)

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Primitive Workbench | Copper Dust | 3 | Tin Dust | 1 | Bronze Dust | 4 | 0 | 0 |
| Steam | Primitive Workbench | Stone | 8 | — | — | Primitive Furnace | 1 | 0 | 0 |
| Steam | Primitive Workbench | Bronze Ingot | 4 | Stone | 4 | Boiler | 1 | 0 | 0 |
| Steam | Primitive Workbench | Bronze Ingot | 6 | — | — | Alloy Smelter | 1 | 0 | 0 |
| Steam | Primitive Workbench | Primitive Workbench | 1 | Bronze Ingot | 12 | Steam Workbench | 1 | 0 | 0 |
| Steam | Primitive Workbench | Copper Ingot | 2 | — | — | Item Pipe | 4 | 0 | 0 |
| Steam | Primitive Workbench | Stone | 8 | — | — | Stone Chest | 1 | 0 | 0 |

> Item Pipe recipe requires Copper Ingot (needs Primitive Furnace first).
> Starter Chest includes 4× Item Pipe so the very first pipe connection is not blocked.

---

### Primitive Furnace (coal fuel slot, no steam)

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Primitive Furnace | Copper Dust | 1 | — | — | Copper Ingot | 1 | 4 | 0 |
| Steam | Primitive Furnace | Tin Dust | 1 | — | — | Tin Ingot | 1 | 4 | 0 |
| Steam | Primitive Furnace | Bronze Dust | 1 | — | — | Bronze Ingot | 1 | 4 | 0 |
| Steam | Primitive Furnace | Iron Dust | 1 | — | — | Iron Ingot | 1 | 4 | 0 |
| Steam | Primitive Furnace | Copper Ore | 1 | — | — | Copper Ingot | 1 | 8 | 0 |
| Steam | Primitive Furnace | Tin Ore | 1 | — | — | Tin Ingot | 1 | 8 | 0 |

> Ore → Ingot direct is slower (8s vs 4s) and gives 1:1 instead of the Macerator+Washer 2:1 route.
> Impure Dust cannot be smelted directly — washing is mandatory to access the doubled yield.

---

### Alloy Smelter (4 L/s)

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Alloy Smelter | Copper Ingot | 3 | Tin Ingot | 1 | Bronze Ingot | 4 | 4 | 4 |
| Steam | Alloy Smelter | Copper Dust | 3 | Tin Dust | 1 | Bronze Ingot | 4 | 5 | 4 |

> Two Bronze routes: ingot route (faster) and dust route (slower, works without a prior furnace step).
> Dust route kept so the machine is never useless in early game.

---

### Steam Workbench (2 L/s)

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Steam Workbench | Bronze Ingot | 4 | — | — | Fluid Pipe | 4 | 3 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 8 | Stone | 4 | Steam Macerator | 1 | 8 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 4 | Stone | 4 | Steam Compressor | 1 | 8 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 6 | — | — | Steam Extractor | 1 | 8 | 2 |
| Steam | Steam Workbench | Iron Ingot | 16 | Stone | 64 | Brick Furnace | 1 | 10 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 8 | Fluid Pipe | 4 | Chemical Reactor | 1 | 10 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 16 | — | — | Steam Workbench | 1 | 10 | 2 |
| Steam | Steam Workbench | Bronze Ingot | 6 | Fluid Pipe | 2 | Steam Washer | 1 | 8 | 2 |
| Steam | Steam Workbench | Copper Ingot | 1 | — | — | Copper Wire | 4 | 3 | 2 |
| Steam | Steam Workbench | Etched Copper Board | 1 | Copper Wire | 4 | Primitive Circuit | 1 | 8 | 2 |

> Steam Workbench can replicate itself using only Bronze — no Primitive Workbench required.
> Costs more than the original (16 Bronze vs 12 Bronze + consuming the Primitive Workbench) to keep the first upgrade meaningful.

---

### Steam Macerator (4 L/s) — research unlock

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Steam Macerator | Copper Ore | 1 | — | — | Impure Copper Dust | 2 | 4 | 4 |
| Steam | Steam Macerator | Tin Ore | 1 | — | — | Impure Tin Dust | 2 | 4 | 4 |
| Steam | Steam Macerator | Iron Ore | 1 | — | — | Impure Iron Dust | 2 | 4 | 4 |
| Steam | Steam Macerator | Sulfur Ore | 1 | — | — | Impure Sulfur Dust | 2 | 4 | 4 |
| Steam | Steam Macerator | Coal | 1 | — | — | Coal Dust | 2 | 4 | 4 |

> Ores produce **Impure Dust** — must be washed before smelting. Impure Dust cannot be smelted directly.
> Coal is the exception: it outputs Coal Dust directly (fuel doesn't require purification).
> Sulfur Ore node unlocked via mid-Steam research; required for the circuit chain.

---

### Steam Compressor (2 L/s) — research unlock

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Steam Compressor | Bronze Ingot | 2 | — | — | Bronze Plate | 1 | 4 | 2 |
| Steam | Steam Compressor | Iron Ingot | 2 | — | — | Iron Plate | 1 | 4 | 2 |

> Plates are primarily used in LV machine crafting. Compressor is a late-Steam research unlock to prepare for the tier transition.

---

### Steam Washer (2 L/s) — research unlock

Two output ports required: one for pure dust, one for Stone byproduct.

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output 1 | Qty | Output 2 | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|---|---|
| Steam | Steam Washer | Impure Copper Dust | 1 | Water | 100 mL | Copper Dust | 1 | Stone | 1 | 4 | 2 |
| Steam | Steam Washer | Impure Tin Dust | 1 | Water | 100 mL | Tin Dust | 1 | Stone | 1 | 4 | 2 |
| Steam | Steam Washer | Impure Iron Dust | 1 | Water | 100 mL | Iron Dust | 1 | Stone | 1 | 4 | 2 |
| Steam | Steam Washer | Impure Sulfur Dust | 1 | Water | 100 mL | Sulfur Dust | 1 | Stone | 1 | 4 | 2 |

> Primary source of Stone in the game — Starter Chest provides bootstrap supply only.
> Future tiers: washing with chemical fluids (e.g. Sulfuric Acid, Mercury) yields additional rare byproducts instead of plain Stone.
> Two output pipes required; assign colors separately to route dust and Stone to different destinations.

---

### Chemical Reactor (4 L/s) — research unlock

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Chemical Reactor | Sulfur Dust | 4 | Water | 500 mL | Sulfuric Acid | 500 mL | 8 | 4 |
| Steam | Chemical Reactor | Copper Plate | 1 | Sulfuric Acid | 250 mL | Etched Copper Board | 1 | 6 | 4 |

> First machine to take a chemical fluid as input — introduces the fluid pipe layer for non-water fluids.
> 500 mL Sulfuric Acid etches 2 Copper Boards; requires a dedicated acid production line to scale.
> Sulfuric Acid simplified from real H₂SO₄ chemistry (S + H₂O) — intentional tutorial abstraction.

---

### Brick Furnace (4 L/s) — late Steam, LV gate

| Tier | Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | L/s |
|---|---|---|---|---|---|---|---|---|---|
| Steam | Brick Furnace | Iron Ingot | 4 | Coal | 2 | Steel Ingot | 1 | 16 | 4 |

> Slow by design. The cost is in building the Brick Furnace itself (16 Iron Ingot + 64 Stone).
> Producing the first Steel Ingot triggers the LV unlock research node.

---

## Critical Paths

### Bronze path (main Steam loop)
```
Copper Ore ──► Macerator ──► Impure Copper Dust ──► Washer ──► Copper Dust ──►
                                                                                Alloy Smelter ──► Bronze Ingot
Tin Ore    ──► Macerator ──► Impure Tin Dust    ──► Washer ──► Tin Dust    ──►
```
Washer also outputs Stone on each wash — route to a chest or into Brick Furnace build queue.

Manual bootstrap (before Macerator + Washer — uses pure dusts from Starter Chest):
```
Copper Dust (chest) + Tin Dust (chest) ──► Primitive Workbench ──► Bronze Dust ──► Primitive Furnace ──► Bronze Ingot
```

### LV unlock path
```
Iron Ore ──► Macerator ──► Iron Dust ──► Primitive Furnace ──► Iron Ingot ──► Steam Workbench ──► Brick Furnace
                                                                                                        │
                                                                              Iron Ingot ×4 + Coal ×2 ──► Steel Ingot ×1  ← LV UNLOCKED
```

### Primitive Circuit path (LV gate material)
```
Sulfur Ore ──► Macerator ──► Sulfur Dust ──►
                                              Chemical Reactor ──► Sulfuric Acid (mL)
                              Water      ──►                               │
                                                                           ▼
Copper Ore ──► Macerator ──► Copper Dust ──► Furnace ──► Copper Ingot ──► Compressor ──► Copper Plate ──► Chemical Reactor ──► Etched Copper Board
                                                                │                                                                        │
                                                                └──► Steam Workbench ──► Copper Wire ×4 ──────────────────────────────────┤
                                                                                                                                          ▼
                                                                                              Tin Ingot ×2 (solder) ──► Steam Workbench ──► Primitive Circuit
```

---

## Starter Chest Contents

| Item | Qty | Purpose |
|---|---|---|
| Coal | 32 | Fuel for Primitive Furnace + Boiler bootstrap |
| Copper Dust | 24 | Bronze Dust crafting (8 batches) |
| Tin Dust | 8 | Bronze Dust crafting (8 batches) |
| Stone | 32 | Machine crafting bootstrap + Stone Chest crafting |
| Item Pipe | 4 | First pipe connections before Copper Ingots available |
