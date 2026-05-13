# Hyperclocked Factory — Technical Reference

> **Living document.** Updated every time Unity work is done — scripts, prefabs, SOs, scene changes, editor tools.  
> Read this before touching any script. If something isn't here, add it.

---

## Table of Contents

1. [Scene Setup Flow](#1-scene-setup-flow)
2. [Folder Structure](#2-folder-structure)
3. [ScriptableObjects](#3-scriptableobjects)
4. [Runtime Scripts](#4-runtime-scripts)
5. [Editor Scripts](#5-editor-scripts)
6. [Prefabs](#6-prefabs)
7. [Scene Hierarchy](#7-scene-hierarchy)
8. [Wiring Reference](#8-wiring-reference)
9. [Coordinate System](#9-coordinate-system)
10. [Sprite Import Conventions](#10-sprite-import-conventions)

---

## 1. Scene Setup Flow

Run these menu items **in order** the first time you open the project in Unity:

| Step | Menu Item | What It Does |
|---|---|---|
| 1 | `HF > Setup Game Scene` | Creates `GameScene.unity` with camera + GridManager + MachinePlaceholder prefab |
| 2 | `HF > Setup Floor` | Imports `floor.png`, creates `FloorTile.asset`, adds Grid/Tilemap to scene, paints 16×16 |
| 3 | `HF > Create Phase 1 SO Assets` | Imports machine sprites, creates all ItemData / RecipeData / MachineData SOs |
| 4 | `HF > Setup Input & Camera` | Adds `InputReader` GameObject and `CameraController` to Main Camera |
| 5 | `HF > Setup Inventory UI` | Builds full Canvas hierarchy: hotbar, toolbar, inventory panel, pickup confirm. Also adds PlayerInventory and PlacementController. |
| 6 | `HF > Setup Pipe System` | Creates PipeData + pipe ItemData SOs; adds TickManager, PipeNetwork, PipePortPanel to scene. |
| 7 | `HF > Setup Machine Info Panel` | Builds MachineInfoPanel UI under GameCanvas; adds MachineInfoPanel_Host to scene with all fields wired. |
| 8 | `HF > Setup Milestone UI` | Adds MilestoneTrackerPanel under GameCanvas and wires MilestonesBtn → Toggle. |

After step 1, the MachinePlaceholder prefab is automatically wired into GridManager via the editor script — no manual drag needed.

---

## 2. Folder Structure

```
Assets/
├── Art/
│   ├── Cables/
│   ├── Fonts/
│   │   ├── m5x7.ttf                         ← pixel font by Magnus Pålsson (source TTF)
│   │   └── m5x7 SDF.asset                   ← TMP_FontAsset (SDF); set as TMP default font
│   ├── Machines/
│   │   ├── SteamExtractorSpriteSheet.png    ← 9-frame horizontal strip, 32×32 each
│   │   └── SteamAlloySmelterSpriteSheet.png ← 9-frame horizontal strip, 32×32 each
│   ├── Palette/
│   ├── Pipes/
│   ├── Tiles/
│   │   ├── floor.png                        ← 2-variant floor tile sheet, 32×32 each
│   │   └── nodes.png                        ← 6-frame node sprite strip (empty/coal/copper/tin/iron/sulfur), 32×32 each
│   └── UI/
├── Data/
│   ├── Items/         ← ItemData SO assets (one file per item)
│   ├── Machines/      ← MachineData SO assets (one file per machine)
│   ├── Milestones/    ← MilestoneData SO assets
│   ├── Pipes/         ← PipeData SO assets (one file per color/layer combo)
│   ├── Recipes/       ← RecipeData SO assets
│   └── FloorTile.asset ← RandomFloorTile SO (created by HF > Setup Floor)
├── Prefabs/
│   ├── Machines/
│   │   └── MachinePlaceholder.prefab  ← SpriteRenderer + MachinePlaceholderView
│   ├── Pipes/
│   └── UI/
├── Scenes/
│   ├── GameScene.unity   ← main game scene (created by HF > Setup Game Scene)
│   └── SampleScene.unity ← Unity default, unused
└── Scripts/
    ├── Core/
    │   ├── CameraController.cs
    │   ├── ExtractorInstance.cs   ← §9
    │   ├── GridManager.cs
    │   ├── InputReader.cs
    │   ├── MachinePlaceholderView.cs
    │   ├── MachineInstance.cs     ← §8
    │   ├── MachineState.cs        ← §8
    │   ├── MilestoneManager.cs    ← §10
    │   ├── NodeInstance.cs
    │   ├── NodeManager.cs
    │   ├── NodeView.cs
    │   ├── PlacedMachine.cs
    │   ├── PlacedPipe.cs          ← §7
    │   ├── PlacementController.cs
    │   ├── PlayerInventory.cs
    │   ├── PipeNetwork.cs         ← §7
    │   ├── PipeView.cs            ← §7
    │   ├── RandomFloorTile.cs
    │   └── TickManager.cs         ← §7
    ├── Data/
    │   ├── InventorySlot.cs
    │   ├── ItemData.cs
    │   ├── MachineData.cs
    │   ├── MilestoneData.cs
    │   ├── PipeData.cs            ← §7
    │   ├── RecipeData.cs
    │   └── TierEnum.cs
    ├── Debug/
    │   └── DebugStartInventory.cs ← populates hotbar on Start for testing; remove before ship
    ├── Editor/
    │   ├── FloorSetupEditor.cs
    │   ├── GameSceneBuilder.cs
    │   ├── InputCameraSetupEditor.cs
    │   ├── InventoryUISetupEditor.cs
    │   ├── MachineInfoPanelSetupEditor.cs ← §8
    │   ├── MilestoneUISetupEditor.cs      ← §10
    │   ├── PipeSetupEditor.cs     ← §7
    │   └── SOSetupEditor.cs
    ├── Milestones/    ← (empty — MilestoneManager goes here)
    ├── Save/          ← (empty — GridState JSON goes here)
    └── UI/
        ├── HotbarUI.cs
        ├── InventoryScreenUI.cs
        ├── InventorySlotUI.cs
        ├── MachineInfoPanel.cs    ← §8
        ├── NodeSelectionEntry.cs
        ├── NodeSelectionPanel.cs
        ├── PipePortPanel.cs       ← §7
        ├── MilestoneNodeUI.cs     ← §10
        ├── MilestoneTrackerUI.cs  ← §10
        └── WorkshopButtonUI.cs
```

---

## 3. ScriptableObjects

ScriptableObjects hold all game data. No values are hardcoded in scripts.

---

### ItemData
**File:** `Assets/Scripts/Data/ItemData.cs`  
**Asset location:** `Assets/Data/Items/`  
**Menu:** `HF > Item Data`

| Field | Type | Description |
|---|---|---|
| `itemName` | `string` | Display name (e.g. "Copper Ore") |
| `icon` | `Sprite` | Inventory/hotbar icon — assign when art is ready |
| `stackSize` | `int` | Max items per inventory slot (default 64) |
| `placeableMachine` | `MachineData` | If set, tapping a grid cell places this machine. Null for ores, dusts, ingots. |
| `placeablePipe` | `PipeData` | If set, tapping a grid cell places this pipe. Mutually exclusive with `placeableMachine`. |

**Phase 1 assets created by `HF > Create Phase 1 SO Assets`:**

| File | itemName |
|---|---|
| `Coal.asset` | Coal |
| `CopperOre.asset` | Copper Ore |
| `TinOre.asset` | Tin Ore |
| `CopperDust.asset` | Copper Dust |
| `TinDust.asset` | Tin Dust |
| `CopperIngot.asset` | Copper Ingot |
| `BronzeIngot.asset` | Bronze Ingot |
| `Item_SteamExtractor.asset` | Steam Extractor (placeable; `placeableMachine` → Steam_Extractor) |
| `Item_PrimitiveFurnace.asset` | Primitive Furnace (placeable; `placeableMachine` → Steam_PrimitiveFurnace) |

---

### RecipeData
**File:** `Assets/Scripts/Data/RecipeData.cs`  
**Asset location:** `Assets/Data/Recipes/`  
**Menu:** `HF > Recipe Data`

| Field | Type | Description |
|---|---|---|
| `inputs` | `ItemStack[]` | Required inputs (item + quantity pairs) |
| `outputs` | `ItemStack[]` | Produced outputs (item + quantity pairs) |
| `processingTime` | `float` | Seconds to complete one cycle |
| `energyCost` | `float` | Watts consumed per cycle (0 = steam or manual) |
| `steamPerTick` | `float` | Litres of steam consumed per second (0 = electric) |

**`ItemStack` struct** (defined in `RecipeData.cs`):

| Field | Type | Description |
|---|---|---|
| `item` | `ItemData` | Reference to the item SO |
| `quantity` | `int` | Amount required/produced |

**Phase 1 assets:**

| File | Inputs | Outputs | Time |
|---|---|---|---|
| `Recipe_SmeltCopper.asset` | 1× Copper Dust | 1× Copper Ingot | 4 s |
| `Recipe_SmeltBronze.asset` | 3× Copper Dust + 1× Tin Dust | 1× Bronze Ingot | 4 s |

---

### MachineData
**File:** `Assets/Scripts/Data/MachineData.cs`  
**Asset location:** `Assets/Data/Machines/`  
**Menu:** `HF > Machine Data`

| Field | Type | Description |
|---|---|---|
| `machineName` | `string` | Display name |
| `tier` | `Tier` (enum) | Steam / LV / MV / HV |
| `tileSizeX` | `int` | Grid width (always 1 for now) |
| `tileSizeY` | `int` | Grid height (always 1 for now) |
| `steamPerTick` | `float` | Steam consumption L/s (0 if electric) |
| `wattsDraw` | `float` | Power draw in Watts (0 if steam-powered) |
| `voltageRequired` | `int` | Required voltage: 0 (steam), 32 (LV), 128 (MV), 512 (HV) |
| `availableRecipes` | `RecipeData[]` | All recipes this machine can run |
| `sprite` | `Sprite` | Idle sprite — shown when placed (frame 0 of the sprite sheet) |
| `animatorController` | `RuntimeAnimatorController` | Animator for idle/active/halted states — assign when ready |

**Phase 1 assets:**

| File | machineName | sprite source |
|---|---|---|
| `Steam_Extractor.asset` | Steam Extractor | `SteamExtractorSpriteSheet.png` frame 0 |
| `Steam_PrimitiveFurnace.asset` | Primitive Furnace | `SteamAlloySmelterSpriteSheet.png` frame 0 (temp stand-in) |

> The Primitive Furnace uses the Alloy Smelter sprite as a placeholder. Swap `sprite` in the Inspector when real art is done — no code change needed.

---

### MilestoneData
**File:** `Assets/Scripts/Data/MilestoneData.cs`  
**Asset location:** `Assets/Data/Milestones/`  
**Menu:** `HF > Milestone Data`

| Field | Type | Description |
|---|---|---|
| `milestoneName` | `string` | Display name (e.g. "S1 — First Bronze Ingot") |
| `triggerItem` | `ItemData` | Fire when this item is first produced (leave null if machine trigger) |
| `triggerMachine` | `MachineData` | Fire when this machine is first built (leave null if item trigger) |
| `unlockType` | `MilestoneUnlockType` | Machine / Recipe / FloorExpansion / TierTransition |
| `unlockTarget` | `ScriptableObject` | The SO that gets unlocked (MachineData or RecipeData); null for expansion/transition |
| `floorExpansionSize` | `int` | New grid side length when `unlockType == FloorExpansion` |
| `prerequisites` | `MilestoneData[]` | Must all be unlocked before this milestone becomes available |

**`MilestoneUnlockType` enum** (defined in `MilestoneData.cs`):
- `Machine` — adds a MachineData to the buildable set
- `Recipe` — adds a RecipeData to a machine's available recipes
- `FloorExpansion` — grows the grid to `floorExpansionSize × floorExpansionSize`
- `TierTransition` — unlocks the next tier (LV, MV, etc.)

**Phase 1 assets created by `HF > Create Phase 1 SO Assets`:**

| File | milestoneName | Trigger | Prerequisites |
|---|---|---|---|
| `Milestone_S0.asset` | S0 — Steam Age Begins | None (auto-fires on scene load) | — |
| `Milestone_S1.asset` | S1 — First Alloy | First Bronze Ingot produced | S0 |

---

### RandomFloorTile
**File:** `Assets/Scripts/Core/RandomFloorTile.cs`  
**Asset location:** `Assets/Data/FloorTile.asset`  
**Menu:** `HF > Random Floor Tile`

| Field | Type | Description |
|---|---|---|
| `variants` | `Sprite[]` | The sliced floor tile sprites — populated by `HF > Setup Floor` |

How it works: `GetTileData` hashes the cell's `(x, y)` position using a deterministic integer hash. The hash picks both the sprite variant and one of four 90° rotations. Same cell always produces the same result — no runtime randomness. `TileFlags.LockTransform` prevents the Tilemap from overriding the rotation.

---

### TierEnum
**File:** `Assets/Scripts/Data/TierEnum.cs`

```
public enum Tier { Steam = 0, LV = 1, MV = 2, HV = 3 }
```

Used by `MachineData.tier` and by `MachinePlaceholderView` to pick the tint color.

---

### PipeData
**File:** `Assets/Scripts/Data/PipeData.cs`  
**Asset location:** `Assets/Data/Pipes/`  
**Menu:** `HF > Pipe Data`  
**Created by:** `HF > Setup Pipe System`

| Field | Type | Description |
|---|---|---|
| `pipeName` | `string` | Display name, e.g. "White Item Pipe" |
| `color` | `PipeColor` | White / Red / Green / Blue / Yellow |
| `layer` | `PipeLayer` | Item / Fluid |
| `icon` | `Sprite` | Hotbar/inventory icon — assign when art is ready |

**Enums (defined in `PipeData.cs`):**
- `PipeColor` — `White, Red, Green, Blue, Yellow`
- `PipeLayer` — `Item, Fluid`

Only same-color, same-layer pipes connect to each other. Machines accept connections from any color pipe.

**Phase 1 assets created by `HF > Setup Pipe System`:**

| File | Color | Layer |
|---|---|---|
| `ItemPipe_White.asset` | White | Item |
| `ItemPipe_Red.asset` | Red | Item |
| `ItemPipe_Green.asset` | Green | Item |
| `ItemPipe_Blue.asset` | Blue | Item |
| `ItemPipe_Yellow.asset` | Yellow | Item |

Matching `ItemData` SOs are also created in `Assets/Data/Items/` (`ItemPipe_{Color}_Item.asset`) so pipes can be held in the hotbar and placed on the grid.

---

## 4. Runtime Scripts

---

### GridManager
**File:** `Assets/Scripts/Core/GridManager.cs`  
**GameObject:** `GridManager` in GameScene  
**Pattern:** Singleton (`GridManager.Instance`)

Owns the dictionary that maps grid cells to placed machines. All placement/removal goes through this.

**Inspector fields:**

| Field | Type | Default | Description |
|---|---|---|---|
| `width` | `int` | 16 | Grid width in cells |
| `height` | `int` | 16 | Grid height in cells |
| `machinePrefab` | `GameObject` | MachinePlaceholder prefab | Instantiated for every placed machine |

**Public API:**

| Method | Returns | Description |
|---|---|---|
| `IsInBounds(cell)` | `bool` | True if cell is within current grid size |
| `IsCellEmpty(cell)` | `bool` | True if no machine is at that cell |
| `TryPlace(data, cell)` | `bool` | Instantiates prefab, calls `MachinePlaceholderView.Init`, registers in dict, fires `OnMachinePlaced` |
| `TryRemove(cell)` | `bool` | Destroys the GameObject, removes from dict, fires `OnMachineRemoved` |
| `IsWorkshopPlaced(workshopData)` | `bool` | Returns true if any placed machine matches the given MachineData SO |
| `GetAt(cell)` | `PlacedMachine` | Returns runtime data for the machine at cell, or null |
| `CellToWorld(cell)` | `Vector3` | Cell (0,0) → world (0.5, 0.5, 0). Centre of the cell. |
| `WorldToCell(world)` | `Vector2Int` | Floors the world position to cell coords |

**Events:**

| Event | Signature | Fires when |
|---|---|---|
| `OnMachinePlaced` | `Action<PlacedMachine>` | A machine is successfully placed |
| `OnMachineRemoved` | `Action<PlacedMachine>` | A machine is removed from the grid |
| `OnPipePlaced` | `Action<PlacedPipe>` | A pipe is successfully placed |
| `OnPipeRemoved` | `Action<PlacedPipe>` | A pipe is removed from the grid |

**Pipe layer public API:**

| Method | Returns | Description |
|---|---|---|
| `IsPipeAt(cell)` | `bool` | True if a pipe exists at that cell |
| `GetPipeAt(cell)` | `PlacedPipe` | Returns pipe at cell, or null |
| `TryPlacePipe(data, cell)` | `bool` | Creates pipe GO with `PipeView`, registers in `_pipes`, fires `OnPipePlaced`, refreshes bitmask |
| `TryRemovePipe(cell)` | `bool` | Destroys pipe GO, removes from `_pipes`, fires `OnPipeRemoved`, refreshes bitmask |

Pipes and machines occupy the same cell space but different dictionaries — a cell can hold one machine OR one pipe, not both. (Pipes cannot be placed on machine cells and vice versa — `IsCellEmpty` checks `_grid` only; pipe cells are checked separately via `IsPipeAt`.)

**Bitmask refresh:** after any `TryPlacePipe` or `TryRemovePipe`, `RefreshPipeNeighbors` is called. It re-evaluates the 4 cardinal neighbors of the changed cell and calls `PipeView.UpdateConnections` on each. A pipe connects to a neighbor if they share the same `PipeColor` and `PipeLayer`.

**Gizmos:** draws the grid lines in the Scene view (visible in editor, not in game).

**How `TryPlace` works step by step:**
1. Bounds and empty check
2. `Instantiate(machinePrefab)` at `CellToWorld(cell)`, parented to GridManager transform
3. If the prefab has `MachinePlaceholderView`, calls `view.Init(data)` → sets sprite or tinted square
4. Creates a `PlacedMachine` and stores it in `_grid[cell]`

---

### PlacedMachine
**File:** `Assets/Scripts/Core/PlacedMachine.cs`  
**Type:** Plain C# class (not a MonoBehaviour)

Runtime snapshot of one placed machine. Lives in `GridManager._grid`.

| Field | Type | Description |
|---|---|---|
| `data` | `MachineData` | SO reference — the machine's static definition |
| `cell` | `Vector2Int` | Grid position |
| `gameObject` | `GameObject` | The scene instance |
| `activeRecipe` | `RecipeData` | Which recipe is currently selected (null = none) |

---

### MachinePlaceholderView
**File:** `Assets/Scripts/Core/MachinePlaceholderView.cs`  
**Component on:** `MachinePlaceholder.prefab`  
**Requires:** `SpriteRenderer`

Called by `GridManager.TryPlace` immediately after instantiation.

**`Init(MachineData data)` logic:**
- If `data.sprite != null` → sets that sprite directly (uses real art)
- If `data.sprite == null` → generates a 32×32 white texture at runtime and tints it with the tier color

**Tier tint colors (placeholder only):**

| Tier | Color |
|---|---|
| Steam | Bronze-ish `(0.76, 0.60, 0.42)` |
| LV | Steel grey `(0.55, 0.55, 0.60)` |
| MV | Aluminium blue `(0.40, 0.70, 0.85)` |
| HV | Gold `(0.95, 0.80, 0.20)` |

When real sprites land: assign `sprite` in the `MachineData` SO → `Init` picks the real sprite automatically, no code change.

---

### RandomFloorTile *(also a ScriptableObject)*
**File:** `Assets/Scripts/Core/RandomFloorTile.cs`  
Documented under §3 ScriptableObjects. The script inherits `TileBase` so Unity treats it as both a tile asset and a runtime object.

---

### InventorySlot
**File:** `Assets/Scripts/Data/InventorySlot.cs`  
**Type:** Plain C# class (serializable, not a MonoBehaviour or SO)

One slot = one item type + quantity. Lives inside `PlayerInventory` arrays.

| Field | Type | Description |
|---|---|---|
| `item` | `ItemData` | What's in the slot; null = empty |
| `quantity` | `int` | Stack count |
| `IsEmpty` | `bool` (get) | True if item is null or quantity ≤ 0 |

Key methods: `Set(item, qty)`, `Clear()`, `Matches(item)`.

---

### PlayerInventory
**File:** `Assets/Scripts/Core/PlayerInventory.cs`  
**GameObject:** `PlayerInventory` in GameScene  
**Pattern:** Singleton (`PlayerInventory.Instance`)

Owns all player-held items. UI and placement systems read from and write to this — never to each other.

**Slot layout:**
- Hotbar: `InventorySlot[8]` — indices 0–7
- Inventory: `InventorySlot[36]` — 4 rows × 9 columns

**Events:**

| Event | Fires when |
|---|---|
| `OnInventoryChanged` | Any slot content changes (add, consume, move) |
| `OnHotbarSelectionChanged(int)` | Selected slot index changes; -1 = nothing selected |

**Public API:**

| Method | Description |
|---|---|
| `SelectHotbarSlot(int)` | Selects slot, or deselects if already selected (toggle) |
| `ClearSelection()` | Sets selected index to -1 |
| `GetSelectedItem()` | Returns ItemData of selected slot, or null |
| `GetHotbarSlot(int)` | Direct slot accessor |
| `GetInventorySlot(int)` | Direct slot accessor |
| `TryAddItem(item, qty)` | Stacks into existing slot first, then fills empty; hotbar before inventory |
| `TryConsumeSelected()` | Removes 1 from selected hotbar slot; clears slot if it hits 0 |
| `TryConsumeFromHotbar(index, qty)` | Removes qty from specific hotbar slot |
| `MoveInventoryToHotbar(invIndex)` | Moves inventory slot to first empty hotbar slot; swaps with selected if full |

---

### InventorySlotUI
**File:** `Assets/Scripts/UI/InventorySlotUI.cs`  
**Used by:** `HotbarUI` (8 instances) and `InventoryScreenUI` (36 instances)  
**Requires:** `Button` component

Shared visual component for a single slot. Inspector fields assigned by `InventoryUISetupEditor`.

| Field | Type | Set by |
|---|---|---|
| `iconImage` | `Image` | Editor script |
| `countLabel` | `TMP_Text` | Editor script |
| `highlight` | `Image` | Editor script |

**Setup flow:**  
`Setup(index, onTap)` — called once from `HotbarUI.Start()` or `InventoryScreenUI.Start()`. Wires the Button onClick to `onTap(index)`. Never call this at edit time.

**Refresh flow:**  
`Refresh(slot, selected)` — updates icon visibility, count label text, and highlight enabled state.

---

### HotbarUI
**File:** `Assets/Scripts/UI/HotbarUI.cs`  
**GameObject:** `HotbarPanel` (child of `GameCanvas`)  
**Pattern:** Singleton (`HotbarUI.Instance`)

Manages 8 `InventorySlotUI` views. Subscribes to `PlayerInventory` events in `Start()`.

**Slot tap flow:** `InventorySlotUI.Button.onClick` → `HotbarUI.OnSlotTapped(index)` → `PlayerInventory.SelectHotbarSlot(index)` → `OnHotbarSelectionChanged` event → `HotbarUI.Refresh()`.

Inspector field `slots[]` (size 8) wired by `InventoryUISetupEditor`.

---

### InventoryScreenUI
**File:** `Assets/Scripts/UI/InventoryScreenUI.cs`  
**Component on:** `GameCanvas` in GameScene  
**Pattern:** Singleton (`InventoryScreenUI.Instance`)

Manages the inventory panel (36 slots) and the pickup confirmation panel.

**Inspector fields (all set by editor script):**

| Field | Type | Description |
|---|---|---|
| `inventoryPanel` | `GameObject` | Full-screen inventory panel; toggled on/off |
| `slots` | `InventorySlotUI[36]` | Grid slot views |
| `pickupConfirmPanel` | `GameObject` | Confirmation panel shown on long-press |
| `pickupConfirmLabel` | `TMP_Text` | "Pick up [machine name]?" label |

**Inventory slot tap flow:** `OnSlotTapped(index)` → `PlayerInventory.MoveInventoryToHotbar(index)`.

**Pickup confirm flow:**  
1. `PlacementController.HandleLongPress` → `ShowPickupConfirm(cell, machineName)` → panel appears  
2. Player taps "PICK UP" → `OnPickupConfirmed()` → finds ItemData for machine via `placeableMachine` reverse-lookup → `PlayerInventory.TryAddItem` → `GridManager.TryRemove`  
3. Player taps "CANCEL" → `OnPickupCancelled()` → panel hides

---

### MilestoneTrackerUI  *(§10)*
**File:** `Assets/Scripts/UI/MilestoneTrackerUI.cs`  
**Component on:** `GameCanvas`  
**Pattern:** Singleton (`MilestoneTrackerUI.Instance`)

Full-screen overlay that shows all milestones tracked by `MilestoneManager`. Opened via `MilestonesBtn` in the Toolbar.

**Inspector fields:**

| Field | Type | Description |
|---|---|---|
| `panel` | `GameObject` | `MilestoneTrackerPanel` — shown/hidden by Toggle/Close |
| `contentRoot` | `Transform` | `ScrollView/Viewport/Content` — parent for spawned cards |

**Public API:**

| Method | Description |
|---|---|
| `Toggle()` | Show/hide the panel (called by MilestonesBtn) |
| `Close()` | Hide the panel (called by header close button) |

**Runtime behavior:**
- `Start()` subscribes to `MilestoneManager.OnMilestoneUnlocked` and calls `BuildCards()`.
- `BuildCards()` creates one `MilestoneNodeUI` card per entry in `MilestoneManager.AllMilestones`. Cards are built entirely in code (no prefab required) using `VerticalLayoutGroup`, `Image`, and `TextMeshProUGUI`.
- On `OnMilestoneUnlocked`, calls `Refresh()` on all cards so colors update in real time.

**Scene hierarchy under `GameCanvas`:**
```
MilestoneTrackerPanel     ← full-screen overlay (hidden by default)
├── Header                ← dark bar: "MILESTONES" title + ✕ close button
└── ScrollView            ← vertical scroll, Mask on Viewport
    └── Viewport
        └── Content       ← VerticalLayoutGroup + ContentSizeFitter
            └── [Card_*]  ← spawned at runtime per MilestoneData SO
```

---

### MilestoneNodeUI  *(§10)*
**File:** `Assets/Scripts/UI/MilestoneNodeUI.cs`  
**Component on:** card GameObjects created by `MilestoneTrackerUI.BuildCards()`

Displays one milestone as a tappable card.

**Serialized fields (set via reflection by `MilestoneTrackerUI`):**
`background` (Image), `nameLabel`, `conditionLabel`, `rewardLabel` (TMP_Text), `detailPanel` (GameObject)

**State colors:**

| State | Background Color |
|---|---|
| Locked (prerequisites unmet) | Dark grey `(0.22, 0.22, 0.26)` |
| Available (prereqs met, not yet triggered) | Light grey `(0.75, 0.75, 0.80)` |
| Unlocked | Green `(0.20, 0.65, 0.30)` |

**`Init(MilestoneData)`** — sets labels, calls `Refresh()`, hides detail panel.  
**`Refresh()`** — re-reads `MilestoneManager.IsUnlocked` + prerequisites to pick the correct color.  
**`OnTap()`** — toggles the detail panel (shows reward text).

---

### WorkshopButtonUI
**File:** `Assets/Scripts/UI/WorkshopButtonUI.cs`  
**Component on:** `WorkshopBtn` in Toolbar

Keeps the Workshop toolbar button grayed out (`Button.interactable = false`) until a Workshop machine is placed on the grid. Subscribes to `GridManager.OnMachinePlaced` / `OnMachineRemoved` events.

**Inspector fields:**

| Field | Type | Description |
|---|---|---|
| `workshopData` | `MachineData` | The Workshop SO — button enables only when this exact machine is on the grid |

**Wire-up:** drag the Workshop `MachineData` SO into `workshopData` in the inspector. The button is disabled by default (no Workshop placed at start).

---

### NodeInstance
**File:** `Assets/Scripts/Core/NodeInstance.cs`  
Plain serializable runtime class — one per node slot.

| Field | Type | Description |
|---|---|---|
| `cell` | `Vector2Int` | Grid cell this node occupies |
| `isWaterNode` | `bool` | True for the special Water node (immovable, no panel) |
| `assignedResource` | `ItemData` | Currently assigned ore; null = unassigned |
| `hasExtractor` | `bool` | True when an Extractor is placed on this node |
| `view` | `NodeView` | Runtime ref to the scene-side view component |

---

### NodeManager
**File:** `Assets/Scripts/Core/NodeManager.cs`  
**GameObject:** `NodeManager` in GameScene  
**Pattern:** Singleton (`NodeManager.Instance`)

Owns all node instances. Spawns node GameObjects at Start, tracks unlocked resources, routes tap → NodeSelectionPanel.

**Inspector fields:**

| Field | Type | Description |
|---|---|---|
| `nodeDefinitions` | `List<NodeDefinition>` | Fixed cell positions + isWaterNode flag for each slot |
| `nodePrefab` | `GameObject` | Prefab with SpriteRenderer + NodeView |
| `resourceEntries` | `List<NodeResourceEntry>` | Maps each ItemData → its node sprite |
| `startingResources` | `List<ItemData>` | Resources unlocked at start (Coal, Copper Ore, Tin Ore) |
| `emptyNodeSprite` | `Sprite` | `node_empty` sprite shown when unassigned |

**Steam tier node positions:** (3,5) · (8,10) · (12,5)

**Public API:**

| Method | Returns | Description |
|---|---|---|
| `GetNodeAt(cell)` | `NodeInstance` | Returns node at cell or null |
| `IsNodeCell(cell)` | `bool` | True if a node exists at that cell |
| `TryOpenNodePanel(cell)` | void | Opens NodeSelectionPanel for the node at cell (no-op for water nodes) |
| `AssignResource(node, item)` | void | Sets node.assignedResource, refreshes view, fires OnNodeAssigned |
| `GetNodeSprite(item)` | `Sprite` | Looks up node sprite for an ItemData; returns emptyNodeSprite if not found |
| `UnlockResource(item)` | void | Adds resource to the unlocked list (called by MilestoneManager) |

**Events:** `OnNodeAssigned(NodeInstance)` — fires when player selects an ore for a node.

---

### NodeView
**File:** `Assets/Scripts/Core/NodeView.cs`  
**Requires:** `SpriteRenderer`

Attached to each spawned node GO. Displays the current assigned resource's node sprite (or empty sprite). Sorting order = 0 (below machines at order 1).

---

### NodeSelectionPanel
**File:** `Assets/Scripts/UI/NodeSelectionPanel.cs`  
**Pattern:** Singleton; hides via `CanvasGroup` (alpha/interactable/blocksRaycasts) so Awake always runs.

Opens when player taps an unoccupied node with no hotbar item selected. Rebuilds entry list each open from `NodeManager.UnlockedResources`. Selecting an entry calls `NodeManager.AssignResource` and closes.

**Inspector fields:** `entryPrefab`, `entryContainer` (ScrollView Content), `closeButton`.

---

### NodeSelectionEntry
**File:** `Assets/Scripts/UI/NodeSelectionEntry.cs`  
**Prefab:** `Assets/Prefabs/Nodes/NodeSelectionEntry.prefab`

Single row in the NodeSelectionPanel list. HorizontalLayoutGroup: 32×32 Icon (Image) + flexible Label (TextMeshProUGUI). `Setup(resource, sprite, callback)` wires the Button.onClick.

---

### PlacementController
**File:** `Assets/Scripts/Core/PlacementController.cs`  
**GameObject:** `PlacementController` in GameScene  
**Execution order:** -50 (after InputReader -100, before default 0)

Subscribes to `InputReader` events in `Start()` and translates them into grid actions.

**Tap → Place flow:**
1. `InputReader.OnTap(screenPos)`
2. `CameraController.ScreenToCell(screenPos)` → null if off-grid → return
3. `PlayerInventory.SelectedHotbarIndex` → -1 → return
4. `slot.item.placeableMachine` → null (non-placeable item) → return
5. `GridManager.IsCellEmpty(cell)` → false → return
6. `PlayerInventory.TryConsumeFromHotbar(index)` → places machine
7. `GridManager.TryPlace(machine, cell)`
8. If slot emptied → `PlayerInventory.ClearSelection()`

**Long press → Pickup flow:**
1. `InputReader.OnLongPress(screenPos)`
2. `CameraController.ScreenToCell` → null → return
3. `GridManager.GetAt(cell)` → null (no machine) → return
4. `InventoryScreenUI.ShowPickupConfirm(cell, machineName)`

---

### PlacedPipe
**File:** `Assets/Scripts/Core/PlacedPipe.cs`  
**Type:** Plain C# class (not a MonoBehaviour)

Runtime snapshot of one placed pipe. Lives in `GridManager._pipes`.

| Field | Type | Description |
|---|---|---|
| `data` | `PipeData` | SO reference — color, layer |
| `cell` | `Vector2Int` | Grid position |
| `gameObject` | `GameObject` | The scene instance (has `PipeView`) |
| `portAssignments` | `Dictionary<Vector2Int, PortDirection>` | Key = adjacent machine cell; value = Input or Output |

**`PortDirection` enum** (defined in `PlacedPipe.cs`): `Input, Output`  
Port assignments are set at runtime by the player via `PipePortPanel`.

---

### PipeView
**File:** `Assets/Scripts/Core/PipeView.cs`  
**Added to:** each pipe GameObject by `GridManager.TryPlacePipe`  
**Requires:** `SpriteRenderer` on the same GameObject (the center body)

Placeholder pipe visual: a colored center square + 4 directional arm sprites. Arms are enabled/disabled based on the bitmask of same-color same-layer neighbors.

**API:**

| Method | Description |
|---|---|
| `Build(PipeColor)` | Called once after instantiation. Creates center renderer + 4 arm child GOs, tints all to the pipe color. |
| `UpdateConnections(n, e, s, w)` | Enables/disables arm GOs based on which cardinal directions have a matching neighbor. |

**Sorting order:** center = 2, arms = 2. Above machines (order 1), below UI.

**Color table:**

| PipeColor | RGBA |
|---|---|
| White | (200, 200, 200) |
| Red | (220, 60, 60) |
| Green | (60, 200, 60) |
| Blue | (60, 120, 220) |
| Yellow | (220, 200, 60) |

When real pipe sprites land, replace `PipeView.Build` to assign sprites from an atlas — the `UpdateConnections` interface stays the same.

---

### TickManager
**File:** `Assets/Scripts/Core/TickManager.cs`  
**GameObject:** `TickManager` in GameScene  
**Pattern:** Singleton (`TickManager.Instance`)  
**Created by:** `HF > Setup Pipe System`

Fires a global tick every `tickInterval` seconds (default 1s). All time-based game systems (pipes, machines, extractors) subscribe to `OnTick` instead of using `Update`.

| Event | Fires when |
|---|---|
| `OnTick` | Every `tickInterval` seconds (default 1.0s) |

Inspector field `tickInterval` — adjust in editor for debug speed-up; never hardcode tick timing in subscribers.

---

### PipeNetwork
**File:** `Assets/Scripts/Core/PipeNetwork.cs`  
**GameObject:** `PipeNetwork` in GameScene  
**Pattern:** Singleton (`PipeNetwork.Instance`)  
**Created by:** `HF > Setup Pipe System`

Maintains the adjacency graph of all same-color same-layer pipe segments and drives GT-style item transit (items are invisible in the pipe; they arrive after N ticks = N cells traveled).

**How the graph is maintained:**
- Subscribes to `GridManager.OnPipePlaced` / `OnPipeRemoved`
- On place: scans 4 cardinal neighbors; adds bidirectional edge for each same-color same-layer neighbor
- On remove: removes all edges to/from that cell

**BFS routing (`FindPath`):**

```
FindPath(sourceMachineCell, sinkMachineCell, color, layer)
→ Collect start cells: pipes adjacent to source that have portAssignment Output toward source
→ Collect goal cells: pipes adjacent to sink that have portAssignment Input toward sink
→ BFS through same-color same-layer pipe graph
→ Return ordered List<Vector2Int> of pipe cells, or null if unreachable
```

**Item transit (`DispatchItem` + `Tick`):**

```
DispatchItem(item, path, destinationMachine)
→ Queues a TransitEntry: { destination, item, ticksLeft = path.Count }

Tick (via TickManager.OnTick):
→ Decrements ticksLeft for all entries
→ On ticksLeft == 0: fires OnItemArrived(destinationMachine, item) and removes entry
```

**Events:**

| Event | Signature | Fires when |
|---|---|---|
| `OnItemArrived` | `Action<Vector2Int, ItemData>` | An in-transit item reaches its destination machine cell |

Machines (Section 8) subscribe to `OnItemArrived` to pull items into their input buffer.

---

### PipePortPanel
**File:** `Assets/Scripts/UI/PipePortPanel.cs`  
**Pattern:** Singleton (`PipePortPanel.Instance`)  
**Created by:** `HF > Setup Pipe System` (bare GameObject — UI fields must be wired manually)

Opens when player taps an existing pipe with no hotbar item selected. Shows one toggle button per adjacent machine connection, letting the player cycle between `Input` and `Output`.

**Inspector fields (wire manually in the scene):**

| Field | Type | Description |
|---|---|---|
| `panel` | `GameObject` | Root panel to show/hide |
| `titleText` | `TextMeshProUGUI` | "Pipe Ports [x,y]" label |
| `entryContainer` | `Transform` | Parent for entry rows |
| `entryPrefab` | `GameObject` | Row prefab: TMP_Text label + Button |

All fields are null-safe — the component compiles and runs without them (ports are toggled in memory even if UI is not wired yet).

**Open flow:**
1. `PlacementController.HandleTap` → no hotbar item, pipe at cell → `PipePortPanel.Instance.Open(cell)`
2. Panel scans 4 neighbors for machines; shows one row per adjacent machine
3. Row label = "North/South/East/West: Input/Output"
4. Button toggles `PlacedPipe.portAssignments[machineCell]` between `Input` and `Output`

---

### MachineState
**File:** `Assets/Scripts/Core/MachineState.cs`

```csharp
public enum MachineState { Idle, Processing, OutputFull, NoInput, Halted }
```

| State | Meaning |
|---|---|
| `Idle` | Has a recipe selected, waiting for inputs |
| `Processing` | Inputs consumed, timer counting down |
| `OutputFull` | Cycle finished but output pipe is full — halted until path clears |
| `NoInput` | No recipe or inputs missing |
| `Halted` | No node (Extractor), no recipe, or explicitly stopped |

---

### MachineInstance
**File:** `Assets/Scripts/Core/MachineInstance.cs`  
**Component on:** machine GameObjects (added by `GridManager.TryPlace`)

MonoBehaviour attached to every placed machine GO. Drives the per-machine state machine and buffers.

**Fields (set by GridManager after instantiation):**

| Field | Type | Description |
|---|---|---|
| `cell` | `Vector2Int` | Grid position (set via `Init`) |
| `State` | `MachineState` | Current machine state |
| `Progress` | `float` | 0–1 progress through the current processing cycle |
| `InputBuffer` | `Dictionary<ItemData, int>` | Items waiting to be consumed |
| `OutputBuffer` | `Queue<ItemData>` | Items produced, waiting for dispatch |

**Public API:**

| Method | Description |
|---|---|
| `Init(MachineData, Vector2Int)` | Called once by GridManager after placement; subscribes to TickManager and PipeNetwork |
| `CanAcceptItem(ItemData)` | Returns true if the active recipe needs this item and the buffer has room |
| `SetRecipe(RecipeData)` | Changes active recipe; clears buffers and resets timer |

**Tick logic (state machine):**

```
Idle:      If recipe set and all inputs buffered → consume inputs → Processing
Processing: Decrement timer each tick → on complete → push to OutputBuffer → OutputFull
OutputFull: Try TryDispatchOutputs → if dispatched → Idle; else stay OutputFull
```

**`TryDispatchOutputs`:** calls `PipeNetwork.FindPathToAcceptor` for each item in OutputBuffer. If a path exists, calls `PipeNetwork.DispatchItem`. If no path, state stays `OutputFull`.

**Item arrival:** subscribes to `PipeNetwork.OnItemArrived`. Filters by cell. If `CanAcceptItem`, increments InputBuffer. If buffer didn't have room, item is lost (prototype simplification).

---

### ExtractorInstance
**File:** `Assets/Scripts/Core/ExtractorInstance.cs`  
**Extends:** `MachineInstance`

Extractor variant — no recipe, no input buffer. Produces 1× the node's resource per tick and dispatches it to the nearest accepting machine via pipe.

**Public API:**

| Method | Description |
|---|---|
| `SetNode(NodeInstance)` | Wires to a node; sets `node.hasExtractor = true`; calls `node.view.Refresh()` |
| `ClearNode()` | Detaches from node; sets `hasExtractor = false`; state → Halted |

**Tick logic:**
1. If no node or no assigned resource → `Halted`
2. `PipeNetwork.FindPathToAcceptor(cell, item)` → no path → `OutputFull`
3. Path found → `PipeNetwork.DispatchItem` → state `Processing`

**Wiring:** `NodeManager` subscribes to `GridManager.OnMachinePlaced` and calls `SetNode` automatically when an Extractor lands on a node cell. `ClearNode` is called on `OnMachineRemoved`.

---

### MachineInfoPanel
**File:** `Assets/Scripts/UI/MachineInfoPanel.cs`  
**Component on:** `MachineInfoPanel_Host` in GameScene  
**Pattern:** Singleton (`MachineInfoPanel.Instance`)

Shows runtime info for a tapped machine. Opened by `PlacementController` when player taps a machine cell with no hotbar item selected.

**Inspector fields (all wired by `HF > Setup Machine Info Panel`):**

| Field | Type | Description |
|---|---|---|
| `panel` | `GameObject` | Root panel under GameCanvas; hidden by default |
| `nameLabel` | `TextMeshProUGUI` | Machine name |
| `stateLabel` | `TextMeshProUGUI` | Current `MachineState` string; live-updated every frame |
| `progressBar` | `Slider` | 0–1 fill; live-updated every frame |
| `recipeContainer` | `Transform` | HorizontalLayoutGroup parent for recipe buttons |
| `recipeButtonPrefab` | `GameObject` | Button prefab with TMP_Text label |
| `removeButton` | `Button` | → `OnRemove` → `InventoryScreenUI.ShowPickupConfirm` |
| `closeButton` | `Button` | → `Close` |

**Open/close flow:**
- `Open(PlacedMachine)` — sets `_current`, shows panel, calls `Refresh` + `RebuildRecipeButtons`
- `Close()` — clears `_current`, hides panel
- `Update()` — live-updates `stateLabel` and `progressBar` each frame while open

**Recipe button flow:** one button per recipe in `data.availableRecipes`. Tap → `instance.SetRecipe(r)` → `Refresh()`.

**Remove flow:** `OnRemove` → `InventoryScreenUI.ShowPickupConfirm(cell, machineName)` (same confirm dialog as long-press pickup).

---

### InputReader
**File:** `Assets/Scripts/Core/InputReader.cs`  
**GameObject:** `InputReader` in GameScene  
**Pattern:** Singleton (`InputReader.Instance`)  
**Execution Order:** -100 (runs before CameraController and all placement code via `[DefaultExecutionOrder(-100)]`)

Pointer state machine that unifies touch and mouse input. Other systems subscribe to its events or read its per-frame properties — nothing else should call `Input` or `Mouse.current` directly.

**State machine:**

```
None
 → pointer down → MaybeTap (records position + time)

MaybeTap
 → moved > dragThreshold px → Dragging
 → held > longPressTime without moving → fires OnLongPress → None
 → released without moving → fires OnTap → None

Dragging (single finger or right-click)
 → second finger down → Pinching
 → released → None

Pinching (two fingers)
 → one finger lifted → None (CameraController will pick up remaining single touch next frame)
```

**Events:**

| Event | Argument | When fired |
|---|---|---|
| `OnTap` | `Vector2` screen position | Pointer released with movement ≤ `dragThreshold` pixels |
| `OnLongPress` | `Vector2` screen position (press origin) | Held ≥ `longPressTime` seconds without moving |

**Per-frame outputs (read by CameraController in LateUpdate):**

| Property | Type | Description |
|---|---|---|
| `ScreenDragDelta` | `Vector2` | Screen-pixel movement this frame; zero when not dragging |
| `PinchScreenDelta` | `float` | Change in pinch distance this frame; positive = fingers spreading |
| `ScrollZoomDelta` | `float` | Pre-scaled scroll value (`-scrollY × sensitivity × deltaTime`); feed directly into CameraController zoom |
| `IsDragging` | `bool` | True when in Dragging state |
| `IsPinching` | `bool` | True when in Pinching state |

**Inspector fields:**

| Field | Default | Description |
|---|---|---|
| `dragThreshold` | 10 px | Pixel movement before MaybeTap → Dragging |
| `longPressTime` | 0.5 s | Hold time before OnLongPress fires |
| `scrollZoomSensitivity` | 3 | How much each scroll-wheel tick changes orthographic size. Tune this in the Inspector. |

**Touch vs mouse mapping:**

| Action | Touch | Mouse |
|---|---|---|
| Tap (place/select) | Single finger, quick lift | Left-click, no drag |
| Pan | Single finger drag | Right-click drag |
| Zoom | Two-finger pinch | Scroll wheel (in CameraController) |
| Long press | Single finger hold | *(not mapped — mouse has right-click context)* |

**Uses `UnityEngine.InputSystem.EnhancedTouch`** — enabled/disabled in `OnEnable`/`OnDisable`.

---

### CameraController
**File:** `Assets/Scripts/Core/CameraController.cs`  
**Component on:** `Main Camera` in GameScene  
**Requires:** `Camera` component, `InputReader` in scene  
**Update:** runs in `LateUpdate` so InputReader state is settled first

Reads InputReader each frame, applies zoom and pan, clamps camera within grid bounds.

**Inspector fields:**

| Field | Default | Description |
|---|---|---|
| `minZoom` | 4 | Minimum orthographic size (closest zoom) |
| `maxZoom` | 12 | Maximum orthographic size (furthest zoom — fits 16×16 grid) |
| `pinchZoomSpeed` | 0.05 | Zoom delta per screen-pixel of pinch change |
| `gridWidth` | 16 | Must match GridManager.width |
| `gridHeight` | 16 | Must match GridManager.height |
| `maxOverpan` | 4 | World units (tiles) the viewport can travel past the grid edge on any side. Same value applies to all four sides — aspect-ratio agnostic. |

**Zoom logic:**
- Pinch: `orthographicSize -= PinchScreenDelta * pinchZoomSpeed` (spreading fingers = smaller size = zoom in)
- Scroll: `orthographicSize -= scrollY * scrollZoomSpeed * deltaTime`
- Always clamped to `[minZoom, maxZoom]`

**Pan logic:**
- `worldPerPixel = (orthographicSize × 2) / Screen.height`
- `cameraPosition += -ScreenDragDelta × worldPerPixel`
- Negative because dragging right moves the scene right → camera moves left

**Clamp logic — world-unit overpan:**
- `maxOverpan` (default 4 tiles) is the same on all four sides — aspect-ratio agnostic.
- Per axis: `camPos ∈ [ halfDim - maxOverpan, gridDim - halfDim + maxOverpan ]`
- This means the viewport edge can travel exactly `maxOverpan` tiles past the grid boundary in any direction, giving equal pan freedom regardless of screen shape.
- Safety fallback: if viewport is wider/taller than `gridDim + 2×maxOverpan` (extreme zoom-out on wide screens), camera centres on that axis.
- Area beyond the grid edge is intentionally exposed — art tracked in `ArtChecklist_Steam.md` → Environment.

**Public API:**

| Method | Returns | Description |
|---|---|---|
| `ScreenToCell(Vector2 screenPos)` | `Vector2Int?` | Screen → world → cell. Returns `null` if outside grid bounds. Used by placement and selection. |
| `CellToWorld(Vector2Int cell)` | `Vector3` | Convenience wrapper around GridManager.CellToWorld |

**ScreenToCell internals:**  
`distToWorld = Abs(camera.z)` (camera is at z=-10, world at z=0, so distance = 10)  
`world = Camera.ScreenToWorldPoint(screenPos.xy, distToWorld)` → world.z forced to 0  
Then delegates to `GridManager.WorldToCell` and `GridManager.IsInBounds`.

---

### MilestoneManager  *(§10)*
**File:** `Assets/Scripts/Core/MilestoneManager.cs`  
**GameObject:** `MilestoneManager` in GameScene  
**Pattern:** Singleton (`MilestoneManager.Instance`)

Tracks which milestones are unlocked. Listens for item-produced and machine-placed events, checks trigger conditions and prerequisite chains, then fires `OnMilestoneUnlocked`. The Milestone Tracker UI subscribes to this event.

**Inspector fields:**

| Field | Type | Description |
|---|---|---|
| `allMilestones` | `List<MilestoneData>` | All milestones in the game. Drag milestone SOs here. |

**Public API:**

| Member | Type | Description |
|---|---|---|
| `Instance` | `MilestoneManager` | Singleton accessor |
| `AllMilestones` | `IReadOnlyList<MilestoneData>` | All registered milestones |
| `IsUnlocked(m)` | `bool` | True if milestone has already fired |
| `OnMilestoneUnlocked` | `static event Action<MilestoneData>` | Fires whenever a new milestone is unlocked |

**Inputs listened to:**

| Event | Source | Action |
|---|---|---|
| `MachineInstance.OnItemProduced` | Any machine finishing a recipe | Checks item-triggered milestones |
| `GridManager.OnMachinePlaced` | Player placing a machine | Checks machine-triggered milestones |

**Unlock flow:**
1. `Start()` auto-fires all milestones with no `triggerItem` and no `triggerMachine` (e.g. S0).
2. When an event fires, iterates `allMilestones` looking for unmatched triggers.
3. `TryUnlock(m)` checks prerequisites; if all are in `_unlocked`, adds `m` and fires `OnMilestoneUnlocked`.
4. After each unlock, cascades to re-check auto-milestones whose prerequisites may now be satisfied.

**`MachineInstance.OnItemProduced`** — added to `MachineInstance` to support milestone detection. Fires once per output stack in `FinishProcessing`, carrying the `ItemData` produced and the `MachineData` that produced it.

---

## 5. Editor Scripts

Editor scripts live in `Assets/Scripts/Editor/` and are stripped from builds automatically. They only run in the Unity Editor.

---

### GameSceneBuilder
**File:** `Assets/Scripts/Editor/GameSceneBuilder.cs`  
**Menu:** `HF > Setup Game Scene`

Run once to bootstrap the project. Safe to re-run on a fresh scene.

**What it creates:**

| Object | Details |
|---|---|
| `Main Camera` | Orthographic, size 8, centered at `(8, 8, -10)` (center of 16×16 grid), dark background |
| `MachinePlaceholder.prefab` | Saved to `Assets/Prefabs/Machines/` — has SpriteRenderer + MachinePlaceholderView |
| `GridManager` GameObject | Has `GridManager` component; `machinePrefab` field wired to the prefab above via `SerializedObject` |
| `GameScene.unity` | Saved to `Assets/Scenes/` |

**Wiring detail:** uses `SerializedObject` + `FindProperty("machinePrefab")` to set the prefab reference on `GridManager` programmatically, so no manual drag is needed after running the menu item.

---

### FloorSetupEditor
**File:** `Assets/Scripts/Editor/FloorSetupEditor.cs`  
**Menu:** `HF > Setup Floor`

Run after `HF > Setup Game Scene`. Paints the floor Tilemap in the active scene.

**Step by step:**
1. **Configures `floor.png` import** — sets Point filter, no compression, PPU=32, Multiple sprites mode, slices into 32×32 cells (auto-detects column count from texture width)
2. **Loads sliced sprites** — finds all `Sprite` sub-assets of `floor.png`
3. **Creates `FloorTile.asset`** at `Assets/Data/` — a `RandomFloorTile` SO with all variants assigned
4. **Finds or creates `Grid` GameObject** in the active scene
5. **Finds or creates `Floor` child Tilemap** — sets `TilemapRenderer.sortingOrder = -10` (renders behind everything)
6. **Paints 16×16 cells** using the tile — each cell gets a deterministic random variant + rotation

**Constants (top of file):**

| Constant | Value | Meaning |
|---|---|---|
| `TileSize` | 32 | px per tile |
| `GridW / GridH` | 16 / 16 | Starting grid size — update when floor expands |

---

### InventoryUISetupEditor
**File:** `Assets/Scripts/Editor/InventoryUISetupEditor.cs`  
**Menu:** `HF > Setup Inventory UI`

Builds the full Canvas UI hierarchy in one click. Safe to re-run — uses `GetOrCreateChild` which skips existing objects.

**What it creates:** See Scene Hierarchy §7 for the full tree.

**Key wiring done by the script:**
- `HotbarUI.slots[]` ← array of 8 `InventorySlotUI` references (via `SerializedObject`)
- `InventoryScreenUI.slots[]` ← array of 36 `InventorySlotUI` references
- `InventoryScreenUI.inventoryPanel/pickupConfirmPanel/pickupConfirmLabel` ← child object refs
- All Button `onClick` persistent listeners (CloseButton, PickupBtn, CancelBtn, InventoryBtn in Toolbar)

**Layout constants (top of file):**

| Constant | Value | Meaning |
|---|---|---|
| `SlotSize` | 64px | Width and height of each slot |
| `SlotSpacing` | 4px | Gap between slots |
| `HotbarPadding` | 8px | Vertical padding inside hotbar |
| `ToolbarHeight` | 48px | Height of the bottom toolbar |

**Canvas settings:** ScaleWithScreenSize, reference 360×640, match width (matchWidthOrHeight = 0). This makes the UI scale correctly on portrait phones of any size.

---

### InputCameraSetupEditor
**File:** `Assets/Scripts/Editor/InputCameraSetupEditor.cs`  
**Menu:** `HF > Setup Input & Camera`

Run after `HF > Setup Game Scene`. Safe to re-run — skips components already present.

**What it does:**
1. Finds or creates `InputReader` GameObject in the active scene; adds `InputReader` component if missing
2. Finds `Main Camera`; adds `CameraController` component if missing
3. Sets `CameraController.gridWidth/Height = 16` via `SerializedObject` (mirrors GridManager default)
4. Marks scene dirty — **save the scene (Ctrl+S) after running**

---

### PipeSetupEditor
**File:** `Assets/Scripts/Editor/PipeSetupEditor.cs`  
**Menu:** `HF > Setup Pipe System`

Run once after `HF > Create Phase 1 SO Assets`. Safe to re-run — skips existing assets.

**What it creates:**
1. `PipeData` SO assets in `Assets/Data/Pipes/` — one per `PipeColor` (Item layer)
2. `ItemData` SO assets in `Assets/Data/Items/` — one per color with `placeablePipe` wired, so pipes can be held in the hotbar
3. `TickManager` GameObject in scene (skips if already present)
4. `PipeNetwork` GameObject in scene (skips if already present)
5. `PipePortPanel` GameObject in scene — **wire panel/prefab fields manually** (null-safe without wiring)

---

### MachineInfoPanelSetupEditor
**File:** `Assets/Scripts/Editor/MachineInfoPanelSetupEditor.cs`  
**Menu:** `HF > Setup Machine Info Panel`

Builds the `MachineInfoPanel` UI hierarchy under `GameCanvas` and creates `MachineInfoPanel_Host` with all fields wired. Safe to re-run — destroys and recreates child objects that lack a `RectTransform` (leftover from a failed previous run).

**What it creates under `GameCanvas`:**

| Child | Component | Notes |
|---|---|---|
| `MachineInfoPanel` | `Image` | Panel root; bottom-anchored, 200px tall, above hotbar |
| `MachineInfoPanel/NameLabel` | `TextMeshProUGUI` | Machine name, 16px bold |
| `MachineInfoPanel/StateLabel` | `TextMeshProUGUI` | State string, 12px green tint |
| `MachineInfoPanel/ProgressBar` | `Slider` | 0–1 progress |
| `MachineInfoPanel/RecipeContainer` | `HorizontalLayoutGroup` | Parent for recipe buttons |
| `MachineInfoPanel/RemoveButton` | `Button`, `Image` | Red; confirms pickup |
| `MachineInfoPanel/CloseButton` | `Button`, `Image` | Grey; closes panel |

Also creates `RecipeButton.prefab` at `Assets/Prefabs/UI/RecipeButton.prefab` if it doesn't exist (32×120px button with centred TMP label).

**Scene object:** `MachineInfoPanel_Host` — a bare GameObject (not under Canvas) that holds `MachineInfoPanel` component with all serialized refs wired via `SerializedObject`.

---

### SOSetupEditor
**File:** `Assets/Scripts/Editor/SOSetupEditor.cs`  
**Menu:** `HF > Create Phase 1 SO Assets`

Creates all game data SO assets for Phase 1. Safe to re-run — uses `GetOrCreate` which skips existing assets and re-applies the init action if they already exist.

**Step by step:**
1. Calls `ConfigureSheet` on both machine sprite sheets — same import settings as floor (Point, PPU=32, Multiple, 9 named frames: `extractor_0`…`extractor_8` and `smelter_0`…`smelter_8`)
2. Creates `ItemData` assets in `Assets/Data/Items/`
3. Creates `RecipeData` assets in `Assets/Data/Recipes/` — item references wired via the ItemData objects created in step 2
4. Loads `smeltCopper` and `smeltBronze` recipe assets
5. Creates `MachineData` assets in `Assets/Data/Machines/` — recipe arrays and idle sprites wired in

**Frame naming convention for machine sheets:**
- `extractor_0` = Steam Extractor idle
- `extractor_1` = halted
- `extractor_2` = selected
- `extractor_3` = startup (1 frame)
- `extractor_4` – `extractor_8` = active animation (5 frames)
- Same pattern for `smelter_*`

---

## 6. Prefabs

### MachinePlaceholder
**Path:** `Assets/Prefabs/Machines/MachinePlaceholder.prefab`  
**Created by:** `HF > Setup Game Scene`

| Component | Notes |
|---|---|
| `SpriteRenderer` | Displays the machine sprite or tinted square |
| `MachinePlaceholderView` | Called by GridManager on placement; sets sprite from MachineData |

This prefab is the template for every placed machine in Phase 1. When real machine GameObjects are built (Phase 2+), each machine type will get its own prefab with an Animator.

---

## 7. Scene Hierarchy

After running all seven `HF` menu items, `GameScene.unity` looks like this:

```
GameScene
├── Main Camera            [Camera, AudioListener, CameraController]
│     orthographic size 8, pos (8, 8, -10), background #121218
│     CameraController: minZoom=4, maxZoom=12, maxOverpan=4, gridWidth=16, gridHeight=16
│
├── InputReader            [InputReader]  ← execution order -100
│     dragThreshold=10px, longPressTime=0.5s, scrollZoomSensitivity=3
│
├── GridManager            [GridManager]
│     width=16, height=16, machinePrefab → MachinePlaceholder.prefab
│
├── TickManager            [TickManager]
│     tickInterval=1.0s
│
├── PipeNetwork            [PipeNetwork]
│
├── PipePortPanel          [PipePortPanel]
│     panel/titleText/entryContainer/entryPrefab — wire manually when UI prefab is built
│
├── MachineInfoPanel_Host  [MachineInfoPanel]  ← §8; all fields wired by Setup Machine Info Panel
│
├── PlayerInventory        [PlayerInventory]
├── PlacementController    [PlacementController]  ← execution order -50
│
├── Grid                   [Grid]
│     └── Floor            [Tilemap, TilemapRenderer]  sortingOrder=-10
│
└── GameCanvas             [Canvas, CanvasScaler, GraphicRaycaster, InventoryScreenUI]
      Canvas: ScreenSpaceOverlay, sortingOrder=10
      CanvasScaler: ScaleWithScreenSize, ref=360×640, matchWidth=1.0
      │
      ├── EventSystem      [EventSystem, InputSystemUIInputModule]
      │
      ├── Toolbar          [Image, HorizontalLayoutGroup]  ← bottom 48px
      │     ├── MilestonesBtn  [Button, Image, TMP_Text]
      │     ├── InventoryBtn   [Button, Image, TMP_Text]  → InventoryScreenUI.ToggleInventory
      │     ├── InspectBtn     [Button, Image, TMP_Text]  (stub)
      │     └── SettingsBtn    [Button, Image, TMP_Text]  (stub)
      │
      ├── HotbarPanel      [Image, HorizontalLayoutGroup, HotbarUI]  ← above toolbar (64px)
      │     └── HotbarSlot0–7  [Button, Image, InventorySlotUI]
      │           ├── Icon     [Image]
      │           ├── Count    [TMP_Text]
      │           └── Highlight [Image]
      │
      ├── InventoryPanel   [RectTransform]  ← full-screen, hidden by default
      │     ├── Background [Image]
      │     ├── Title      [TMP_Text]
      │     ├── SlotGrid   [GridLayoutGroup]  9 cols × 4 rows
      │     │     └── InvSlot0–35  [Button, Image, InventorySlotUI]
      │     └── CloseButton [Button, Image, TMP_Text]  → InventoryScreenUI.ToggleInventory
      │
      ├── MachineInfoPanel   [Image]  ← §8; hidden by default; opened on machine tap
      │     ├── NameLabel    [TextMeshProUGUI]
      │     ├── StateLabel   [TextMeshProUGUI]
      │     ├── ProgressBar  [Slider]
      │     ├── RecipeContainer [HorizontalLayoutGroup]
      │     ├── RemoveButton [Button, Image]
      │     └── CloseButton  [Button, Image]
      │
      └── PickupConfirmPanel [Image]  ← hidden, shown on long-press
            ├── Label      [TMP_Text]
            ├── PickupBtn  [Button]  → InventoryScreenUI.OnPickupConfirmed
            └── CancelBtn  [Button]  → InventoryScreenUI.OnPickupCancelled
```

Machines placed at runtime are instantiated as children of `GridManager`.

---

## 8. Wiring Reference

How the key objects reference each other:

```
InputReader (scene singleton)
  fires OnTap(screenPos)       ──→ placement / selection systems (Phase 1+)
  fires OnLongPress(screenPos) ──→ pick-up system (Phase 1+)
  ScreenDragDelta / PinchScreenDelta ──→ CameraController.LateUpdate

CameraController (on Main Camera)
  reads InputReader.ScreenDragDelta  → pan
  reads InputReader.PinchScreenDelta → zoom
  reads Mouse.scroll                 → zoom
  clamps within [0,16]×[0,16] world space
  ScreenToCell(screenPos) ──→ GridManager.WorldToCell + IsInBounds

MachineData SO ──────────────────┐
  .sprite        ──→ Sprite      │  (idle frame from sprite sheet)
  .availableRecipes[] ──→ RecipeData SO[]
                                 │
RecipeData SO                    │
  .inputs[].item  ──→ ItemData SO│
  .outputs[].item ──→ ItemData SO│
                                 │
GridManager (scene) ─────────────┤
  .machinePrefab  ──→ MachinePlaceholder.prefab
  ._grid[cell]    ──→ PlacedMachine (runtime class)
                         .data   ──→ MachineData SO
                         .gameObject ──→ scene instance
                         .activeRecipe ──→ RecipeData SO

MachinePlaceholder.prefab
  MachinePlaceholderView.Init(MachineData)
    → reads .sprite → sets SpriteRenderer.sprite

RandomFloorTile SO (FloorTile.asset)
  .variants[] ──→ Sprite[] (sliced from floor.png)
  used by Tilemap "Floor" in scene
```

---

## 9. Coordinate System

| Space | Origin | Unit |
|---|---|---|
| Grid | `(0,0)` = bottom-left cell | 1 cell |
| World | `(0,0)` = bottom-left corner of cell (0,0) | 1 Unity unit = 1 tile = 32 px |
| Screen | Unity default | pixels |

**Cell centre formula:** `CellToWorld(x, y) = (x + 0.5, y + 0.5, 0)`  
→ Cell `(0,0)` centre = world `(0.5, 0.5, 0)`  
→ Cell `(15,15)` centre = world `(15.5, 15.5, 0)`

**Camera position `(8, 8, -10)`** centres on the 16×16 grid (grid spans 0→16 on both axes, centre = 8,8).

When the grid expands (milestone FloorExpansion), `GridManager.width` and `GridManager.height` must be updated, `FloorSetupEditor` constants must match, and the camera target position must update to `(newSize/2, newSize/2, -10)`.

---

## 10. Sprite Import Conventions

All sprites use **Point (no filter)** and **no compression** — required for crisp pixel art.

| Setting | Value |
|---|---|
| Filter Mode | Point |
| Compression | None |
| Pixels Per Unit | 32 |
| Sprite Mode | Multiple (for sheets) / Single (for standalone) |
| Mesh Type | Full Rect |

**Machine sprite sheet layout (horizontal strip):**

```
[0: idle] [1: halted] [2: selected] [3: startup...] [N: active...]
```

Frame 0 is always the idle sprite. This is what `MachineData.sprite` references.  
Animator states (idle → active → halted) are wired up per-machine when real animation frames exist — see `Docs/SpriteSpecs.md` for frame counts per machine.

**Floor tile sheet layout:**
```
[variant_0] [variant_1]   ← 2 × 32×32 px
```
Both variants are passed to `RandomFloorTile.variants[]`.
