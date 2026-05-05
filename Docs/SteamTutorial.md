# Steam Age — Tutorial & Bootstrapping Flow

## Design Intent

The Steam Age serves as a hands-on tutorial. Every step teaches a core mechanic:
- Pipe routing (Bronze Dust → Furnace)
- Machine placement
- Machine-to-machine item flow
- Proximity-based fluid intake (Boiler next to Water Node)
- Fluid pipes vs item pipes distinction
- Machine upgrade / replacement (Primitive → Steam Workbench)

The player never reads a wall of text. They learn by doing each step to progress.

---

## Starting State

When a new game begins, the factory floor has two pre-placed objects:

### Starter Chest
A fixed chest that cannot be moved or removed. Contains:
| Item | Quantity | Purpose |
|---|---|---|
| Coal | 32 | Fuel for Boiler later |
| Copper Dust | 24 | Bronze production |
| Tin Dust | 8 | Bronze production |
| Stone | 16 | Craft first Furnace |

> **Why dusts instead of ores?** Skipping the ore → dust step removes the need to teach the Macerator in the first five minutes. The player learns one mechanic at a time. Macerating is introduced later as "how you get more resources".

### Water Node
A pre-placed, infinite water source on the factory floor. Cannot be moved or removed. Provides water to any Boiler placed within 1 tile of it (no pipe required). Once the player unlocks **Fluid Pipes**, water can be routed freely at any distance.

The Water Node is visually distinct from ore nodes — it uses an animated water tile rather than a static ore icon.

### Primitive Workbench
A pre-placed machine. It has:
- Input slots (receives items from pipes or directly from the Starter Chest)
- A recipe selector (tap to choose a recipe from the available list)
- An output slot (sends finished items into the pipe network)
- No power requirement — it processes instantly (or near-instantly)

---

## Tutorial Flow

### Step 1 — Make Bronze Dust
**Teaches:** Using a machine, selecting a recipe.

The player taps the Primitive Workbench and selects the **Bronze Dust** recipe.

```
Workbench recipe:
  3× Copper Dust + 1× Tin Dust → 1× Bronze Dust
```

The Workbench pulls Copper Dust and Tin Dust from the Starter Chest (they are connected by default at game start with a short pipe segment — the tutorial highlights this connection).

Output: Bronze Dust is placed into the Workbench's output slot.

---

### Step 2 — Craft a Furnace
**Teaches:** Crafting a machine, placing it on the floor.

Player selects the **Primitive Furnace** recipe in the Workbench:

```
Workbench recipe:
  8× Stone → 1× Primitive Furnace
```

Output: A Primitive Furnace item appears in the Workbench's output slot. The tutorial prompts the player to pick it up and place it on the floor.

---

### Step 3 — Route Bronze Dust to the Furnace
**Teaches:** Pipe placement, machine-to-machine routing — the core mechanic of the game.

The tutorial prompts the player to:
1. Place a pipe segment from the Workbench output to the Furnace input
2. Watch Bronze Dust travel through the pipe into the Furnace

```
Workbench [output] ──pipe──► Furnace [input]
```

The Furnace smelts Bronze Dust → Bronze Ingot.

```
Furnace recipe:
  1× Bronze Dust → 1× Bronze Ingot
```

Output slot of the Furnace now holds Bronze Ingots.

---

### Step 4 — Route Bronze Ingots Back to the Workbench
**Teaches:** Multi-machine pipe chains, closing a loop.

Player pipes the Furnace output back to the Workbench input (or to a buffer chest if one is introduced here).

```
Furnace [output] ──pipe──► Workbench [input]
```

Now the Workbench has Bronze Ingots available to craft the next tier of objects.

---

### Step 5 — Craft the Boiler and Alloy Smelter
**Teaches:** Unlocking machines, building the first real production setup.

With Bronze Ingots available, the player crafts two machines in the Workbench:

```
Workbench recipe:
  8× Stone + 4× Bronze Ingot → 1× Boiler

Workbench recipe:
  6× Bronze Ingot → 1× Alloy Smelter
```

The player places both on the floor.

**Placing the Boiler — proximity rule (tutorial-enforced):**
The tutorial highlights the pre-placed Water Node and requires the player to place the Boiler in the tile directly adjacent to it. When adjacent, the Boiler automatically draws water — no pipe needed.

```
[WATER NODE] [BOILER]   ← 1 tile apart, water drawn automatically
```

A tooltip explains: *"The Boiler is close enough to the water source to draw from it directly. Craft Fluid Pipes later to place your Boiler anywhere."*

Coal still needs to be piped in via an item pipe from the Starter Chest (or a buffer). This reinforces the difference: **solid items use item pipes, fluids use proximity or fluid pipes**.

```
Starter Chest ──item pipe──► Boiler (Coal input)
Water Node    ──proximity──► Boiler (Water input, no pipe needed yet)
Boiler        ──???─────────► Steam machines (steam distribution — see below)
```

**Steam distribution from the Boiler:**
The Boiler outputs steam. Like water, steam is a fluid — it uses the same proximity rule initially. Steam machines placed adjacent to the Boiler receive steam automatically. Once Fluid Pipes are unlocked, steam can be routed freely.

This means the tutorial naturally clusters the first machines around the Boiler, which is a physically intuitive layout.

- The **Alloy Smelter** will replace the Workbench's Bronze Dust recipe with a proper automated Bronze production line

---

### Step 6 — Craft and Place the Steam Workbench
**Teaches:** Machine replacement / upgrade. The "shedding training wheels" moment.

The Steam Workbench recipe consumes the Primitive Workbench itself as a crafting ingredient:

```
Workbench recipe:
  1× Primitive Workbench (self) + 12× Bronze Ingot → 1× Steam Workbench
```

When the player confirms this recipe:
- The Primitive Workbench is removed from the floor
- A Steam Workbench item is produced
- The player places the Steam Workbench

This is a deliberate, irreversible step. The tutorial makes clear what is happening. It creates a satisfying milestone: the player has "graduated" from hand-crafting into steam-powered automation.

---

## Post-Tutorial State

After the tutorial, the player has:
- A working **Boiler** producing Steam
- A **Steam Workbench** (connected to Steam) for crafting new machines
- An **Alloy Smelter** for automated Bronze production
- A **Primitive Furnace** still on the floor (can keep or replace)
- An empty (or nearly empty) Starter Chest
- No ore extractor yet — the next free-play goal is building and placing a **Steam Extractor** on a Copper or Tin node

The Steam Workbench can craft additional Steam Workbenches using Bronze, allowing the player to scale crafting capacity.

---

## Primitive Workbench — Recipe Whitelist

The Primitive Workbench only supports these recipes (no others):

| Recipe | Input | Output | Purpose |
|---|---|---|---|
| Bronze Dust | 3× Copper Dust + 1× Tin Dust | 1× Bronze Dust | First material |
| Primitive Furnace | 8× Stone | 1× Primitive Furnace | First machine |
| Boiler | 8× Stone + 4× Bronze Ingot | 1× Boiler | Steam source |
| Alloy Smelter | 6× Bronze Ingot | 1× Alloy Smelter | First steam machine |
| Steam Workbench | 1× Primitive Workbench + 12× Bronze Ingot | 1× Steam Workbench | Tier upgrade |

---

## Fluid Pipes vs Item Pipes

Two distinct pipe types exist in the game. The tutorial establishes both:

| | Item Pipe | Fluid Pipe |
|---|---|---|
| Carries | Solid items (ores, ingots, dusts, components) | Liquids and gases (water, steam, chemicals) |
| Tutorial introduction | Step 3 (Bronze Dust → Furnace) | Unlocked after tutorial via Steam Workbench |
| Pre-fluid alternative | — | Proximity (1 tile from source) |
| Visual | Dark metal pipe | Blue-tinted pipe with flow animation |

**Fluid Pipe recipe (Steam Workbench):**
```
4× Bronze Ingot → 4× Fluid Pipe segments
```

Once the player has Fluid Pipes, the proximity rule no longer applies — the Boiler and Water Node can be placed anywhere on the floor as long as they are connected by a Fluid Pipe chain.

---

## Steam Workbench — Additional Capabilities

The Steam Workbench is a proper steam-powered machine. It:
- Consumes Steam from a connected Boiler (adjacent or via Fluid Pipe)
- Has a larger recipe list than the Primitive Workbench
- Can be crafted in multiples (as many as the player wants, using Bronze)
- Crafts all remaining Steam Age machines:

| Machine | Cost |
|---|---|
| Steam Extractor | 8× Bronze Ingot + 4× Stone |
| Steam Macerator | 10× Bronze Ingot + 4× Stone |
| Steam Compressor | 8× Bronze Ingot + 4× Stone |
| Fluid Pipe (×4) | 4× Bronze Ingot |
| Additional Steam Workbench | 12× Bronze Ingot |

---

## Mechanic Checklist Introduced by Tutorial

| Mechanic | When Introduced |
|---|---|
| Selecting a recipe in a machine | Step 1 |
| Items flowing from chest to machine | Step 1 |
| Crafting a machine item | Step 2 |
| Placing a machine on the floor | Step 2 |
| Drawing an item pipe between machines | Step 3 |
| Machine-to-machine item routing | Step 3–4 |
| Closing a pipe loop | Step 4 |
| Building a power source (Boiler) | Step 5 |
| Proximity fluid intake (Water Node → Boiler) | Step 5 |
| Proximity steam distribution (Boiler → machines) | Step 5 |
| Item pipe vs fluid pipe distinction | Step 5 |
| Steam-powered machines | Step 5 |
| Machine upgrade / replacement | Step 6 |
| Fluid Pipe crafting and placement | Post-tutorial free play |
