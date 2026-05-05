# Steam Tier — Milestone Tree

Milestones trigger automatically the first time their condition is met. No currency spent.
The Milestone Tree UI always shows locked (gray) / available (lit) / unlocked (green) state.

---

## Trunk (linear, must complete in order)

| # | Condition | Unlocks |
|---|---|---|
| S0 | Game start (free) | Primitive Workbench, Primitive Furnace, Boiler, Steam Extractor |
| S1 | Produce first Bronze Ingot | Alloy Smelter recipe + Steam Workbench recipe |
| S2 | Build Steam Workbench | Floor expansion 20×20, Steam Macerator, **Branch A**, **Branch B** |

---

## Branch A — Iron / Steel path

Unlocked by: **Build Steam Workbench (S2)**

| # | Condition | Unlocks |
|---|---|---|
| A1 | Produce Impure Dust (any ore) | Steam Washer + Brick Furnace recipe |

---

## Branch B — Chemistry / Circuit path

Unlocked by: **Build Steam Workbench (S2)**

| # | Condition | Unlocks |
|---|---|---|
| B1 | Produce Bronze Plate | Steam Compressor + Chemical Reactor recipe |
| B2 | Build Chemical Reactor | Sulfur Ore node appears on floor |
| B3 | Produce Sulfuric Acid | Primitive Circuit recipe |

---

## LV Gate (requires both branches)

| # | Condition | Unlocks |
|---|---|---|
| G1 | Produce Steel Ingot **AND** Primitive Circuit | **LV tier unlocked** + floor expansion 32×32 |

---

## Optional upgrades (no gate dependency)

| # | Condition | Unlocks |
|---|---|---|
| OPT1 | Produce N× Bronze Ingot *(TBD count)* | Steam Extractor rate upgrade ×2 |

---

## Full tree diagram

```
[S0] Start
       │
       ▼
[S1] Bronze Ingot ──────────────────────────────────────►
       │
       ▼
[S2] Build Steam Workbench
       │
       ├─────────────────────┬─────────────────────────────
       │                     │
       ▼                     ▼
  BRANCH A               BRANCH B
  (Iron/Steel)           (Chemistry/Circuit)
       │                     │
[A1] Impure Dust        [B1] Bronze Plate
       │                     │
       ├── Steam Washer  [B2] Build Chemical Reactor
       │                     │── Sulfur Ore node unlocked
       └── Brick Furnace      │
               │         [B3] Sulfuric Acid
               ▼               │
         Steel Ingot      Primitive Circuit
               │               │
               └───────┬───────┘
                        │
                       [G1] LV TIER UNLOCKED
                        + Floor expansion 32×32
```

---

## Notes

- Branch A and Branch B are independent — players can work both simultaneously after S2.
- The LV gate is a hard AND: both Steel Ingot and Primitive Circuit must be produced.
- Optional milestones (OPT) have no prerequisites and do not block progress.
- RP counts marked TBD are tuned in playtesting.
