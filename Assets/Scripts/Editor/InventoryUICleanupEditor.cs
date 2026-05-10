using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class InventoryUICleanupEditor
{
    [MenuItem("HF/Cleanup Inventory UI")]
    public static void Cleanup()
    {
        string[] targets = { "GameCanvas", "EventSystem", "PlayerInventory", "PlacementController" };
        int removed = 0;
        foreach (var name in targets)
        {
            var go = GameObject.Find(name);
            if (go != null) { Object.DestroyImmediate(go); removed++; }
        }
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log($"[HF] Cleanup removed {removed} object(s). Save scene (Ctrl+S).");
    }
}
