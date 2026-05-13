using UnityEditor;
using UnityEngine;

public static class PipeSetupEditor
{
    [MenuItem("HF/Setup Pipe System")]
    public static void SetupPipeSystem()
    {
        CreatePipeSOs();
        AddSceneComponents();
        EditorUtility.DisplayDialog("HF", "Pipe system setup complete.", "OK");
    }

    private static void CreatePipeSOs()
    {
        const string dir = "Assets/Data/Pipes";
        if (!System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);

        // Create one PipeData SO per color (Item layer)
        var colors = new[] { PipeColor.White, PipeColor.Red, PipeColor.Green, PipeColor.Blue, PipeColor.Yellow };
        foreach (var color in colors)
        {
            string path = $"{dir}/ItemPipe_{color}.asset";
            if (AssetDatabase.LoadAssetAtPath<PipeData>(path) != null) continue;

            var pd = ScriptableObject.CreateInstance<PipeData>();
            pd.pipeName = $"{color} Item Pipe";
            pd.color = color;
            pd.layer = PipeLayer.Item;
            AssetDatabase.CreateAsset(pd, path);
        }

        // Create matching ItemData SOs so pipes can live in the hotbar
        const string itemDir = "Assets/Data/Items";
        foreach (var color in colors)
        {
            string path = $"{itemDir}/ItemPipe_{color}_Item.asset";
            if (AssetDatabase.LoadAssetAtPath<ItemData>(path) != null) continue;

            var pipeData = AssetDatabase.LoadAssetAtPath<PipeData>($"{dir}/ItemPipe_{color}.asset");

            var id = ScriptableObject.CreateInstance<ItemData>();
            id.itemName = $"{color} Pipe";
            id.stackSize = 64;
            id.placeablePipe = pipeData;
            AssetDatabase.CreateAsset(id, path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[HF] Pipe SO assets created.");
    }

    private static void AddSceneComponents()
    {
        // TickManager
        if (Object.FindFirstObjectByType<TickManager>() == null)
        {
            var go = new GameObject("TickManager");
            go.AddComponent<TickManager>();
            Debug.Log("[HF] TickManager added to scene.");
        }

        // PipeNetwork
        if (Object.FindFirstObjectByType<PipeNetwork>() == null)
        {
            var go = new GameObject("PipeNetwork");
            go.AddComponent<PipeNetwork>();
            Debug.Log("[HF] PipeNetwork added to scene.");
        }

        // PipePortPanel — minimal stub (no visual prefab yet; panel field left null)
        if (Object.FindFirstObjectByType<PipePortPanel>() == null)
        {
            var go = new GameObject("PipePortPanel");
            go.AddComponent<PipePortPanel>();
            Debug.Log("[HF] PipePortPanel added to scene (wire up panel/prefab fields manually).");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }
}
