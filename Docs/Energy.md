# Energy System

## Voltage Tiers

| Tier | Voltage | Typical recipe cost |
|---|---|---|
| LV | 32 V | 8–64 W |
| MV | 128 V | 32–256 W |
| HV | 512 V | 128–1024 W |

Each tier is ×4 the previous voltage.

---

## Power Unit: Watts (W)

**W = V × A**

Machines draw W per tick from the cable network. Cables carry W up to their capacity (voltage × amperage). There is no rounding — machines draw exactly their recipe W cost per tick.

Example: 1A LV cable = 32W capacity. Two machines drawing 8W each = 16W total — cable handles both with 16W headroom.

---

## Cables

Cables are **lossless** — no energy lost over distance.

Each cable has a **voltage tier** and an **amperage rating**. These define its W capacity:

| Amperage | LV (32V) | MV (128V) | HV (512V) |
|---|---|---|---|
| 1A | 32W | 128W | 512W |
| 4A | 128W | 512W | 2048W |
| 8A | 256W | 1024W | 4096W |

Cable materials (which material maps to which tier/amperage) are defined in `Docs/Recipes_LV.md` and equivalent tier recipe docs.

---

## Destruction Rules

### Overvolt — cable destroyed
If a generator's output voltage exceeds the cable's voltage tier, the cable tile is destroyed instantly.

- LV Generator (32V) → MV cable (128V): safe
- MV Generator (128V) → LV cable (32V): **LV cable destroyed**

### Overvolt — machine destroyed
If the cable's voltage tier exceeds the machine's rated voltage tier, the machine tile is destroyed instantly.

- LV cable (32V) → LV machine: safe
- MV cable (128V) → LV machine: **LV machine destroyed**

### Overcurrent — cable burnt
If the total W demand on a cable segment exceeds its W capacity, the cable tile transitions to a **burnt state**.

- Burnt cable is non-functional and breaks the circuit at that segment.
- Burn does **not** propagate to adjacent cable segments.
- To repair: remove the burnt tile and place a new cable.

---

## Overclocking

Overclocking is achieved by running a recipe in a **higher-tier machine** — not by feeding higher voltage to a lower-tier machine.

| Machine tier vs recipe tier | W cost | Speed |
|---|---|---|
| Same tier | 1× (base) | 1× |
| 1 tier above | 4× | 2× |
| 2 tiers above | 16× | 4× |

Example: LV Macerator recipe costs 16W at 10s. Run the same recipe in an MV Macerator → 64W at 5s.

Feeding a cable voltage higher than a machine's rated tier does **not** overclock it — it destroys the machine.

---

## Battery Buffer

The Battery Buffer is a 1×1 machine that acts as both an **energy buffer** and a **battery charger**.

- Voltage-locked: an LV Battery Buffer accepts only LV batteries; MV buffer accepts only MV batteries.
- Holds up to **8 battery slots**.
- Each filled slot contributes **1A** of output at the buffer's voltage tier.
- Full LV Battery Buffer (8 slots) → 8A × 32V = **256W output**.
- The buffer draws from the network to charge inserted batteries when machine demand is below network supply.
- The buffer outputs to the network when generator supply drops below machine demand.

### Battery Tiers

Three battery tiers exist per voltage tier (materials TBD):

| Battery tier | Capacity |
|---|---|
| I | 8,000 Ws |
| II | 16,000 Ws |
| III | 32,000 Ws |

Mixed battery tiers are allowed in the same buffer. Total storage = sum of all inserted batteries.

Full LV Battery Buffer examples:

| Batteries inserted | Output | Total storage |
|---|---|---|
| 8× Tier I | 256W | 64,000 Ws |
| 8× Tier II | 256W | 128,000 Ws |
| 8× Tier III | 256W | 256,000 Ws |

---

## Recipe Energy Costs

All recipe energy costs are expressed in **W** (watts consumed per second of processing).

A recipe running in a machine of its native tier draws exactly its listed W cost per tick. Overclocking (higher-tier machine) multiplies cost by 4× per tier step and halves processing time per tier step.
