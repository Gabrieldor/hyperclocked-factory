using UnityEditor;
using UnityEngine;

/// Imports pipes_item.png and pipes_fluid.png, slices into 16×32px sprites,
/// and creates PipeSprites ScriptableObject assets.
public static class PipeSpritesSetupEditor
{
    [MenuItem("HF/Setup Pipe Sprites")]
    public static void SetupPipeSprites()
    {
        ConfigureSheet("Assets/Art/Pipes/pipes_item.png",  "item");
        ConfigureSheet("Assets/Art/Pipes/pipes_fluid.png", "fluid");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssignHotbarIcons();
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("HF", "Pipe sprites imported and icons assigned.", "OK");
    }

    private static void AssignHotbarIcons()
    {
        // Load the E+W horizontal sprite (index 1) from the item sheet — used as hotbar icon
        Sprite icon = null;
        var all = AssetDatabase.LoadAllAssetsAtPath("Assets/Art/Pipes/pipes_item.png");
        foreach (var obj in all)
            if (obj is Sprite sp && sp.name == "pipe_item_01") { icon = sp; break; }

        if (icon == null) { Debug.LogWarning("[HF] pipe_item_01 not found — run Setup Pipe Sprites first."); return; }

        var colors = new[] { "White", "Red", "Green", "Blue", "Yellow" };
        foreach (var c in colors)
        {
            var path = $"Assets/Data/Items/ItemPipe_{c}_Item.asset";
            var id = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (id == null) continue;
            id.icon = icon;
            EditorUtility.SetDirty(id);
        }
        Debug.Log("[HF] Pipe ItemData icons assigned (pipe_item_01 = E+W horizontal).");
    }

    private static void ConfigureSheet(string path, string layer)
    {
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer == null) { Debug.LogError($"[HF] Not found: {path}"); return; }

        importer.textureType        = TextureImporterType.Sprite;
        importer.spriteImportMode   = SpriteImportMode.Multiple;
        importer.filterMode         = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.spritePixelsPerUnit = 32;

        // Slice first 12 columns of 32×32 (indices 0-11 are the valid sprites)
        var metas = new SpriteMetaData[12];
        for (int i = 0; i < 12; i++)
        {
            metas[i] = new SpriteMetaData
            {
                name      = $"pipe_{layer}_{i:D2}",
                rect      = new Rect(i * 32, 0, 32, 32),
                pivot     = new Vector2(0.5f, 0.5f),
                alignment = (int)SpriteAlignment.Center
            };
        }
        importer.spritesheet = metas;

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        Debug.Log($"[HF] Imported {path} → 16 sprites (pipe_{layer}_00..15)");
    }
}
