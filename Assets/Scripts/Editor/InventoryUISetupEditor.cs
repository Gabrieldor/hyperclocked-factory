using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/// Menu: HF > Setup Inventory UI
/// Builds the full inventory/hotbar Canvas hierarchy in the active scene.
/// Also adds PlayerInventory and PlacementController to the scene.
/// Safe to re-run — skips objects that already exist.
public static class InventoryUISetupEditor
{
    // ── Layout constants ─────────────────────────────────────────────────────
    private const float SlotSize       = 66f;
    private const float SlotSpacing    = 4f;
    private const float HotbarPadding  = 8f;
    private const float ToolbarHeight  = 48f;
    private const float HotbarHeight   = SlotSize + HotbarPadding * 2;

    // Colours
    private static readonly Color SlotBg        = new Color(0.12f, 0.12f, 0.15f, 0.95f);
    private static readonly Color HighlightCol  = new Color(1f,    0.8f,  0.2f,  1f);
    private static readonly Color PanelBg       = new Color(0.08f, 0.08f, 0.10f, 0.97f);
    private static readonly Color ToolbarBg     = new Color(0.06f, 0.06f, 0.08f, 1f);
    private static readonly Color ButtonBg      = new Color(0.20f, 0.20f, 0.25f, 1f);

    [MenuItem("HF/Setup Inventory UI")]
    public static void Setup()
    {
        // ── EventSystem ──────────────────────────────────────────────────────
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        // ── Canvas ───────────────────────────────────────────────────────────
        // Destroy any existing GameCanvas so we don't carry over partial state
        // from a previous failed run — the script is fully idempotent and rebuilds.
        var stale = GameObject.Find("GameCanvas");
        if (stale != null) Object.DestroyImmediate(stale);

        var canvasGo = new GameObject("GameCanvas");
        var canvas = canvasGo.GetOrAdd<Canvas>(); // ObjectFactory path → fully initialized
        canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        var scaler = canvasGo.GetOrAdd<CanvasScaler>();
        scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(360, 640);
        scaler.screenMatchMode     = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight  = 0f;

        canvasGo.GetOrAdd<GraphicRaycaster>();

        // ── Managers ─────────────────────────────────────────────────────────
        EnsureManager<PlayerInventory>("PlayerInventory");
        EnsureManager<PlacementController>("PlacementController");

        // ── Toolbar (very bottom) ─────────────────────────────────────────────
        var toolbar = GetOrCreatePanel(canvasGo, "Toolbar", ToolbarHeight, 0f);
        SetupToolbar(toolbar);

        // ── HotbarPanel (just above toolbar) ─────────────────────────────────
        float hotbarBottom = ToolbarHeight;
        var hotbarPanel = GetOrCreatePanel(canvasGo, "HotbarPanel", HotbarHeight, hotbarBottom);
        var hotbarUI    = hotbarPanel.GetOrAdd<HotbarUI>();
        // HorizontalLayoutGroup distributes 8 slots evenly across the panel width
        var hotbarHlg = hotbarPanel.GetOrAdd<HorizontalLayoutGroup>();
        hotbarHlg.childAlignment         = TextAnchor.MiddleCenter;
        hotbarHlg.spacing                = SlotSpacing;
        hotbarHlg.childControlWidth      = true;   // must be true for HLG to set child widths
        hotbarHlg.childControlHeight     = false;
        hotbarHlg.childForceExpandWidth  = true;
        hotbarHlg.childForceExpandHeight = false;
        hotbarHlg.padding = new RectOffset((int)HotbarPadding, (int)HotbarPadding, (int)HotbarPadding, (int)HotbarPadding);
        var hotbarSlots = BuildSlots(hotbarPanel, "HotbarSlot", PlayerInventory.HotbarSize,
                                     horizontal: true);
        SetPrivateSlotArray(hotbarUI, "slots", hotbarSlots);

        // ── InventoryPanel (full-screen overlay, hidden) ──────────────────────
        var invPanel = GetOrCreateFullscreenPanel(canvasGo, "InventoryPanel");
        invPanel.SetActive(false);

        // Background
        var bg = GetOrCreateChild(invPanel, "Background");
        SetRect(bg, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        bg.GetOrAdd<Image>().color = PanelBg;

        // Title
        var title = GetOrCreateChild(invPanel, "Title");
        SetRect(title, new Vector2(0, 1), Vector2.one, new Vector2(0, -60), new Vector2(0, -8));
        var titleTmp = title.GetOrAdd<TextMeshProUGUI>();
        titleTmp.text      = "INVENTORY";
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.fontSize  = 18;

        // Slot grid
        var gridGo = GetOrCreateChild(invPanel, "SlotGrid");
        SetRect(gridGo, new Vector2(0, 0), Vector2.one, new Vector2(8, 56), new Vector2(-8, -72));
        var layout = gridGo.GetOrAdd<GridLayoutGroup>();
        layout.cellSize        = new Vector2(SlotSize, SlotSize);
        layout.spacing         = new Vector2(SlotSpacing, SlotSpacing);
        layout.startCorner     = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis       = GridLayoutGroup.Axis.Horizontal;
        layout.childAlignment  = TextAnchor.UpperCenter;
        layout.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = PlayerInventory.InventoryCols;

        var invSlots = BuildSlots(gridGo, "InvSlot", PlayerInventory.InventorySize,
                                   horizontal: false);

        // Close button
        var closeBtn = GetOrCreateChild(invPanel, "CloseButton");
        SetRect(closeBtn, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(-40, 4), new Vector2(40, 52));
        closeBtn.GetOrAdd<Image>().color = ButtonBg;
        var closeBtnComp = closeBtn.GetOrAdd<Button>();

        var closeLabel = GetOrCreateChild(closeBtn, "Label");
        SetRect(closeLabel, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        var closeTmp = closeLabel.GetOrAdd<TextMeshProUGUI>();
        closeTmp.text      = "CLOSE";
        closeTmp.alignment = TextAlignmentOptions.Center;
        closeTmp.fontSize  = 14;

        // ── PickupConfirmPanel (hidden) ────────────────────────────────────────
        var confirmPanel = GetOrCreateChild(canvasGo, "PickupConfirmPanel");
        SetRect(confirmPanel, new Vector2(0.1f, 0.35f), new Vector2(0.9f, 0.55f),
                Vector2.zero, Vector2.zero);
        confirmPanel.GetOrAdd<Image>().color = PanelBg;
        confirmPanel.SetActive(false);

        var confirmLabel = GetOrCreateChild(confirmPanel, "Label");
        SetRect(confirmLabel, new Vector2(0, 0.5f), Vector2.one, new Vector2(8, 0), new Vector2(-8, -8));
        var confirmTmp = confirmLabel.GetOrAdd<TextMeshProUGUI>();
        confirmTmp.text      = "Pick up machine?";
        confirmTmp.alignment = TextAlignmentOptions.Center;
        confirmTmp.fontSize  = 16;

        var pickupBtn = MakeButton(confirmPanel, "PickupBtn", "PICK UP",
                                   new Vector2(0.05f, 0.05f), new Vector2(0.45f, 0.45f));
        var cancelBtn = MakeButton(confirmPanel, "CancelBtn", "CANCEL",
                                   new Vector2(0.55f, 0.05f), new Vector2(0.95f, 0.45f));

        // ── InventoryScreenUI ────────────────────────────────────────────────
        var invUI = canvasGo.GetOrAdd<InventoryScreenUI>();
        var so    = new SerializedObject(invUI);
        so.FindProperty("inventoryPanel").objectReferenceValue      = invPanel;
        so.FindProperty("pickupConfirmPanel").objectReferenceValue   = confirmPanel;
        so.FindProperty("pickupConfirmLabel").objectReferenceValue   = confirmTmp;
        SetSlotArray(so, "slots", invSlots);
        so.ApplyModifiedPropertiesWithoutUndo();

        // Wire InventoryScreenUI buttons via persistent listeners
        UnityEditor.Events.UnityEventTools.AddPersistentListener(
            closeBtnComp.onClick,
            invUI.ToggleInventory);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(
            pickupBtn.onClick,
            invUI.OnPickupConfirmed);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(
            cancelBtn.onClick,
            invUI.OnPickupCancelled);

        // Wire Inventory toolbar button (created above)
        var toolbarInvBtn = toolbar.transform.Find("InventoryBtn")?.GetComponent<Button>();
        if (toolbarInvBtn != null)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(
                toolbarInvBtn.onClick,
                invUI.ToggleInventory);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[HF] Inventory UI setup complete. Save scene (Ctrl+S).");
        EditorUtility.DisplayDialog("HF Setup",
            "Inventory UI created.\n\nRun: HF > Setup Inventory UI\n" +
            "Then save the scene (Ctrl+S) and hit Play.", "OK");
    }

    // ── Slot builder ─────────────────────────────────────────────────────────

    private static InventorySlotUI[] BuildSlots(GameObject parent, string prefix,
                                                 int count, bool horizontal)
    {
        var result = new InventorySlotUI[count];
        for (int i = 0; i < count; i++)
        {
            string name  = $"{prefix}{i}";
            var slotGo   = GetOrCreateChild(parent, name);
            var slotRect = slotGo.GetOrAdd<RectTransform>();

            if (horizontal)
            {
                slotRect.sizeDelta = new Vector2(SlotSize, SlotSize);
            }

            // Slot background
            var slotImg  = slotGo.GetOrAdd<Image>();
            slotImg.color = SlotBg;
            slotGo.GetOrAdd<Button>(); // Button required by InventorySlotUI

            // Icon
            var iconGo  = GetOrCreateChild(slotGo, "Icon");
            SetRect(iconGo, new Vector2(0.1f, 0.1f), new Vector2(0.9f, 0.9f),
                    Vector2.zero, Vector2.zero);
            var iconImg = iconGo.GetOrAdd<Image>();
            iconImg.preserveAspect = true;
            iconImg.enabled = false;

            // Count label
            var labelGo  = GetOrCreateChild(slotGo, "Count");
            SetRect(labelGo, new Vector2(0.5f, 0), Vector2.one, new Vector2(0, 2), new Vector2(-2, -2));
            var label    = labelGo.GetOrAdd<TextMeshProUGUI>();
            label.alignment = TextAlignmentOptions.BottomRight;
            label.fontSize  = 11;
            label.text      = "";

            // Highlight border
            var hlGo    = GetOrCreateChild(slotGo, "Highlight");
            SetRect(hlGo, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var hlImg   = hlGo.GetOrAdd<Image>();
            hlImg.color   = HighlightCol;
            hlImg.enabled = false;
            // Make hollow by using a transparent center — outline only
            hlImg.type    = Image.Type.Sliced;

            // Wire InventorySlotUI
            var slotUI       = slotGo.GetOrAdd<InventorySlotUI>();
            var slotSO       = new SerializedObject(slotUI);
            slotSO.FindProperty("iconImage").objectReferenceValue  = iconImg;
            slotSO.FindProperty("countLabel").objectReferenceValue = label;
            slotSO.FindProperty("highlight").objectReferenceValue  = hlImg;
            slotSO.ApplyModifiedPropertiesWithoutUndo();

            result[i] = slotUI;
        }
        return result;
    }

    // ── Toolbar ───────────────────────────────────────────────────────────────

    private static void SetupToolbar(GameObject toolbar)
    {
        toolbar.GetOrAdd<Image>().color = ToolbarBg;
        var hlg = toolbar.GetOrAdd<HorizontalLayoutGroup>();
        hlg.childAlignment        = TextAnchor.MiddleCenter;
        hlg.spacing               = 0;
        hlg.childControlWidth     = true;   // must be true for HLG to set child widths
        hlg.childControlHeight    = true;
        hlg.childForceExpandWidth  = true;
        hlg.childForceExpandHeight = true;
        hlg.padding               = new RectOffset(4, 4, 4, 4);

        string[] labels = { "Milestones", "Inventory", "Inspect", "Settings" };
        string[] names  = { "MilestonesBtn", "InventoryBtn", "InspectBtn", "SettingsBtn" };
        for (int i = 0; i < labels.Length; i++)
        {
            var btn = GetOrCreateChild(toolbar, names[i]);
            btn.GetOrAdd<Image>().color = ButtonBg;
            btn.GetOrAdd<Button>();
            // Allow HLG to freely size this button — preferredWidth=0 lets it shrink, flexibleWidth=1 distributes space equally
            var le = btn.GetOrAdd<LayoutElement>();
            le.minWidth      = 0;
            le.preferredWidth = 0;
            le.flexibleWidth  = 1;
            var lbl = GetOrCreateChild(btn, "Label");
            SetRect(lbl, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var tmp = lbl.GetOrAdd<TextMeshProUGUI>();
            tmp.text      = labels[i];
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize  = 11;
        }
    }

    // ── UI helpers ────────────────────────────────────────────────────────────

    private static GameObject GetOrCreatePanel(GameObject canvas, string name,
                                                float height, float bottomOffset)
    {
        var go   = GetOrCreateChild(canvas, name);
        var rect = go.GetOrAdd<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.offsetMin = new Vector2(0, bottomOffset);
        rect.offsetMax = new Vector2(0, bottomOffset + height);
        return go;
    }

    private static GameObject GetOrCreateFullscreenPanel(GameObject canvas, string name)
    {
        var go = GetOrCreateChild(canvas, name);
        SetRect(go, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return go;
    }

    private static Button MakeButton(GameObject parent, string name, string label,
                                      Vector2 anchorMin, Vector2 anchorMax)
    {
        var go  = GetOrCreateChild(parent, name);
        SetRect(go, anchorMin, anchorMax, new Vector2(4, 4), new Vector2(-4, -4));
        go.GetOrAdd<Image>().color = ButtonBg;
        var btn = go.GetOrAdd<Button>();
        var lbl = GetOrCreateChild(go, "Label");
        SetRect(lbl, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        var tmp = lbl.GetOrAdd<TextMeshProUGUI>();
        tmp.text      = label;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize  = 13;
        return btn;
    }

    private static void SetRect(GameObject go, Vector2 anchorMin, Vector2 anchorMax,
                                  Vector2 offsetMin, Vector2 offsetMax)
    {
        var r   = go.GetOrAdd<RectTransform>();
        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
        r.offsetMin = offsetMin;
        r.offsetMax = offsetMax;
    }

    private static GameObject GetOrCreateChild(GameObject parent, string name)
    {
        var tf = parent.transform.Find(name);
        if (tf != null) return tf.gameObject;
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    private static void EnsureManager<T>(string goName) where T : MonoBehaviour
    {
        if (Object.FindFirstObjectByType<T>() != null) return;
        var go = new GameObject(goName);
        go.AddComponent<T>();
    }

    private static void SetPrivateSlotArray(MonoBehaviour target, string fieldName,
                                             InventorySlotUI[] slots)
    {
        var so   = new SerializedObject(target);
        var prop = so.FindProperty(fieldName);
        prop.arraySize = slots.Length;
        for (int i = 0; i < slots.Length; i++)
            prop.GetArrayElementAtIndex(i).objectReferenceValue = slots[i];
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetSlotArray(SerializedObject so, string fieldName,
                                      InventorySlotUI[] slots)
    {
        var prop = so.FindProperty(fieldName);
        prop.arraySize = slots.Length;
        for (int i = 0; i < slots.Length; i++)
            prop.GetArrayElementAtIndex(i).objectReferenceValue = slots[i];
    }
}

// Extension helpers so we don't repeat GetComponent + AddComponent everywhere
internal static class GameObjectExtensions
{
    // ObjectFactory.AddComponent is the editor-aware variant that fully initializes
    // native component data (e.g. Canvas RenderMode) before returning — plain
    // AddComponent() leaves components partially uninitialized in Unity 6.
    public static T GetOrAdd<T>(this GameObject go) where T : Component
    {
        var c = go.GetComponent<T>();
        return c != null ? c : ObjectFactory.AddComponent<T>(go);
    }
}
