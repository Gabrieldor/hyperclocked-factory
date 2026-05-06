# LV Tier — Milestone Tree

Milestones trigger automatically the first time their condition is met. No currency spent.
Picks up immediately after Steam gate **G1** (Steel Ingot + Primitive Circuit produced).

---

## Bootstrap Note

Two circular dependencies exist in the LV component chain. Both are resolved by Workshop manual blueprints — expensive one-time builds that are replaced by the automated route once machines are running:

| Problem | Resolution |
|---|---|
| Lathe needs Steel Rod; Steel Rod comes from Lathe | Steam Workbench gains recipe: Steel Ingot ×2 → Steel Rod ×1 (1:1 ratio, half the Lathe yield — early bootstrap only) |
| Assembler needs LV Motor + LV Piston; Motor/Piston are made in the Assembler | Workshop gains manual blueprints at **B2**: LV Motor (Manual) and LV Piston (Manual) — same components, double the material cost, no Assembler required |

---

## Trunk (linear, must complete in order)

| # | Condition | Unlocks |
|---|---|---|
| L0 | LV Tier unlocked (from G1) | Workshop gains all LV blueprints; Steam Turbine + LV Electric Furnace + LV Alloy Smelter + Wiremill available |
| L1 | Build Steam Turbine | First electric power → LV Macerator + LV Compressor blueprints; Steel Boiler blueprint |
| L2 | Build Wiremill + produce Copper Wire | **Branch A** + **Branch B** unlock; floor expansion 40×40 |

> **L1 rationale:** Steam Turbine is the cheapest first generator — 4 Steel + 4 Iron Plate + 1 Motor + 1 Circuit + 8 Fluid Pipe. The LV Motor here still uses the Steam age Magnetic Steel Ingot route (Assembler: Iron Plate ×2 + Copper Wire ×4 + Magnetic Steel Ingot ×1) as a temporary recipe before the proper Motor chain is unlocked.

---

## Branch A — Chemistry / Fluid path

Unlocked by: **Build Wiremill (L2)**

Focus: LV chemical processing, fluid handling, Aluminium Dust production.

| # | Condition | Unlocks |
|---|---|---|
| A1 | Build LV Chemical Reactor | Cinnabar → Mercury recipe active; Electrolyzer blueprint |
| A2 | Produce Mercury | Lead Dust chain active (Impure Lead + Mercury → Lead + Arsenic); LV Ore Washer blueprint |
| A3 | Build Electrolyzer | Clay decomposition active → Aluminium Dust, Silicon Dust, Gallium byproduct |
| A4 | Produce Aluminium Dust | **Branch A complete** — feeds LV Gate |

> **A1 rationale:** LV Chemical Reactor needs a Pump, which needs a Motor — at this point the player still uses the temporary Motor recipe. The pump dependency pushes Chemistry behind Wiremill.
> **A2 side chain:** Arsenic Dust (Lead byproduct) + Gallium Dust (Clay byproduct) → GaAs Boule in EBF later.
> **A3 note:** Electrolyzer also splits Water → Oxygen + Hydrogen. Oxygen feeds the Cinnabar roasting chain (Cinnabar + O₂ → Mercury + SO₂ → H₂SO₄). Full chemical loop closes here.

---

## Branch B — Component / Assembly path

Unlocked by: **Build Wiremill (L2)**

Focus: LV subcomponent chain, Motor/Piston production, LV Assembler.

| # | Condition | Unlocks |
|---|---|---|
| B1 | Build LV Compressor + produce Steel Plate | Lathe blueprint; Magnetizer blueprint; Steam Workbench gains Steel Rod bootstrap recipe |
| B2 | Build Lathe + produce Steel Rod | Magnetizer blueprint confirmed active; Workshop gains: **LV Motor (Manual)** + **LV Piston (Manual)** blueprints |
| B3 | Build Magnetizer + produce Magnetic Steel Rod | LV Motor automated chain ready (all ingredients now producible) |
| B4 | Build LV Assembler + produce first LV Motor (Assembler route) | LV Extractor + LV Ore Washer + Centrifuge blueprints; floor expansion 44×44; **Branch B complete** |

> **B1 note:** Steel Plate (Steel Ingot ×2 → LV Compressor → Steel Plate ×1) gates most of the LV blueprint tree — it appears in almost every machine recipe. This is the first major "unlock felt" moment of LV.
> **B2 manual blueprints:** LV Motor (Manual) = Magnetic Steel Rod ×4 + Steel Ingot ×4 + Copper Wire ×8 + Steel Ring ×4 (double cost); LV Piston (Manual) = LV Motor ×1 + Steel Plate ×4 + Steel Ingot ×8 (double cost). These exist only to break the Assembler bootstrap loop.
> **B4 rationale:** Once the Assembler is running the real Motor/Piston chain, the temporary manual recipes become obsolete. Centrifuge and Extractor unlock here because both require Motor + Pump (pump now producible via Assembler).

---

## LV Gate (requires both branches)

| # | Condition | Unlocks |
|---|---|---|
| G2 | Produce Aluminium Dust (A4) **AND** build LV Assembler (B4) | Basic Circuit recipe unlocked; MV Circuit recipe unlocked |
| G3 | Produce 3× MV Circuit | Electric Blast Furnace blueprint; Solar Panel blueprint |
| G4 | Build Electric Blast Furnace | Steel 4× faster; Silicon Boule + GaAs Boule paths active; Aluminium Ingot recipe active |
| G5 | Produce first Aluminium Ingot | **MV tier unlocked** + floor expansion 48×48 |

> **G2→G3:** MV Circuit = Basic Circuit ×2 + Gold Wire ×8 + Nickel Plate ×2 + Silicon Dust ×4 (30s, LV Assembler). Expensive — producing 3 of them is a deliberate grind that forces the player to have ore processing, Electrolyzer, and Wiremill all running at scale.
> **G3 note:** MV Circuit is crafted entirely at LV; no MV machines required. It is "MV-tier" in name because it is the circuit used in MV machine blueprints, not because it requires MV infrastructure to produce.
> **G4→G5 gap:** EBF runs Aluminium Dust (1 → 1 Ingot, 8s) immediately on completion. The gap is short — building the EBF is the hard part.

---

## Optional milestones (no gate dependency)

| # | Condition | Unlocks |
|---|---|---|
| OPT_L1 | Build Steel Boiler (multiblock 3×3) | Dedicated steam upgrade — 288 L/s feeds 4 Steam Turbines simultaneously |
| OPT_L2 | Produce Invar Dust (Iron + Nickel, LV Alloy Smelter) | MV material preview; Invar has no LV use — signals upcoming tier |
| OPT_L3 | Produce GaAs Boule (Gallium + Arsenic, EBF) | Semiconductor path preview; GaAs is an MV circuit component |
| OPT_L4 | Build Solar Panel | Passive power flex; requires Silicon Boule (EBF batch) — late LV achievement |

---

## Full tree diagram

```
[L0] LV Unlocked (from G1)
       │
       ▼
[L1] Build Steam Turbine — first electric power
       │
       ▼
[L2] Build Wiremill + Copper Wire ──── floor 40×40
       │
       ├─────────────────────────┬──────────────────────────────
       │                         │
       ▼                         ▼
  BRANCH A                  BRANCH B
  (Chemistry/Fluid)         (Components/Assembly)
       │                         │
[A1] Build LV               [B1] Build LV Compressor
     Chemical Reactor              + Steel Plate
       │                         │
[A2] Produce Mercury         [B2] Build Lathe
       │                         │── Workshop: Motor/Piston (manual)
[A3] Build Electrolyzer      [B3] Build Magnetizer
       │                         │── Magnetic Steel Rod
[A4] Aluminium Dust          [B4] Build LV Assembler
       │                         │── LV Motor (Assembler route)
       │                         │── floor 44×44
       └───────────┬─────────────┘
                   │
                  [G2] Basic Circuit + MV Circuit recipes unlocked
                   │
                  [G3] Produce 3× MV Circuit
                   │    └── EBF blueprint unlocked
                  [G4] Build Electric Blast Furnace
                   │
                  [G5] First Aluminium Ingot
                   │
              MV TIER UNLOCKED
              + Floor 48×48
```

---

## Notes

- Branch A and Branch B are independent after L2 — both can progress simultaneously.
- G2 is a hard AND: Aluminium Dust production (A4) and LV Assembler built (B4) both required before Basic/MV Circuit recipes unlock.
- G3 (3× MV Circuit) is the true difficulty wall of LV — it demands Electrolyzer, Wiremill, Nickel ore chain, and Basic Circuit production all running simultaneously.
- MV Circuit is crafted entirely at LV. "MV" refers to its circuit tier, not the machines needed to produce it.
- The manual Motor/Piston Workshop blueprints (B2) are intentionally expensive — a one-time tax to break the bootstrap loop, not a permanent shortcut.
- OPT milestones have no prerequisites and do not block progress.
- Floor sizes: 32×32 (Steam gate) → 40×40 (L2) → 44×44 (B4) → 48×48 (G5/MV gate). Final 64×64 unlocked at HV.
