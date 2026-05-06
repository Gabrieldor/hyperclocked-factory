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

| Component | Machine | Input 1 | Qty | Input 2 | Qty | Input 3 | Qty | Input 4 | Qty | Out |
|---|---|---|---|---|---|---|---|---|---|---|
| Steel Ring | Lathe | Steel Rod | 1 | — | — | — | — | — | — | 2 |
| Magnetic Steel Rod | Magnetizer | Steel Rod | 1 | — | — | — | — | — | — | 1 |
| LV Motor | LV Assembler | Magnetic Steel Rod | 2 | Steel Rod | 2 | Copper Wire | 4 | Steel Ring | 2 | 1 |
| LV Piston | LV Assembler | LV Motor | 1 | Steel Plate | 2 | Steel Rod | 2 | Steel Ingot | 4 | 1 |
| LV Pump | LV Assembler | LV Motor | 1 | Steel Ring | 2 | Bronze Plate | 1 | Fluid Pipe | 2 | 1 |
| Basic Circuit | LV Assembler | Primitive Circuit | 2 | Gold Wire | 4 | Iron Plate | 1 | — | — | 1 |
| MV Circuit | LV Assembler | Basic Circuit | 2 | Gold Wire | 8 | Nickel Plate | 2 | Silicon Dust | 4 | 1 |
| Steel Plate | LV Compressor | Steel Ingot | 2 | — | — | — | — | — | — | 1 |

> **Circuit tiers (all crafted at LV):** Primitive Circuit (Steam age, current LV Circuit) → Basic Circuit (LV Assembler, more efficient LV Circuit) → MV Circuit (LV Assembler, expensive; required to build EBF controller ×3).
> **LV Circuit slot:** machine blueprints accept Primitive Circuit or Basic Circuit interchangeably. MV Circuit is a separate, higher tier used only in EBF and future MV machine blueprints.
> **Motor chain:** Steel Ingot → Lathe → Steel Rod; Rod → Magnetizer → Magnetic Steel Rod; Rod → Lathe → Steel Ring ×2; Copper Ingot → Wiremill → Copper Wire.
> **Piston chain:** LV Motor + Steel Ingot → Compressor → Steel Plate; Lathe → Steel Rod.
> **Pump chain:** LV Motor + Fluid Pipe (Bronze Ingot ×4 → Workbench → 4) + Bronze Ingot → Compressor → Bronze Plate.

---

### LV Machine Blueprints

Materials: Steel Ingot, Iron Plate, Copper Wire, LV Motor, LV Piston, LV Pump, LV Circuit (Primitive Circuit now; Basic Circuit TBD), Fluid Pipe, Steel Rod.

| Machine | Steel Ingot | Iron Plate | Cu Wire | Motor | Piston | Pump | Circuit | Other |
|---|---|---|---|---|---|---|---|---|
| LV Electric Furnace | 4 | 2 | 4 | — | — | — | 1 | — |
| LV Macerator | 4 | 2 | — | 1 | — | — | 1 | — |
| LV Compressor | 4 | 2 | — | — | 1 | — | 1 | — |
| LV Extractor | 4 | 2 | 4 | 1 | — | — | 1 | — |
| LV Assembler | 4 | 4 | 4 | 1 | 1 | — | 2 | — |
| LV Alloy Smelter | 4 | 2 | 4 | — | — | — | 1 | — |
| LV Chemical Reactor | 4 | 2 | — | — | — | 1 | 2 | Fluid Pipe ×8 |
| LV Ore Washer | 4 | 2 | — | — | — | 1 | 1 | Fluid Pipe ×4 |
| Lathe | 4 | 2 | — | 1 | — | — | 1 | Steel Rod ×4 |
| Wiremill | 4 | 2 | 4 | 1 | — | — | 1 | — |
| Electrolyzer | 4 | 4 | 8 | — | — | 1 | 2 | — |
| Magnetizer | 4 | 2 | 8 | — | — | — | 1 | — |
| Electric Blast Furnace *(Controller)* | 8 | 4 | 8 | — | 2 | — | 3× MV Circuit | Steel Rod ×4 |
| Steam Turbine | 4 | 4 | — | 1 | — | — | 1 | Fluid Pipe ×8 |
| Solar Panel | — | 2 | 4 | — | — | — | 1 | *(TBD — Silicon source pending)* |

> **Motor** drives rotating/grinding mechanisms. **Piston** handles compression/linear motion. **Pump** handles fluid-touching machines.
> **Electric Blast Furnace** is a multiblock (3×3): Controller + 8× EBF Casing. Controller cost listed above; see Multiblock section for casing recipe.
> **EBF Controller** requires 3× MV Circuit — crafted at LV in the Assembler (Basic Circuit ×2 + Gold Wire ×8 + Nickel Plate ×2 + Silicon Dust ×4). Expensive by design; gates the Aluminium → MV path.
> **Steel Rod:** Steel Ingot → Lathe → Steel Rod ×2. Lathe must be built before machines that require Steel Rod as a blueprint ingredient.
> **Steam Turbine** is early LV — cheapest way to bridge Steam infrastructure into the LV power network.

---

### LV Multiblock Components

#### Steel Boiler

| Component | Steel Plate | Steel Ingot | Iron Ingot | LV Circuit | Fluid Pipe | Qty needed |
|---|---|---|---|---|---|---|
| Steel Boiler Casing | 4 | — | 2 | — | — | 8 |
| Steel Boiler Controller | 8 | — | — | 1 | 4 | 1 |

**Full Steel Boiler total:** 32× Steel Plate, 16× Iron Ingot, 1× LV Circuit, 4× Fluid Pipe.

#### Electric Blast Furnace

| Component | Invar Plate | Steel Ingot | LV Circuit | Qty needed |
|---|---|---|---|---|
| EBF Casing | 4 | 2 | — | 8 |
| EBF Controller | — | — | — | 1 *(see machine blueprints above)* |

**Full EBF total (casings only):** 32× Invar Plate, 16× Steel Ingot — plus the Controller cost.

> **Invar chain:** Iron Dust ×4 + Nickel Dust ×1 → LV Alloy Smelter → Invar Dust ×5 → LV Electric Furnace → Invar Ingot → LV Compressor → Invar Plate (×2 ingots per plate). Invar is available before the EBF — no circular dependency.

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
| Magnetic Steel Rod | Magnetizer (Steel Rod ×1 → 1) |
| Steel Ring | Lathe (Steel Rod ×1 → Steel Ring ×2) |
| LV Motor | LV Assembler (Magnetic Steel Rod ×2 + Steel Rod ×2 + Copper Wire ×4 + Steel Ring ×2) |
| LV Piston | LV Assembler (LV Motor ×1 + Steel Plate ×2 + Steel Rod ×2 + Steel Ingot ×4) |
| LV Pump | LV Assembler (LV Motor ×1 + Steel Ring ×2 + Bronze Plate ×1 + Fluid Pipe ×2) |
| Invar Dust | LV Alloy Smelter (Iron Dust ×4 + Nickel Dust ×1 → Invar Dust ×5) |
| Invar Ingot | LV Electric Furnace (Invar Dust ×1 → 1) |
| Invar Plate | LV Compressor (Invar Ingot ×2 → 1) — used in EBF Casing ×4 per casing |
| GaAs Boule | Electric Blast Furnace (Gallium Dust ×16 + Arsenic Dust ×16, 60s) |
| Primitive Circuit | Steam Workbench (Etched Copper Board + Copper Wire) — LV Circuit tier 1 |
| Basic Circuit | LV Assembler: Primitive Circuit ×2 + Gold Wire ×4 + Iron Plate → 1×; or GaAs Boule ×1 + Gold Wire ×8 + Iron Plate ×2 → 3× (post-EBF) |
| MV Circuit | LV Assembler (Basic Circuit ×2 + Gold Wire ×8 + Nickel Plate ×2 + Silicon Dust ×4) — required for EBF controller ×3 |
| Silicon Boule | Electric Blast Furnace (Silicon Dust ×64, 60s) |
| Fluid Pipe | Steam Workbench (Bronze Ingot ×4 → 4) |
