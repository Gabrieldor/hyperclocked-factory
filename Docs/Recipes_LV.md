# LV Tier — Recipe Reference

All recipes for the LV tier. Balance values are placeholder — tune in playtesting.

Column key: **Machine | Input 1 | Qty | Input 2 | Qty | Output | Qty | Time (s) | W**  
Fluid quantities in mL unless noted as L (liters).

---

## Generators

### Steam Turbine

- **Input:** Steam, 64 L/s
- **Output:** 32W (1A at 32V)
- **Buffer:** 256 L internal steam buffer
- **Idle:** No steam → output stops, no damage

> 4 Boilers (16 L/s each) or 1 Steel Boiler (288 L/s, up to 4 turbines with steam to spare) required for full output.

---

### Solar Panel

- **Input:** None (passive)
- **Output:** 8W (0.25A at 32V)

> Stack 4 Solar Panels for 1A equivalent (32W). Craft from Silicon Wafer — available mid-LV after Electrolyzer is set up.

---

### Generator Comparison


| Generator     | Output           | Fuel         |
| ------------- | ---------------- | ------------ |
| Steam Turbine | 32W (1A @ 32V)   | 64 L/s steam |
| Solar Panel   | 8W (0.25A @ 32V) | None         |


---

## Steel Boiler (Multiblock)

Multiblock structure. Replaces and far outperforms the basic Boiler for dedicated steam production.

### Structure

- **Footprint:** 3×3 (9 tiles)
- **Tiles:** 1× Steel Boiler Controller + 8× Steel Boiler Casing
- **Layout:** Controller in any position; all 9 tiles must form a contiguous 3×3 block; all non-controller tiles must be Steel Boiler Casing
- **Valid:** Controller detects the 3×3 pattern on placement and activates; invalid structure = inactive

### Specs

- **Input:** Coal (via controller fuel slot) + Water (proximity or fluid pipe to any casing tile)
- **Output:** Steam via controller output port
- **Steam rate:** 288 L/s (18× standard Boiler)
- **Fuel:** 1 Coal = 8s (same per-coal rate as standard Boiler; just far more output per coal)
- **Water:** continuous draw, same rate scale-up as steam output
- **Idle:** No coal or no water → output stops, no damage

> 288 L/s supports 4 Steam Turbines (256W = 8A at 32V) with 32 L/s to spare for steam machines.

### Crafting

*(TBD — requires Steel Plate and Steel Ingot; defined when LV crafting materials finalized)*

---

## Machines


| Machine                | Replaces               | W   | Notes                                           |
| ---------------------- | ---------------------- | --- | ----------------------------------------------- |
| LV Electric Furnace    | Primitive Furnace      | 16W | Automated dust → ingot smelting                 |
| LV Macerator           | Steam Macerator        | 16W | Electric; faster                                |
| LV Compressor          | Steam Compressor       | 8W  | Electric; faster; also compacts Tiny Piles      |
| LV Extractor           | Steam Extractor        | 8W  | Electric; faster pull rate                      |
| LV Assembler           | Steam Workbench        | 8W  | Automated crafting; no player input required    |
| LV Alloy Smelter       | Steam Alloy Smelter    | 16W | Electric; faster                                |
| LV Chemical Reactor    | Steam Chemical Reactor | 16W | Electric; faster                                |
| LV Ore Washer          | Steam Washer           | 8W  | Electric; faster                                |
| Lathe                  | —                      | 16W | New; ingot → rods                               |
| Wiremill               | —                      | 16W | New; ingot → wires (2× yield vs Assembler)      |
| Electrolyzer           | —                      | 32W | New; Clay decomposition + Water splitting       |
| Magnetizer             | —                      | 32W | New; produces Magnetic Steel                    |
| Electric Blast Furnace | Brick Furnace          | 64W | New; high-temp alloys + GaAs Boule; steel 4× faster; multiblock 3×3 |


---

## Recipes

### LV Electric Furnace (16W)


| Input          | Qty | Output          | Qty | Time (s) |
| -------------- | --- | --------------- | --- | -------- |
| Copper Dust    | 1   | Copper Ingot    | 1   | 2        |
| Tin Dust       | 1   | Tin Ingot       | 1   | 2        |
| Bronze Dust    | 1   | Bronze Ingot    | 1   | 2        |
| Iron Dust      | 1   | Iron Ingot      | 1   | 2        |
| Lead Dust      | 1   | Lead Ingot      | 1   | 2        |
| Nickel Dust    | 1   | Nickel Ingot    | 1   | 2        |
| Gold Dust      | 1   | Gold Ingot      | 1   | 2        |
| Aluminium Dust | 1   | Aluminium Ingot | 1   | 2        |
| Invar Dust     | 1   | Invar Ingot     | 1   | 2        |


> Replaces Primitive Furnace — faster (2s vs 4s) and automated. Does not handle high-temp alloys (Steel, Aluminium); those go in the Electric Blast Furnace. Invar Dust is smelted here since it is a nickel-iron alloy that does not require blast temperatures.

---

### LV Macerator (16W)

Inherits all Steam Macerator recipes. New LV ore recipes:


| Input        | Qty | Output 1           | Qty | Output 2 | Qty | Time (s) |
| ------------ | --- | ------------------ | --- | -------- | --- | -------- |
| Gold Ore     | 1   | Impure Gold Dust   | 2   | —        | —   | 2        |
| Nickel Ore   | 1   | Impure Nickel Dust | 2   | —        | —   | 2        |
| Lead Ore     | 1   | Impure Lead Dust   | 2   | —        | —   | 2        |
| Cinnabar Ore | 1   | Cinnabar Dust      | 2   | —        | —   | 2        |
| Clay         | 1   | Clay Dust          | 2   | —        | —   | 2        |
| Coal         | 1   | Coal Dust          | 2   | —        | —   | 2        |


> Clay Dust is an intermediate for the Electrolyzer if raw Clay is unavailable. Clay → Clay Dust → Electrolyzer is equivalent to Clay → Electrolyzer directly.

---

### LV Compressor (8W)

Inherits all Steam Compressor recipes. New LV recipes:


| Input               | Qty | Output          | Qty | Time (s) |
| ------------------- | --- | --------------- | --- | -------- |
| Tiny Pile of X Dust | 9   | X Dust          | 1   | 4        |
| Aluminium Ingot     | 2   | Aluminium Plate | 1   | 4        |
| Gold Ingot          | 2   | Gold Plate      | 1   | 4        |
| Nickel Ingot        | 2   | Nickel Plate    | 1   | 4        |
| Lead Ingot          | 2   | Lead Plate      | 1   | 4        |
| Invar Ingot         | 2   | Invar Plate     | 1   | 4        |


> "Tiny Pile of X Dust → X Dust" applies to any tiny pile type (e.g. Gallium, Arsenic, etc.).

---

### LV Ore Washer (8W)

Inherits all Steam Washer recipes. New LV ore recipes:


| Input 1            | Qty | Input 2 | Qty    | Output 1    | Qty | Output 2 | Qty | Time (s) |
| ------------------ | --- | ------- | ------ | ----------- | --- | -------- | --- | -------- |
| Impure Gold Dust   | 1   | Water   | 100 mL | Gold Dust   | 1   | Stone    | 1   | 2        |
| Impure Nickel Dust | 1   | Water   | 100 mL | Nickel Dust | 1   | Stone    | 1   | 2        |


> Lead does not use the standard wash route — Impure Lead Dust requires Mercury in the Chemical Reactor to extract Arsenic as a byproduct.

---

### LV Assembler (8W)

Automated version of Steam Workbench. Inherits all Steam Workbench recipes.

#### LV Subcomponents

Intermediate parts required for machine blueprints (Workshop). All assembled here — no player input needed.

| Output | Qty | Input 1 | Qty | Input 2 | Qty | Input 3 | Qty | Input 4 | Qty | Time (s) |
|---|---|---|---|---|---|---|---|---|---|---|
| LV Motor | 1 | Magnetic Steel Rod | 2 | Steel Rod | 2 | Copper Wire | 4 | Steel Ring | 2 | 10 |
| LV Piston | 1 | LV Motor | 1 | Steel Plate | 2 | Steel Rod | 2 | Steel Ingot | 4 | 10 |
| LV Pump | 1 | LV Motor | 1 | Steel Ring | 2 | Bronze Plate | 1 | Fluid Pipe | 2 | 10 |
| Basic Circuit | 1 | Primitive Circuit | 2 | Gold Wire | 4 | Iron Plate | 1 | — | — | 15 |
| Basic Circuit | 3 | GaAs Boule | 1 | Gold Wire | 8 | Iron Plate | 2 | — | — | 20 |
| MV Circuit | 1 | Basic Circuit | 2 | Gold Wire | 8 | Nickel Plate | 2 | Silicon Dust | 4 | 30 |

> **Motor chain:** Steel Ingot → Lathe → Steel Rod; Steel Rod → Magnetizer → Magnetic Steel Rod; Steel Rod → Lathe → Steel Ring ×2; Copper Ingot → Wiremill → Copper Wire.
> **Piston chain:** LV Motor + Steel Ingot → Compressor → Steel Plate + Lathe → Steel Rod.
> **Pump chain:** LV Motor + Fluid Pipe (Bronze Ingot ×4 → Workbench → 4× Fluid Pipe) + Bronze Ingot → Compressor → Bronze Plate.
> **Basic Circuit (bootstrap):** Primitive Circuit ×2 + Gold Wire ×4 + Iron Plate → 1× Basic Circuit. Available as soon as the Assembler is running.
> **Basic Circuit (GaAs batch):** GaAs Boule ×1 + Gold Wire ×8 + Iron Plate ×2 → 3× Basic Circuit. Post-EBF efficiency upgrade — GaAs Boule is produced in the EBF (Gallium Dust ×16 + Arsenic Dust ×16, 60s).
> **MV Circuit chain:** Basic Circuit ×2 + Gold Wire ×8 + Nickel Plate ×2 + Silicon Dust ×4. Requires Electrolyzer (Silicon Dust) + Nickel ore chain + Wiremill at scale. 3× MV Circuit required to build the EBF controller.

#### Batteries

| Output | Qty | Input 1 | Qty | Input 2 | Qty | Time (s) |
|---|---|---|---|---|---|---|
| Battery Hull | 1 | Lead Plate | 2 | Iron Plate | 1 | 8 |
| Tier I Battery | 1 | Battery Hull | 1 | Sulfur Dust | 4 | 6 |
| Tier II Battery | 1 | Battery Hull | 1 | Sodium Dust | 4 | 6 |
| Tier III Battery | 1 | Battery Hull | 1 | Lithium Dust | 4 | 6 |

> **Battery Hull chain:** Lead Ore → Macerator → Impure Lead Dust → Chemical Reactor (+ Mercury) → Lead Dust → Furnace → Lead Ingot → Compressor → Lead Plate ×2 + Iron Plate → Assembler → Battery Hull.
> **Tier I:** Sulfur Dust (Steam-era Chemical Reactor byproduct or node). Matches 8K Ws capacity.
> **Tier II:** Sodium Dust from Clay Electrolyzer. Matches 16K Ws capacity.
> **Tier III:** Lithium Dust from Clay Electrolyzer. Matches 32K Ws capacity.

> Unlike the Steam Workbench, the Assembler runs fully unattended — pipe items in, receive output via pipe.

---

### LV Alloy Smelter (16W)

Inherits all Steam Alloy Smelter recipes. New LV recipes:


| Input 1   | Qty | Input 2       | Qty | Output             | Qty | Time (s) |
| --------- | --- | ------------- | --- | ------------------ | --- | -------- |
| Iron Dust | 4   | Nickel Dust   | 1   | Invar Dust         | 5   | 6        |
| Lead Dust | 3   | Antimony Dust | 1   | Battery Alloy Dust | 4   | 6        |


> Invar is an MV prerequisite material. Battery Alloy is used in LV battery housing.

---

### LV Chemical Reactor (16W)

Inherits all Steam Chemical Reactor recipes. New LV recipes:


| Input 1          | Qty    | Input 2 | Qty    | Output 1  | Qty     | Output 2     | Qty    | Time (s) |
| ---------------- | ------ | ------- | ------ | --------- | ------- | ------------ | ------ | -------- |
| Cinnabar Dust    | 1      | Oxygen  | 250 mL | Mercury   | 250 mL  | SO₂          | 250 mL | 6        |
| SO₂              | 500 mL | Oxygen  | 250 mL | H₂SO₄     | 1000 mL | —            | —      | 8        |
| Impure Lead Dust | 1      | Mercury | 100 mL | Lead Dust | 1       | Arsenic Dust | 1      | 6        |


> SO₂ + O₂ → H₂SO₄ requires Water input too; full recipe: 500 mL SO₂ + 250 mL O₂ + 500 mL H₂O → 1000 mL H₂SO₄.  
> This H₂SO₄ route gives 2× yield vs the Steam tier recipe (Sulfur Dust + Water).

---

### Electric Blast Furnace (64W) *(multiblock 3×3)*

Replaces Brick Furnace for all high-temperature processing. Steel production is 4× faster than Brick Furnace.

**Structure:** 1× EBF Controller + 8× EBF Casing. EBF Casing is made from Invar — see `Docs/Machine_Crafting.md`.

| Input 1                           | Qty | Input 2      | Qty | Output          | Qty | Time (s) |
| --------------------------------- | --- | ------------ | --- | --------------- | --- | -------- |
| Iron Dust                         | 4   | Coal Dust    | 1   | Steel Ingot     | 1   | 4        |
| Aluminium Dust                    | 1   | —            | —   | Aluminium Ingot | 1   | 8        |
| Gallium Dust                      | 16  | Arsenic Dust | 16  | GaAs Boule      | 1   | 60       |
| *(Stainless Steel TBD — MV gate)* |     |              |     |                 |     |          |

> Steel: 4s vs Brick Furnace's 16s — same inputs, 4× throughput. Brick Furnace becomes obsolete.
> Aluminium requires the EBF (660 °C melting point). Source: Clay → Electrolyzer → Aluminium Dust → EBF → Aluminium Ingot. MV gate material path.
> GaAs Boule (60s batch) feeds the efficient Basic Circuit recipe — run a batch unattended while the factory processes other materials.

---

### Electrolyzer (32W)


| Input     | Qty     | Output 1      | Qty    | Output 2 | Qty    | Time (s) |
| --------- | ------- | ------------- | ------ | -------- | ------ | -------- |
| Water     | 1000 mL | Oxygen        | 500 mL | Hydrogen | 500 mL | 8        |
| Clay Dust | 1       | *(see below)* |        |          |        | 8        |


**Clay Dust outputs** (player selects recipe in UI):


| Recipe           | Output         | Qty | Byproduct                 | Qty |
| ---------------- | -------------- | --- | ------------------------- | --- |
| Clay → Sodium    | Sodium Dust    | 1   | —                         | —   |
| Clay → Lithium   | Lithium Dust   | 1   | —                         | —   |
| Clay → Aluminium | Aluminium Dust | 1   | —                         | —   |
| Clay → Silicon   | Silicon Dust   | 1   | Tiny Pile of Gallium Dust | 1   |


> Gallium is always a byproduct of the Silicon recipe only. Compact 9× Tiny Pile of Gallium → 1× Gallium Dust via Compressor.

---

### Magnetizer (32W)


| Input       | Qty | Output               | Qty | Time (s) |
| ----------- | --- | -------------------- | --- | -------- |
| Steel Ingot | 1   | Magnetic Steel Ingot | 1   | 8        |
| Steel Rod   | 1   | Magnetic Steel Rod   | 1   | 8        |
| Iron Rod    | 1   | Magnetic Iron Rod    | 1   | 8        |

> Magnetic Steel Rod is the primary LV Motor input. Magnetic Steel Ingot is an earlier placeholder for pre-Motor Assembler recipes.

---

### Lathe (16W)


| Input           | Qty | Output           | Qty | Time (s) |
| --------------- | --- | ---------------- | --- | -------- |
| Copper Ingot    | 1   | Copper Rod       | 2   | 4        |
| Tin Ingot       | 1   | Tin Rod          | 2   | 4        |
| Iron Ingot      | 1   | Iron Rod         | 2   | 4        |
| Steel Ingot     | 1   | Steel Rod        | 2   | 4        |
| Bronze Ingot    | 1   | Bronze Rod       | 2   | 4        |
| Gold Ingot      | 1   | Gold Rod         | 2   | 4        |
| Nickel Ingot    | 1   | Nickel Rod       | 2   | 4        |
| Aluminium Ingot | 1   | Aluminium Rod    | 2   | 4        |
| Steel Rod       | 1   | Steel Ring       | 2   | 4        |
| Copper Rod      | 1   | Copper Ring      | 2   | 4        |

> 1 Ingot → 2 Rods. 1 Rod → 2 Rings. Rings are required for LV Motors and LV Pumps. Higher tiers will use rings of their own primary metal.

---

### Wiremill (16W)


| Input           | Qty | Output         | Qty | Time (s) |
| --------------- | --- | -------------- | --- | -------- |
| Copper Ingot    | 1   | Copper Wire    | 8   | 4        |
| Gold Ingot      | 1   | Gold Wire      | 8   | 4        |
| Aluminium Ingot | 1   | Aluminium Wire | 8   | 4        |


> 2× yield over the Steam Workbench/Assembler route (4 wire per ingot → 8). Dedicated Wiremill is required to scale wire production at LV.

---

