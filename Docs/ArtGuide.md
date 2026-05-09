# Art Style Guide — Hyperclocked Factory

> 32×32 px tiles, top-down 2D, pixel art. Mobile-first — assets must read clearly at ~100–130 px rendered size on a 390pt-wide screen.

---

## Palette

**Chosen palette: DB32 (DawnBringer 32)**

32 colors, warm industrial tones, strong contrast between darks and highlights. Download:
- Palette PNG: `Art/Palette/db32.png`
- Aseprite swatch: `Art/Palette/db32.ase`
- Unity reference sprite: `Art/Palette/db32_reference.png`

### Role Assignments

| Role | Color | DB32 Name (approx) |
|---|---|---|
| Machine casing (dark) | `#31222C` | Dark maroon-gray |
| Machine casing (mid) | `#4E3D45` | Medium warm gray |
| Machine casing highlight | `#6B5B64` | Light warm gray |
| Status light — active | `#99E550` | Lime green |
| Status light — idle | `#5B6EE1` | Blue |
| Status light — error/overvolt | `#D95763` | Red |
| Status light — no power | `#45283C` | Deep purple-gray |
| Item pipe | `#DF7126` | Copper orange |
| Fluid pipe | `#5FCDE4` | Sky blue |
| Cable | `#FBF236` | Yellow |
| Floor tile (base) | `#222034` | Dark navy |
| Floor tile (grid line) | `#2C2B3E` | Slightly lighter navy |
| Node slot (inactive) | `#3F3F74` | Muted indigo |
| Node slot (active/extracting) | `#D9A066` | Warm tan-gold |
| UI background | `#1A1A2E` | Near-black blue |
| UI border | `#847E87` | Medium gray |
| UI text (primary) | `#CBDBFC` | Off-white blue |
| UI text (secondary) | `#847E87` | Medium gray |
| UI button (normal) | `#323C39` | Dark green-gray |
| UI button (pressed) | `#4F6781` | Blue-gray |
| UI button (disabled) | `#222034` | Same as floor |

> All in-game sprites must only use DB32 colors. UI may use a small set of these — do not introduce colors outside the palette.

---

## Tile Anatomy (32×32 px)

### Machine Tile

```
+--------------------------------+
|####  casing border (4px)  ####|
|#  +------------------------+  #|
|#  |                        |  #|
|#  |    center icon         |  #|
|#  |    16×16 px            |  #|
|#  |    (centered)          |  #|
|#  +------------------------+  #|
|#                           [L] #|  ← status light 4×4px, bottom-right corner
+--------------------------------+
```

- **Casing border:** 4px solid dark casing color on all sides; 1px highlight on top-left edges, 1px shadow on bottom-right edges
- **Center icon:** 16×16 px, describes the machine's function (gear, flask, anvil, etc.); centered in the tile
- **Status light:** 4×4 px square, bottom-right corner (pixel 28–31, 28–31); color driven by machine state
- **Port arrows:** 4×2 px arrow drawn on the casing edge at the port direction; visible only when a pipe is adjacent; color matches pipe type (orange = item, blue = fluid)

### Pipe Tile

16 variants from a 4-bit directional bitmask (N, E, S, W). Render as:
- **Straight (2 directions):** center channel, 8px wide, with a 2px highlight along one edge
- **Corner (2 adjacent directions):** curved 90° channel
- **T-junction (3 directions):** T-shaped channel
- **Cross (all 4):** + shaped channel, 16px center
- **Cap (1 direction):** half-pipe ending in a rounded or flat cap
- **Empty (0):** not rendered (transparent)

Pipe channel width: **8px** centered on the tile axis. 2px border on each side (dark casing color). 4px center fill (pipe color).

Item pipe: copper-orange. Fluid pipe: sky blue. Cable: yellow — thinner channel (6px).

### Floor Tile

Two variants:
- **Base:** solid `#222034`, no detail
- **Grid line:** same base + 1px lines at tile edges in `#2C2B3E`; used as the default — gives subtle grid feel without overwhelming the machines

### Node Slot Tile

- **Inactive (no Extractor):** floor base + a faint 16×16 ore silhouette centered (2–3 colors, muted, ~50% opacity equivalent via dithering)
- **Active (Extractor placed):** floor base + glowing ring (8px diameter, node tan-gold color), 2-frame pulse animation

---

## Animation Specs

All animations are sprite sheet rows. Frame size: 32×32 px.

### Machine — Idle

- **Default:** static (1 frame). No animation unless the machine has an obvious mechanical element.
- **Optional:** 2-frame pulse on the status light only (light dims by ~30% on frame 2). Frame duration: 30 ticks (0.5s per frame).

### Machine — Active (processing)

- **4–6 frame loop.** Examples by machine type:
  - Macerator / Lathe: spinning gear or drill in the center icon area (4 frames, rotating CW)
  - Furnace / Blast Furnace: pulsing glow on icon (4 frames, brightness oscillation)
  - Chemical Reactor / Electrolyzer: bubble or spark animation (4–6 frames)
  - Assembler: piston or arm motion (4 frames)
  - Wiremill: spinning spool (4 frames)
- Frame duration: 8–12 ticks (0.13–0.20s per frame) — fast enough to feel alive, slow enough to read at a glance.

### Machine — No Power / Halted

- Static, status light set to error/no-power color. No animation.

### Pipe — Item Transit

- **None.** Items are invisible in pipes (GT-style). Pipes animate only via machine activity.

### Node Slot — Extracting

- 2-frame pulse on the active ring. Frame duration: 20 ticks (0.33s per frame).

### Extractor Placed on Node

- Small 4×4 px indicator above the Extractor tile, pulsing in node tan-gold. Syncs with node slot pulse.

---

## UI Style

### Font

**Primary font: m5x7** (free, open license)
- Download: `Art/Fonts/m5x7.ttf`
- Use at integer scales only: 1× (7px cap height), 2× (14px), 3× (21px)
- All in-game text uses this font

**Fallback: 04b_03** if m5x7 feels too large at small sizes.

### Panels

- Background: `#1A1A2E` (near-black), no transparency
- Border: 1px solid `#847E87` on all sides; 1px inner highlight (`#CBDBFC` at 30%) on top-left
- Corner: square (no rounding) — matches pixel art aesthetic
- Padding: 4px inside border on all sides

### Buttons

3-state sprites (9-slice or fixed size):
- **Normal:** `#323C39` background, `#847E87` 1px border, `#CBDBFC` label
- **Pressed:** `#4F6781` background, border unchanged, label shifts 1px down-right
- **Disabled:** `#222034` background, `#45283C` border, `#847E87` label (dimmed)

Standard button height: **16px** (2× font scale). Minimum tap target: **44×44 pt** (enforce via padding/invisible hit area in Unity — button sprite can be smaller).

### Build Menu Icons

- Size: **24×24 px** (centered in a 32×32 pt tap target with 4px padding each side)
- Same DB32 palette; simplified version of the full machine tile icon (no casing border, just the center icon on a transparent background)
- 3 states: normal, selected (yellow border), locked (desaturated + lock overlay)

---

## Sprite Sheet Layout

Organize sprite sheets per category:

| Sheet | Contents | Size |
|---|---|---|
| `machines.png` | All machine tiles, 4 states each (idle, active, halted, selected) | 1 row per machine |

**Machine row frame order** (applies to all tiers):

| Frame index | State |
|---|---|
| 0 | Idle (static) |
| 1 | Halted (static) |
| 2 | Selected (static) |
| 3+ | Active animation frames (4–6 frames) |

No spacing between frames — Unity slices by 32×32 starting at x=0.
| `pipes_item.png` | 16 item pipe variants | single row |
| `pipes_fluid.png` | 16 fluid pipe variants | single row |
| `cables.png` | 16 cable variants | single row |
| `floor.png` | Floor base + grid line variant | 2 frames |
| `nodes.png` | Node slot inactive + active (2-frame) | single row |
| `icons_build.png` | 24×24 build menu icons, 3 states each | 1 row per machine |
| `ui_panels.png` | Panel corners, edges, button states | atlas |

All sheets: PNG, no compression, power-of-two dimensions preferred.

---

## Checklist Before Phase 1

- [ ] DB32 palette files downloaded → `Art/Palette/`
- [ ] m5x7 font downloaded → `Art/Fonts/`
- [ ] Unity `Assets/` folder structure created (see `PHASE_0_CHECKLIST.md` §8a)
- [ ] Placeholder floor tile drawn (can be a solid color for Phase 1)
- [ ] Placeholder machine tile drawn (solid casing color + white icon placeholder)
- [ ] Placeholder pipe tiles drawn (flat color, no border detail)
