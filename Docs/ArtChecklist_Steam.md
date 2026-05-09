# Art Checklist — Steam Tier

All sprites use DB32 palette, 32×32 px tiles.  
Machine sprite sheet row order: **0=idle · 1=halted · 2=selected · 3+=active animation (4–8 frames)**

---

## Machines — `machines_steam.png`

One row per machine. Tick each state individually.


| Machine             | Idle | Halted | Selected | Active Anim |
| ------------------- | ---- | ------ | -------- | ----------- |
| Steam Extractor     | [x]  | [x]    | [x]      | [x]         |
| Primitive Workbench | [ ]  | [ ]    | [ ]      | [ ]         |
| Primitive Furnace   | [ ]  | [ ]    | [ ]      | [ ]         |
| Boiler              | [ ]  | [ ]    | [ ]      | [ ]         |
| Alloy Smelter       | [ x] | [x]    | [ x]     | [x]         |
| Steam Workbench     | [ ]  | [ ]    | [ ]      | [ ]         |
| Steam Macerator     | [ ]  | [ ]    | [ ]      | [ ]         |
| Steam Compressor    | [ ]  | [ ]    | [ ]      | [ ]         |
| Steam Washer        | [ ]  | [ ]    | [ ]      | [ ]         |
| Brick Furnace       | [ ]  | [ ]    | [ ]      | [ ]         |
| Chemical Reactor    | [ ]  | [ ]    | [ ]      | [ ]         |
| Workshop Controller | [ ]  | [ ]    | [ ]      | [ ]         |
| Workshop Frame      | [ ]  | —      | —        | —           |


> Workshop Frame only needs 1 sprite (static, no states). Workshop Controller follows normal states.  
> Build menu reuses the idle frame (row 0) as the icon — no separate icon sheet. Locked state applied via black tint in code.

---

## Infrastructure — `pipes_item.png` · `pipes_fluid.png` · `cables.png`

16 variants each from 4-bit directional bitmask (N/E/S/W).


| Sheet             | Variants done | Notes                      |
| ----------------- | ------------- | -------------------------- |
| `pipes_item.png`  | 16 / 16       | Copper orange, 8px channel |
| `pipes_fluid.png` | 16 / 16       | Sky blue, 8px channel      |


> Cables not needed for Steam tier — skip until LV.

---

## Floor & Nodes — `floor.png` · `nodes.png`


| Asset           | Done | Notes                                                                                                                   |
| --------------- | ---- | ----------------------------------------------------------------------------------------------------------------------- |
| Floor tile      | [ x] | Custom art, no grid lines; 2 asymmetric variants, random rotation enabled in Unity Tilemap (8 apparent variants)        |
| Floor grid tile | [ x] | Same art with 1px grid overlay, used for factory floor; 2 asymmetric variants, random rotation enabled in Unity Tilemap |
| Coal Node       | [ x] | Single static sprite, distinct from other ore nodes                                                                     |
| Copper Ore Node | [ x] | Single static sprite                                                                                                    |
| Tin Ore Node    | [x ] | Single static sprite                                                                                                    |
| Iron Ore Node   | [x ] | Single static sprite                                                                                                    |
| Sulfur Ore Node | [ x] | Single static sprite                                                                                                    |
| Water Node      | [ x] | Animated water tile, visually distinct from ore nodes                                                                   |


---

## Interactable Objects


| Asset         | Done | Notes                                        |
| ------------- | ---- | -------------------------------------------- |
| Starter Chest | [ ]  | Static sprite, pre-placed, cannot be moved   |
| Stone Chest   | [ ]  | Placeable storage; 8× Stone → 1× Stone Chest |


---

---

## UI — `ui_panels.png`


| Asset                     | Done | Notes                                     |
| ------------------------- | ---- | ----------------------------------------- |
| Panel background + border | [ ]  | `#1A1A2E` bg, 1px `#847E87` border        |
| Button — normal           | [ ]  | `#323C39` bg                              |
| Button — pressed          | [ ]  | `#4F6781` bg, label shifts 1px down-right |
| Button — disabled         | [ ]  | `#222034` bg, `#45283C` border            |


---

## Font


| Asset                               | Done |
| ----------------------------------- | ---- |
| m5x7.ttf downloaded to `Art/Fonts/` | [ ]  |
