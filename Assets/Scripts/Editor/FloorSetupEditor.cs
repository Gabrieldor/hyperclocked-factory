using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;

/// Menu: HF > Setup Floor
/// Imports floor.png as a sliced sprite sheet, creates the Grid + Tilemap in the
/// active scene, makes a RandomFloorTile asset, and paints the 16×16 grid.
public static class FloorSetupEditor
{
    private const string SpritePath = "Assets/Art/Tiles/floor.png";
    private const string TileAssetPath = "Assets/Data/FloorTile.asset";
    private const int TileSize = 32;
    private const int GridW = 16;
    private const int GridH = 16;

    [MenuItem("HF/Setup Floor")]
    public static void SetupFloor()
    {
        // --- 1. Import sprite sheet ---
        ConfigureSpriteImport();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

        // --- 2. Load sliced sprites ---
        var sprites = AssetDatabase.LoadAllAssetsAtPath(SpritePath);
        var spriteList = new System.Collections.Generic.List<Sprite>();
        foreach (var obj in sprites)
            if (obj is Sprite sp) spriteList.Add(sp);

        if (spriteList.Count == 0)
        {
            Debug.LogError("[HF] No sprites found in " + SpritePath);
            return;
        }

        // --- 3. Create RandomFloorTile asset ---
        var tile = AssetDatabase.LoadAssetAtPath<RandomFloorTile>(TileAssetPath);
        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance<RandomFloorTile>();
            AssetDatabase.CreateAsset(tile, TileAssetPath);
        }
        tile.variants = spriteList.ToArray();
        EditorUtility.SetDirty(tile);
        AssetDatabase.SaveAssets();

        // --- 4. Find or create Grid + Tilemap in the active scene ---
        var grid = EnsureGrid();
        var tilemap = EnsureTilemap(grid);

        // --- 5. Paint the grid ---
        tilemap.ClearAllTiles();
        for (int x = 0; x < GridW; x++)
            for (int y = 0; y < GridH; y++)
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);

        // --- 6. Save ---
        EditorSceneManager.MarkSceneDirty(tilemap.gameObject.scene);
        AssetDatabase.Refresh();
        Debug.Log($"[HF] Floor painted: {GridW}×{GridH} with {spriteList.Count} variant(s).");
    }

    private static void ConfigureSpriteImport()
    {
        var importer = AssetImporter.GetAtPath(SpritePath) as TextureImporter;
        if (importer == null) return;

        importer.textureType       = TextureImporterType.Sprite;
        importer.spriteImportMode  = SpriteImportMode.Multiple;
        importer.filterMode        = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType    = SpriteMeshType.FullRect;
        settings.spritePixelsPerUnit = TileSize;
        importer.SetTextureSettings(settings);

        // Auto-slice into TileSize×TileSize cells
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(SpritePath);
        int cols = tex != null ? Mathf.Max(1, tex.width  / TileSize) : 2;
        int rows = tex != null ? Mathf.Max(1, tex.height / TileSize) : 1;

        var metas = new SpriteMetaData[cols * rows];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int i = r * cols + c;
                metas[i] = new SpriteMetaData
                {
                    name   = $"floor_{i}",
                    rect   = new Rect(c * TileSize, r * TileSize, TileSize, TileSize),
                    pivot  = new Vector2(0.5f, 0.5f),
                    alignment = (int)SpriteAlignment.Center,
                };
            }
        }
        importer.spritesheet = metas;
        importer.SaveAndReimport();
    }

    private static GameObject EnsureGrid()
    {
        var existing = GameObject.Find("Grid");
        if (existing != null) return existing;

        var go = new GameObject("Grid");
        go.AddComponent<Grid>();
        return go;
    }

    private static Tilemap EnsureTilemap(GameObject grid)
    {
        // Look for child named "Floor"
        var floorTf = grid.transform.Find("Floor");
        if (floorTf != null) return floorTf.GetComponent<Tilemap>();

        var child = new GameObject("Floor");
        child.transform.SetParent(grid.transform, false);
        var tm = child.AddComponent<Tilemap>();
        var renderer = child.AddComponent<TilemapRenderer>();
        renderer.sortingOrder = -10;   // behind machines
        return tm;
    }
}
