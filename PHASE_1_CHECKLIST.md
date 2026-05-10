# Phase 1 — Core Prototype Checklist

> Goal: A playable end-to-end loop — Node → Extractor → Pipe → Furnace → Output — with hotbar placement, milestone S1 firing, and a working milestone tracker UI.  
> All decisions are locked in `CLAUDE.md`. This checklist tracks implementation only.

---

## 1. Project Setup

- [x] URP 2D project, Input System, mobile build target confirmed
- [x] `Assets/` folder structure created (Scripts, Data, Art, Prefabs, Scenes)
- [x] Naming conventions confirmed → `CLAUDE.md`

---

## 2. ScriptableObject Definitions

- [x] `ItemData` — itemName, icon, stackSize
- [x] `RecipeData` — inputs[], outputs[], processingTime, energyCost, steamPerTick
- [x] `MachineData` — machineName, tier, tileSize, watts, voltage, recipes[], sprite
- [x] `MilestoneData` — trigger (item or machine), unlockType, unlockTarget, prerequisites[]
- [x] `TierEnum` — Steam / LV / MV / HV
- [x] Create SO assets for Phase 1 items: Coal, Copper Ore, Bronze Ingot, Copper Dust, Tin Dust ← run `HF > Create Phase 1 SO Assets`
- [x] Create SO asset: Primitive Furnace (`MachineData`) ← run `HF > Create Phase 1 SO Assets`
- [x] Create SO asset: Steam Extractor (`MachineData`) ← run `HF > Create Phase 1 SO Assets`
- [x] Create SO recipes: Smelt (Copper Dust → Copper Ingot), Alloy (Cu + Sn → Bronze) ← run `HF > Create Phase 1 SO Assets`

---

## 3. Grid & Floor

- [x] `GridManager` — 16×16, TryPlace / TryRemove / GetAt, world↔cell helpers, gizmo grid
- [x] `MachinePlaceholderView` — tier-tinted colored square until real sprites land
- [x] `RandomFloorTile` — deterministic random variant + 90° rotation from cell hash
- [x] `FloorSetupEditor` — `HF > Setup Floor` paints the 16×16 Tilemap
- [x] **Run `HF > Setup Game Scene**` in Unity (creates camera + GridManager)
- [x] **Run `HF > Setup Floor**` in Unity (imports floor.png, paints 16×16 Tilemap)
- [x] Verify floor renders correctly in Game view

---

## 4. Camera & Input

- [x] Pinch-to-zoom (orthographic size clamp: min 4, max 12) — `CameraController.cs`
- [x] Drag-to-pan (clamp camera within grid bounds) — `CameraController.cs`
- [x] Tap detection → world position → cell coordinate — `InputReader.OnTap` + `CameraController.ScreenToCell`
- [x] All input via Unity Input System (touch + mouse parity) — `InputReader.cs`
- [x] **Run `HF > Setup Input & Camera**` in Unity (adds InputReader + CameraController to scene)

---

## 5. Player Inventory & Hotbar

- [x] `InventorySlot` — holds ItemData + quantity
- [x] `PlayerInventory` — 8 hotbar + 36 inventory slots, singleton
- [x] Hotbar UI — 8 slots always visible above toolbar; selected slot highlighted
- [x] Inventory screen — 4×9 grid, opened from toolbar; tap slot → moves to hotbar
- [x] Tap hotbar slot → sets active placement item
- [x] Tap grid cell (with active item) → place machine / pipe on correct layer
- [x] Long-press placed machine → confirm pick-up → returns to inventory
- [x] **Run `HF > Setup Inventory UI**` in Unity (builds Canvas, hotbar, inventory panel, confirm panel)

---

## 6. Resource Nodes

- [ ] `NodeData` SO — resourceType (ItemData ref), worldCell (Vector2Int)
- [ ] `NodeManager` — holds list of all nodes; knows which have an Extractor
- [ ] Node visual — inactive sprite (slot outline); active sprite (glowing) when Extractor placed
- [ ] Pre-place 3 nodes in GameScene: Copper Ore, Tin Ore, Coal (fixed positions)
- [ ] Nodes render on machine layer (behind pipes)

---

## 7. Item Pipe Network

- [ ] `PipeData` SO — color (enum), layer (item / fluid)
- [ ] Pipe layer in GridManager (separate dict from machines)
- [ ] Place / remove pipes on pipe layer (independent of machine layer)
- [ ] Bitmask auto-connect — pipe sprite updates when neighbors change (4-dir, 16 combos)
- [ ] Pipe color — tap pipe → cycle through colors; only same-color segments connect
- [ ] `PipeNetwork` — adjacency graph of connected same-color pipe segments
- [ ] Port assignment — player assigns port direction per pipe-to-machine connection
- [ ] BFS routing — finds path from machine output port to nearest accepting input port
- [ ] Item transit — items travel at 1 tile / tick (1s); invisible in pipe (GT style)
- [ ] Output full → machine halts until pipe clears

---

## 8. Machine System

- [ ] `MachineState` enum — Idle / Processing / OutputFull / NoInput / NoPower / Halted
- [ ] `MachineInstance` — runtime state (current recipe, progress timer, input/output buffers)
- [ ] `TickManager` — fires global tick every 1s; all machines + extractors subscribe
- [ ] On tick: pull inputs from pipe, advance timer, push outputs to pipe
- [ ] Machine state transitions (idle → processing → output full, etc.)
- [ ] Machine info panel (tap machine) — name, recipe, status, progress bar, Remove button

---

## 9. Steam Extractor

- [ ] `ExtractorInstance` extends `MachineInstance`
- [ ] Must be placed on a Node tile (placement validation)
- [ ] On tick: produces 1× node resource → pushes to output pipe
- [ ] Active/inactive visual driven by `NodeManager`

---

## 10. Primitive Furnace

- [ ] Standard `MachineInstance` — no power (coal-fueled placeholder for prototype)
- [ ] Recipes: Copper Dust → Copper Ingot; Copper + Tin Dust → Bronze Ingot
- [ ] Select active recipe via Machine Info Panel
- [ ] Processing time 4s (4 ticks)

---

## 11. Milestone System

- [ ] `MilestoneManager` singleton — tracks unlocked milestones, listens for item produce events
- [ ] On item produced → check all pending milestones for trigger match → fire unlock
- [ ] On machine built → check trigger match → fire unlock
- [ ] S0 — fires automatically on scene load (unlocks Extractor, Furnace, Boiler slots)
- [ ] S1 — fires on first Bronze Ingot produced

---

## 12. Milestone Tracker UI

- [ ] Milestone screen (full-screen, opened from toolbar)
- [ ] Tree view — nodes for S0, S1, S2, A1, B1, B2, B3, G1
- [ ] Node states: locked (grey) / available (lit) / unlocked (green)
- [ ] Tap node → shows condition text and unlock reward
- [ ] Updates in real time as milestones fire

---

## 13. End-to-End Demo Gate

> All of the below must work together before Phase 1 is "done".

- [ ] Place Extractor on Copper Ore node → copper ore enters pipe
- [ ] Copper ore routes through pipe to Furnace input
- [ ] Furnace produces Copper Ingot after 4 ticks
- [ ] Copper + Tin → Bronze Ingot recipe works
- [ ] Producing Bronze Ingot fires **S1** milestone automatically
- [ ] Milestone Tracker UI reflects S1 unlocked in real time
- [ ] No hard crashes over a 5-minute play session

---

## Deliverables


| Deliverable                     | Done |
| ------------------------------- | ---- |
| Folder structure + SO schemas   | [x]  |
| GridManager + floor tilemap     | [x]  |
| Camera pan + zoom               | [x]  |
| Hotbar + inventory UI           | [x]  |
| Resource nodes                  | [ ]  |
| Item pipe network (BFS routing) | [ ]  |
| Machine tick system             | [ ]  |
| Steam Extractor                 | [ ]  |
| Primitive Furnace               | [ ]  |
| Milestone system                | [ ]  |
| Milestone tracker UI            | [ ]  |
| End-to-end demo passing         | [ ]  |
