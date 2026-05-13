using UnityEditor;
using UnityEngine;

public static class DebugInventorySetupEditor
{
    [MenuItem("HF/Debug: Add Pipe Items to Scene")]
    public static void AddDebugInventory()
    {
        if (Object.FindFirstObjectByType<DebugStartInventory>() != null)
        {
            Debug.Log("[HF] DebugStartInventory already in scene.");
            return;
        }

        var go = new GameObject("DebugStartInventory");
        var comp = go.AddComponent<DebugStartInventory>();

        // Pre-fill with all White pipe ItemData if it exists
        var whitePipe = AssetDatabase.LoadAssetAtPath<ItemData>(
            "Assets/Data/Items/ItemPipe_White_Item.asset");
        if (whitePipe != null)
        {
            comp.startingItems = new DebugStartInventory.DebugItemEntry[]
            {
                new() { item = whitePipe, quantity = 64 }
            };
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("[HF] DebugStartInventory added. Hit Play — White pipes will be in hotbar slot 0.");
    }
}
