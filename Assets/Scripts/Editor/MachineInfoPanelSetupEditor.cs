using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class MachineInfoPanelSetupEditor
{
    [MenuItem("HF/Setup Machine Info Panel")]
    public static void Setup()
    {
        var canvas = GameObject.Find("GameCanvas");
        if (canvas == null) { Debug.LogError("[HF] GameCanvas not found. Run HF > Setup Inventory UI first."); return; }

        // Root panel
        var panel = GetOrCreate(canvas.transform, "MachineInfoPanel");
        var panelRect = Ensure<RectTransform>(panel);
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0);
        panelRect.pivot     = new Vector2(0.5f, 0);
        panelRect.anchoredPosition = new Vector2(0, 116); // above hotbar
        panelRect.sizeDelta = new Vector2(0, 200);
        var bg = Ensure<Image>(panel);
        bg.color = new Color(0.1f, 0.1f, 0.12f, 0.93f);

        // Name label
        var nameLbl = GetOrCreate(panel.transform, "NameLabel");
        SetRect(nameLbl, new Vector2(8, 168), new Vector2(-8, -8));
        var nameTMP = Ensure<TextMeshProUGUI>(nameLbl);
        nameTMP.text = "Machine Name"; nameTMP.fontSize = 16; nameTMP.fontStyle = FontStyles.Bold;

        // State label
        var stateLbl = GetOrCreate(panel.transform, "StateLabel");
        SetRect(stateLbl, new Vector2(8, 144), new Vector2(-8, -24));
        var stateTMP = Ensure<TextMeshProUGUI>(stateLbl);
        stateTMP.text = "Idle"; stateTMP.fontSize = 12; stateTMP.color = new Color(0.7f, 0.9f, 0.7f);

        // Progress bar
        var progGO = GetOrCreate(panel.transform, "ProgressBar");
        SetRect(progGO, new Vector2(8, 120), new Vector2(-8, -30));
        var slider = Ensure<Slider>(progGO);
        slider.minValue = 0; slider.maxValue = 1; slider.value = 0;

        // Recipe container
        var recipeContainer = GetOrCreate(panel.transform, "RecipeContainer");
        SetRect(recipeContainer, new Vector2(8, 60), new Vector2(-8, -56));
        var hlg = Ensure<HorizontalLayoutGroup>(recipeContainer);
        hlg.spacing = 4; hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = true;

        // Recipe button prefab (create if missing)
        const string prefabPath = "Assets/Prefabs/UI/RecipeButton.prefab";
        GameObject recipeButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (recipeButtonPrefab == null)
        {
            var rbGO = new GameObject("RecipeButton");
            Ensure<Image>(rbGO).color = new Color(0.25f, 0.25f, 0.3f);
            var rbBtn = Ensure<Button>(rbGO);
            var rbRect = Ensure<RectTransform>(rbGO);
            rbRect.sizeDelta = new Vector2(120, 40);
            var rbLabel = new GameObject("Label");
            rbLabel.transform.SetParent(rbGO.transform, false);
            var rbTMP = Ensure<TextMeshProUGUI>(rbLabel);
            rbTMP.text = "Recipe"; rbTMP.fontSize = 11; rbTMP.alignment = TextAlignmentOptions.Center;
            SetRect(rbLabel, Vector2.zero, Vector2.zero, true);
            recipeButtonPrefab = PrefabUtility.SaveAsPrefabAsset(rbGO, prefabPath);
            Object.DestroyImmediate(rbGO);
        }

        // Remove button
        var removeGO = GetOrCreate(panel.transform, "RemoveButton");
        SetRect(removeGO, new Vector2(8, 8), new Vector2(-110, -8));
        Ensure<Image>(removeGO).color = new Color(0.7f, 0.2f, 0.2f);
        Ensure<Button>(removeGO);
        var removeLbl = GetOrCreate(removeGO.transform, "Label");
        SetRect(removeLbl, Vector2.zero, Vector2.zero, true);
        var removeTMP = Ensure<TextMeshProUGUI>(removeLbl);
        removeTMP.text = "REMOVE"; removeTMP.fontSize = 12; removeTMP.alignment = TextAlignmentOptions.Center;

        // Close button
        var closeGO = GetOrCreate(panel.transform, "CloseButton");
        SetRect(closeGO, new Vector2(-100, 8), new Vector2(-8, -8));
        Ensure<Image>(closeGO).color = new Color(0.3f, 0.3f, 0.35f);
        Ensure<Button>(closeGO);
        var closeLbl = GetOrCreate(closeGO.transform, "Label");
        SetRect(closeLbl, Vector2.zero, Vector2.zero, true);
        var closeTMP = Ensure<TextMeshProUGUI>(closeLbl);
        closeTMP.text = "CLOSE"; closeTMP.fontSize = 12; closeTMP.alignment = TextAlignmentOptions.Center;

        // MachineInfoPanel component (on a scene root GO, not the canvas child)
        GameObject panelHost = GameObject.Find("MachineInfoPanel_Host");
        if (panelHost == null) panelHost = new GameObject("MachineInfoPanel_Host");
        var comp = panelHost.GetComponent<MachineInfoPanel>() ?? panelHost.AddComponent<MachineInfoPanel>();

        var so = new SerializedObject(comp);
        so.FindProperty("panel").objectReferenceValue            = panel;
        so.FindProperty("nameLabel").objectReferenceValue        = nameLbl.GetComponent<TextMeshProUGUI>();
        so.FindProperty("stateLabel").objectReferenceValue       = stateLbl.GetComponent<TextMeshProUGUI>();
        so.FindProperty("progressBar").objectReferenceValue      = progGO.GetComponent<Slider>();
        so.FindProperty("recipeContainer").objectReferenceValue  = recipeContainer.transform;
        so.FindProperty("recipeButtonPrefab").objectReferenceValue = recipeButtonPrefab;
        so.FindProperty("removeButton").objectReferenceValue     = removeGO.GetComponent<Button>();
        so.FindProperty("closeButton").objectReferenceValue      = closeGO.GetComponent<Button>();
        so.ApplyModifiedProperties();

        panel.SetActive(false);
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("[HF] Machine Info Panel setup complete.");
    }

    private static GameObject GetOrCreate(Transform parent, string name)
    {
        var t = parent.Find(name);
        if (t != null)
        {
            if (t.GetComponent<RectTransform>() != null) return t.gameObject;
            Object.DestroyImmediate(t.gameObject); // stale object from a failed run — recreate
        }
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    private static T Ensure<T>(GameObject go) where T : Component
        => go.GetComponent<T>() ?? go.AddComponent<T>();

    private static void SetRect(GameObject go, Vector2 offsetMin, Vector2 offsetMax, bool stretch = false)
    {
        var r = go.GetComponent<RectTransform>();
        if (r == null) r = go.AddComponent<RectTransform>();
        if (stretch)
        {
            r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one;
            r.offsetMin = offsetMin;   r.offsetMax = offsetMax;
        }
        else
        {
            r.anchorMin = new Vector2(0, 1); r.anchorMax = new Vector2(1, 1);
            r.pivot     = new Vector2(0.5f, 1);
            r.offsetMin = new Vector2(offsetMin.x, offsetMax.y);
            r.offsetMax = new Vector2(offsetMax.x, offsetMin.y);
        }
    }
}
