# Sprite Specs — Per-Machine Animation Reference

> Authoritative frame breakdown for all animated game objects.  
> Fill in `TBD` entries as each sprite is finalized. I read this before wiring any Animator.

---

## Schema

Each machine block lists:


| Field        | Meaning                                                                       |
| ------------ | ----------------------------------------------------------------------------- |
| **sheet**    | Filename + 0-based row index                                                  |
| **idle**     | Frame(s), type (`static` or `loop`), fps if animated                          |
| **halted**   | Frame(s), type                                                                |
| **selected** | Frame(s), type                                                                |
| **startup**  | Frame range, count, `play-once`, fps — **omit line if no startup animation**  |
| **active**   | Frame range, count, `loop`, fps                                               |
| **shutdown** | Frame range, count, `play-once`, fps — **omit line if no shutdown animation** |


Frame ranges are 0-based inclusive: `[3–6]` = frames 3, 4, 5, 6 (4 total). `[0]` = single frame.  
Row indices are 0-based from the top of the sheet.  
`TBD` = not yet drawn; fill in as art is finalized.

> **Startup** plays once when a machine begins processing (idle → active transition).  
> **Shutdown** plays once when processing stops (active → idle transition). Can be the same frames as startup played in reverse — write as `[4–3]` to indicate reversed playback.  
> If omitted, the animator cuts directly between idle and active.

---

## Steam Tier — `machines_steam.png`

### Steam Extractor

- **sheet:** `machines_steam.png` row 0
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **startup:** [3] — 1 frame, play-once
- **active:** [4–8] — 5 frames, loop, TBD fps
- **shutdown:** [3] — 1 frame, play-once (reuses startup frame)

---

### Primitive Workbench

- **sheet:** `machines_steam.png` row 1
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Primitive Furnace

- **sheet:** `machines_steam.png` row 2
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4-frame glow pulse per ArtGuide.

---

### Boiler

- **sheet:** `machines_steam.png` row 3
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Produces steam — consider a startup animation (heat-up) if art supports it.

---

### Alloy Smelter

- **sheet:** `machines_steam.png` row 4
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **startup:** [3–4] — 2 frames, play-once
- **active:** [5–9] — TBD frames, loop, TBD fps
- **shutdown:** [4–3] — 2 frames, play-once

> Idle/halted/selected sprites marked done in ArtChecklist.

---

### Steam Workbench

- **sheet:** `machines_steam.png` row 5
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Steam Macerator

- **sheet:** `machines_steam.png` row 6
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4-frame spinning gear/drill per ArtGuide.

---

### Steam Compressor

- **sheet:** `machines_steam.png` row 7
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Steam Washer

- **sheet:** `machines_steam.png` row 8
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Brick Furnace

- **sheet:** `machines_steam.png` row 9
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Chemical Reactor

- **sheet:** `machines_steam.png` row 10
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4–6 frame bubble/spark per ArtGuide.

---

### Workshop Controller

- **sheet:** `machines_steam.png` row 11
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Workshop Frame

- **sheet:** `machines_steam.png` row 12
- **idle:** [0] — static

> Static only — no states, no animation. Single sprite used in all conditions.

---

## LV Tier — `machines_lv.png`

### LV Electric Furnace

- **sheet:** `machines_lv.png` row 0
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Macerator

- **sheet:** `machines_lv.png` row 1
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Compressor

- **sheet:** `machines_lv.png` row 2
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Extractor

- **sheet:** `machines_lv.png` row 3
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Assembler

- **sheet:** `machines_lv.png` row 4
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4-frame piston/arm motion per ArtGuide.

---

### LV Alloy Smelter

- **sheet:** `machines_lv.png` row 5
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Chemical Reactor

- **sheet:** `machines_lv.png` row 6
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### LV Ore Washer

- **sheet:** `machines_lv.png` row 7
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Lathe

- **sheet:** `machines_lv.png` row 8
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4-frame spinning element per ArtGuide.

---

### Wiremill

- **sheet:** `machines_lv.png` row 9
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4-frame spinning spool per ArtGuide.

---

### Electrolyzer

- **sheet:** `machines_lv.png` row 10
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Expected: 4–6 frame spark/bubble per ArtGuide.

---

### Magnetizer

- **sheet:** `machines_lv.png` row 11
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

---

### Electric Blast Furnace Controller

- **sheet:** `machines_lv.png` row 12
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Multiblock controller tile only. Casings are static (`machines_lv.png` row TBD — add when art exists).

---

### Steam Turbine

- **sheet:** `machines_lv.png` row 13
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Generator — active when producing power, not processing a recipe.

---

### Solar Panel

- **sheet:** `machines_lv.png` row 14
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Always active during daytime if power system tracks day/night; otherwise always active.

---

### Steel Boiler Controller

- **sheet:** `machines_lv.png` row 15
- **idle:** [0] — static
- **halted:** [1] — static
- **selected:** [2] — static
- **active:** [3–TBD] — TBD frames, loop, TBD fps

> Multiblock controller tile only. Casings are static (`machines_lv.png` row TBD — add when art exists).

---

## Infrastructure

### Node Slot

- **sheet:** `nodes.png`
- **inactive:** [0] — static
- **active (extracting):** [1–2] — 2 frames, loop, 3 fps (30-tick pulse per ArtGuide)

### Water Node

- **sheet:** `nodes.png` row TBD
- **idle:** [TBD–TBD] — TBD frames, loop, TBD fps

> Animated water tile per ArtChecklist.