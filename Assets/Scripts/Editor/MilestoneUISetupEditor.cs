using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/// Menu: HF > Setup Milestone UI
/// Adds MilestoneTrackerPanel under GameCanvas and wires MilestonesBtn on the Toolbar.
/// Safe to re-run — destroys and rebuilds the panel.
public static class MilestoneUISetupEditor
{
    private static readonly Color PanelBg    = new Color(0.06f, 0.06f, 0.09f, 0.97f);
    private static readonly Color HeaderBg   = new Color(0.10f, 0.10f, 0.13f, 1f);
    private static readonly Color ButtonBg   = new Color(0.22f, 0.22f, 0.28f, 1f);
    private static readonly Color ScrollBg   = new Color(0.08f, 0.08f, 0.10f, 1f);

    [MenuItem("HF/Setup Milestone UI")]
    public static void Setup()
    {
        var canvasGo = GameObject.Find("GameCanvas");
        if (canvasGo == null)
        {
            EditorUtility.DisplayDialog("HF Setup",
                "GameCanvas not found. Run 'HF > Setup Inventory UI' first.", "OK");
            return;
        }

        // Remove stale panel
        var stale = canvasGo.transform.Find("MilestoneTrackerPanel");
        if (stale != null) Object.DestroyImmediate(stale.gameObject);

        // ── Panel root (full-screen, hidden) ──────────────────────────────────
        var panel = CreateChild(canvasGo, "MilestoneTrackerPanel");
        SetStretch(panel);
        panel.GetOrAdd<Image>().color = PanelBg;
        panel.SetActive(false);

        // ── Header ────────────────────────────────────────────────────────────
        var header = CreateChild(panel, "Header");
        var headerRect = header.GetOrAdd<RectTransform>();
        headerRect.anchorMin = new Vector2(0, 1);
        headerRect.anchorMax = new Vector2(1, 1);
        headerRect.offsetMin = new Vector2(0, -56);
        headerRect.offsetMax = Vector2.zero;
        header.GetOrAdd<Image>().color = HeaderBg;

        var titleGo = CreateChild(header, "Title");
        var titleRect = titleGo.GetOrAdd<RectTransform>();
        titleRect.anchorMin = Vector2.zero;
        titleRect.anchorMax = Vector2.one;
        titleRect.offsetMin = new Vector2(12, 0);
        titleRect.offsetMax = new Vector2(-60, 0);
        var titleTmp = titleGo.GetOrAdd<TextMeshProUGUI>();
        titleTmp.text      = "MILESTONES";
        titleTmp.alignment = TextAlignmentOptions.MidlineLeft;
        titleTmp.fontSize  = 18;
        titleTmp.color     = Color.white;

        var closeBtnGo = CreateChild(header, "CloseBtn");
        var closeBtnRect = closeBtnGo.GetOrAdd<RectTransform>();
        closeBtnRect.anchorMin = new Vector2(1, 0);
        closeBtnRect.anchorMax = new Vector2(1, 1);
        closeBtnRect.offsetMin = new Vector2(-56, 4);
        closeBtnRect.offsetMax = new Vector2(-4, -4);
        closeBtnGo.GetOrAdd<Image>().color = ButtonBg;
        var closeBtn = closeBtnGo.GetOrAdd<Button>();
        closeBtn.transition = Selectable.Transition.None;

        var closeLabelGo = CreateChild(closeBtnGo, "Label");
        SetStretch(closeLabelGo);
        var closeTmp = closeLabelGo.GetOrAdd<TextMeshProUGUI>();
        closeTmp.text      = "✕";
        closeTmp.alignment = TextAlignmentOptions.Center;
        closeTmp.fontSize  = 18;
        closeTmp.color     = Color.white;

        // ── Card container (simple VLG, no scroll/mask for Phase 1) ─────────────
        var contentGo = CreateChild(panel, "CardContainer");
        var contentRect = contentGo.GetOrAdd<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.offsetMin = new Vector2(0, 0);
        contentRect.offsetMax = new Vector2(0, -56);

        var vlg = contentGo.GetOrAdd<VerticalLayoutGroup>();
        vlg.childControlWidth      = true;
        vlg.childControlHeight     = true;
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing                = 6f;
        vlg.padding                = new RectOffset(8, 8, 8, 8);

        // ── MilestoneTrackerUI ────────────────────────────────────────────────
        var trackerUI = canvasGo.GetOrAdd<MilestoneTrackerUI>();
        var so = new SerializedObject(trackerUI);
        so.FindProperty("panel").objectReferenceValue       = panel;
        so.FindProperty("contentRoot").objectReferenceValue = contentGo.transform;
        so.ApplyModifiedPropertiesWithoutUndo();

        // Wire CloseBtn
        UnityEditor.Events.UnityEventTools.AddPersistentListener(
            closeBtn.onClick, trackerUI.Close);

        // Wire MilestonesBtn in Toolbar
        var toolbar        = canvasGo.transform.Find("Toolbar");
        var milestonesBtnT = toolbar?.Find("MilestonesBtn");
        var milestonesBtn  = milestonesBtnT?.GetComponent<Button>();
        if (milestonesBtn != null)
        {
            // Clear ALL persistent listeners (RemoveAllListeners only clears runtime ones)
            var btnSO = new SerializedObject(milestonesBtn);
            btnSO.FindProperty("m_OnClick.m_PersistentCalls.m_Calls").ClearArray();
            btnSO.ApplyModifiedPropertiesWithoutUndo();

            UnityEditor.Events.UnityEventTools.AddPersistentListener(
                milestonesBtn.onClick, trackerUI.Toggle);
        }
        else
        {
            Debug.LogWarning("[HF] MilestonesBtn not found in Toolbar — wire manually.");
        }

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[HF] Milestone UI setup complete.");
        EditorUtility.DisplayDialog("HF Setup",
            "MilestoneTrackerPanel created under GameCanvas.\nSave the scene (Ctrl+S) then hit Play.", "OK");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static GameObject CreateChild(GameObject parent, string name)
    {
        var go   = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    private static void SetStretch(GameObject go)
    {
        var rt = go.GetOrAdd<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
