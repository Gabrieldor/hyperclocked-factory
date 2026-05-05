# Machine Crafting — Workshop Blueprint Reference

All machines and multiblock components are crafted via the **Workshop**. Crafting is instant — resources are consumed from the Workshop's internal storage (54 slots) and the machine goes to the player's hotbar.

Blueprint availability expands via milestones: Steam blueprints are available from Workshop activation; LV blueprints unlock at the LV tier transition.

---

## Steam Age

### Bootstrap — Primitive Workbench Only

These are the only items crafted outside the Workshop. The Primitive Workbench is consumed when the Workshop activates.

| Output | Input 1 | Qty | Input 2 | Qty |
|---|---|---|---|---|
| Bronze Dust | Copper Dust | 3 | Tin Dust | 1 |
| Item Pipe | Copper Ingot | 2 | — | — |
| Stone Chest | Stone | 8 | — | — |
| Workshop Frame | Bronze Ingot | 2 | Stone | 4 |
| Workshop Controller | Bronze Plate | 8 | Iron Ingot | 8 |

> Bronze Plate is produced in the Primitive Furnace from Bronze Ingot... wait — Bronze Plate requires the Steam Compressor. Adjust Workshop Controller cost if the Compressor isn't yet built. Placeholder: may swap to Iron Plate if balance requires.

---

### Steam Machine Blueprints

All costs are Workshop inputs (drawn from internal storage). Materials: Bronze Ingot, Bronze Plate, Iron Ingot, Stone, Fluid Pipe.

| Machine | Bronze Ingot | Bronze Plate | Iron Ingot | Stone | Fluid Pipe | Notes |
|---|---|---|---|---|---|---|
| Primitive Furnace | — | — | — | 8 | — | |
| Boiler | 4 | — | — | 4 | — | |
| Alloy Smelter | 6 | — | — | — | — | |
| Steam Workbench | 16 | — | — | — | — | |
| Steam Macerator | 8 | — | — | 4 | — | Milestone unlock |
| Steam Compressor | 4 | — | — | 4 | — | Milestone unlock |
| Steam Extractor | 6 | — | — | — | — | Milestone unlock |
| Steam Washer | 6 | — | — | — | 2 | Milestone unlock |
| Chemical Reactor | 8 | — | — | — | 4 | Milestone unlock |
| Brick Furnace | — | — | 16 | 64 | — | Late Steam; LV gate |

---

### Steam Multiblock Components

| Component | Bronze Ingot | Bronze Plate | Iron Ingot | Stone | Notes |
|---|---|---|---|---|---|
| Workshop Frame | 2 | — | — | 4 | ×8 needed for full Workshop |
| Workshop Controller | — | 8 | 8 | — | ×1 needed |

**Full Workshop total:** 16× Bronze Ingot, 32× Stone, 8× Bronze Plate, 8× Iron Ingot.

---

## LV Tier

### LV Components

Intermediate items produced in factory machines (not Workshop). Required as ingredients in LV machine blueprints.

| Component | Machine | Input 1 | Qty | Input 2 | Qty | Input 3 | Qty | Output Qty |
|---|---|---|---|---|---|---|---|---|
| LV Motor | LV Assembler | Iron Plate | 2 | Copper Wire | 4 | Magnetic Steel Ingot | 1 | 1 |
| Basic Circuit | LV Assembler | Primitive Circuit | 2 | Gold Wire | 4 | Iron Plate | 1 | 1 |
| Steel Plate | LV Compressor | Steel Ingot | 2 | — | — | — | — | 1 |

> **LV Motor chain:** Iron Ore → Macerator → Washer → Furnace → Iron Plate (Compressor); Steel Ingot → Magnetizer → Magnetic Steel Ingot; Copper Ingot → Wiremill → Copper Wire.
> **Basic Circuit chain:** Primitive Circuit ×2 (Steam age); Gold Ore → Macerator → Washer → Furnace → Gold Ingot → Wiremill → Gold Wire ×8; Iron Plate from Compressor.

---

### LV Machine Blueprints

Materials: Steel Ingot, Iron Plate, Copper Wire, LV Motor, Basic Circuit, Fluid Pipe, Steel Rod.

| Machine | Steel Ingot | Iron Plate | Copper Wire | LV Motor | Basic Circuit | Other |
|---|---|---|---|---|---|---|
| LV Electric Furnace | 4 | 2 | 4 | — | — | — |
| LV Macerator | 4 | 2 | 4 | 1 | — | — |
| LV Compressor | 4 | 2 | — | 1 | — | — |
| LV Extractor | 4 | 2 | 4 | 1 | — | — |
| LV Assembler | 4 | 4 | 4 | 1 | 1 | — |
| LV Alloy Smelter | 4 | 2 | 4 | — | — | — |
| LV Chemical Reactor | 4 | 4 | — | — | 1 | Fluid Pipe ×8 |
| LV Ore Washer | 4 | 2 | — | 1 | — | Fluid Pipe ×4 |
| Lathe | 4 | 2 | — | 1 | — | Steel Rod ×4 |
| Wiremill | 4 | 2 | 4 | 1 | — | — |
| Electrolyzer | 4 | 4 | 8 | — | 1 | — |
| Magnetizer | 4 | 2 | 8 | — | — | — |
| Electric Blast Furnace | 8 | 4 | 8 | — | 1 | — |
| Centrifuge | 4 | 4 | — | 1 | — | Steel Rod ×4 |
| Steam Turbine | 4 | 4 | — | — | — | Fluid Pipe ×8 |
| Solar Panel | — | 2 | 4 | — | — | Silicon Boule ×1 |

> **Steel Rod:** Steel Ingot → Lathe → Steel Rod ×2. Lathe must be built before machines that require Steel Rod as an ingredient.
> **Solar Panel** is mid-to-late LV: Silicon Boule requires Clay → Electrolyzer → Silicon Dust → Electric Blast Furnace → Silicon Boule (60s batch).
> **Steam Turbine** is early LV — relatively cheap to bridge Steam infrastructure into the LV power network.

---

### LV Multiblock Components

| Component | Steel Plate | Steel Ingot | Iron Ingot | Basic Circuit | Fluid Pipe | Qty needed |
|---|---|---|---|---|---|---|
| Steel Boiler Casing | 4 | — | 2 | — | — | 8 |
| Steel Boiler Controller | 8 | — | — | 1 | 4 | 1 |

**Full Steel Boiler total:** 32× Steel Plate, 16× Iron Ingot, 1× Basic Circuit, 4× Fluid Pipe.

> Steel Plate: Steel Ingot ×2 → LV Compressor → Steel Plate ×1.

---

## Material Reference

| Material | Source |
|---|---|
| Bronze Ingot | Alloy Smelter (Copper + Tin) or Primitive Furnace (Bronze Dust) |
| Bronze Plate | Steam Compressor (Bronze Ingot ×2 → 1) |
| Iron Ingot | Primitive Furnace or LV Electric Furnace (Iron Dust) |
| Iron Plate | LV Compressor (Iron Ingot ×2 → 1) |
| Steel Ingot | Electric Blast Furnace (Iron Dust ×4 + Coal Dust ×1, 4s) |
| Steel Plate | LV Compressor (Steel Ingot ×2 → 1) |
| Steel Rod | Lathe (Steel Ingot ×1 → Steel Rod ×2) |
| Copper Wire | Wiremill (Copper Ingot ×1 → 8) or Steam Workbench / Assembler (×4) |
| Gold Wire | Wiremill (Gold Ingot ×1 → 8) |
| Magnetic Steel Ingot | Magnetizer (Steel Ingot ×1 → 1) |
| LV Motor | LV Assembler (see LV Components above) |
| Basic Circuit | LV Assembler (see LV Components above) |
| Primitive Circuit | Steam Workbench (Etched Copper Board + Copper Wire) |
| Silicon Boule | Electric Blast Furnace (Silicon Dust ×64, 60s) |
| Fluid Pipe | Steam Workbench (Bronze Ingot ×4 → 4) |
