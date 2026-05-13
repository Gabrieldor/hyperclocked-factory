using UnityEngine;
using UnityEditor;

/// Menu: HF > Create Phase 1 SO Assets
/// Creates ItemData, MachineData, and RecipeData assets for the Phase 1 prototype.
/// Safe to re-run — skips assets that already exist.
public static class SOSetupEditor
{
    // ── Paths ────────────────────────────────────────────────────────────────
    private const string ItemDir       = "Assets/Data/Items";
    private const string RecipeDir     = "Assets/Data/Recipes";
    private const string MachineDir    = "Assets/Data/Machines";
    private const string MilestoneDir  = "Assets/Data/Milestones";

    private const string ExtractorSheet = "Assets/Art/Machines/SteamExtractorSpriteSheet.png";
    private const string SmelterSheet   = "Assets/Art/Machines/SteamAlloySmelterSpriteSheet.png";

    private const int TileSize  = 32;
    private const int FrameCount = 9;   // both sheets: 9 × 32px frames

    // ── Entry point ──────────────────────────────────────────────────────────
    [MenuItem("HF/Create Phase 1 SO Assets")]
    public static void CreateAll()
    {
        ConfigureSheet(ExtractorSheet, "extractor");
        ConfigureSheet(SmelterSheet,   "smelter");
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

        // Items
        var coalOre     = GetOrCreate<ItemData>(ItemDir, "Coal",          so => { so.itemName = "Coal";          so.stackSize = 64; });
        var copperOre   = GetOrCreate<ItemData>(ItemDir, "CopperOre",     so => { so.itemName = "Copper Ore";    so.stackSize = 64; });
        var tinOre      = GetOrCreate<ItemData>(ItemDir, "TinOre",        so => { so.itemName = "Tin Ore";       so.stackSize = 64; });
        var copperDust  = GetOrCreate<ItemData>(ItemDir, "CopperDust",    so => { so.itemName = "Copper Dust";   so.stackSize = 64; });
        var tinDust     = GetOrCreate<ItemData>(ItemDir, "TinDust",       so => { so.itemName = "Tin Dust";      so.stackSize = 64; });
        var bronzeIngot = GetOrCreate<ItemData>(ItemDir, "BronzeIngot",   so => { so.itemName = "Bronze Ingot";  so.stackSize = 64; });
        var copperIngot = GetOrCreate<ItemData>(ItemDir, "CopperIngot",   so => { so.itemName = "Copper Ingot";  so.stackSize = 64; });

        // Recipes
        // Copper Dust → Copper Ingot (Primitive Furnace, 4 s)
        GetOrCreate<RecipeData>(RecipeDir, "Recipe_SmeltCopper", so =>
        {
            so.inputs  = new[] { new ItemStack { item = copperDust,  quantity = 1 } };
            so.outputs = new[] { new ItemStack { item = copperIngot, quantity = 1 } };
            so.processingTime = 4f;
        });

        // Copper Dust + Tin Dust → Bronze Ingot (Primitive Furnace, 4 s)
        GetOrCreate<RecipeData>(RecipeDir, "Recipe_SmeltBronze", so =>
        {
            so.inputs  = new[]
            {
                new ItemStack { item = copperDust, quantity = 3 },
                new ItemStack { item = tinDust,    quantity = 1 },
            };
            so.outputs = new[] { new ItemStack { item = bronzeIngot, quantity = 1 } };
            so.processingTime = 4f;
        });

        var smeltCopper = AssetDatabase.LoadAssetAtPath<RecipeData>($"{RecipeDir}/Recipe_SmeltCopper.asset");
        var smeltBronze = AssetDatabase.LoadAssetAtPath<RecipeData>($"{RecipeDir}/Recipe_SmeltBronze.asset");

        // Machines
        var extractorSprite = LoadSprite(ExtractorSheet, "extractor_0");   // frame 0 = idle
        var furnaceSprite   = LoadSprite(SmelterSheet,   "smelter_0");     // frame 0 = idle

        GetOrCreate<MachineData>(MachineDir, "Steam_Extractor", so =>
        {
            so.machineName      = "Steam Extractor";
            so.tier             = Tier.Steam;
            so.tileSizeX        = 1;
            so.tileSizeY        = 1;
            so.availableRecipes = System.Array.Empty<RecipeData>();
            so.sprite           = extractorSprite;
            so.isExtractor      = true;
        });

        GetOrCreate<MachineData>(MachineDir, "Steam_PrimitiveFurnace", so =>
        {
            so.machineName      = "Primitive Furnace";
            so.tier             = Tier.Steam;
            so.tileSizeX        = 1;
            so.tileSizeY        = 1;
            so.availableRecipes = new[] { smeltCopper, smeltBronze };
            so.sprite           = furnaceSprite;
        });

        // Machine items (for hotbar placement)
        var extractorMachineData = AssetDatabase.LoadAssetAtPath<MachineData>($"{MachineDir}/Steam_Extractor.asset");
        var furnaceMachineData   = AssetDatabase.LoadAssetAtPath<MachineData>($"{MachineDir}/Steam_PrimitiveFurnace.asset");

        GetOrCreate<ItemData>(ItemDir, "Item_SteamExtractor", so =>
        {
            so.itemName         = "Steam Extractor";
            so.stackSize        = 64;
            so.placeableMachine = extractorMachineData;
            so.icon             = extractorSprite;
        });

        GetOrCreate<ItemData>(ItemDir, "Item_PrimitiveFurnace", so =>
        {
            so.itemName         = "Primitive Furnace";
            so.stackSize        = 64;
            so.placeableMachine = furnaceMachineData;
            so.icon             = furnaceSprite;
        });

        // Milestones
        if (!System.IO.Directory.Exists(MilestoneDir))
            System.IO.Directory.CreateDirectory(MilestoneDir);

        // S0 — auto-fires on scene load (no trigger); unlocks starter machines
        var s0 = GetOrCreate<MilestoneData>(MilestoneDir, "Milestone_S0", so =>
        {
            so.milestoneName   = "S0 — Steam Age Begins";
            so.triggerItem     = null;
            so.triggerMachine  = null;
            so.prerequisites   = System.Array.Empty<MilestoneData>();
        });

        // S1 — fires on first Bronze Ingot produced
        GetOrCreate<MilestoneData>(MilestoneDir, "Milestone_S1", so =>
        {
            so.milestoneName   = "S1 — First Alloy";
            so.triggerItem     = bronzeIngot;
            so.triggerMachine  = null;
            so.prerequisites   = new[] { s0 };
        });

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[HF] Phase 1 SO assets created.");
        EditorUtility.DisplayDialog("HF Setup", "Phase 1 SO assets created.\nCheck Assets/Data/ for all items, recipes, machines, and milestones.", "OK");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static void ConfigureSheet(string path, string prefix)
    {
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) return;

        importer.textureType        = TextureImporterType.Sprite;
        importer.spriteImportMode   = SpriteImportMode.Multiple;
        importer.filterMode         = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType      = SpriteMeshType.FullRect;
        settings.spritePixelsPerUnit = TileSize;
        importer.SetTextureSettings(settings);

        var metas = new SpriteMetaData[FrameCount];
        for (int i = 0; i < FrameCount; i++)
        {
            metas[i] = new SpriteMetaData
            {
                name      = $"{prefix}_{i}",
                rect      = new Rect(i * TileSize, 0, TileSize, TileSize),
                pivot     = new Vector2(0.5f, 0.5f),
                alignment = (int)SpriteAlignment.Center,
            };
        }
        importer.spritesheet = metas;
        importer.SaveAndReimport();
    }

    private static Sprite LoadSprite(string sheetPath, string spriteName)
    {
        var all = AssetDatabase.LoadAllAssetsAtPath(sheetPath);
        foreach (var obj in all)
            if (obj is Sprite sp && sp.name == spriteName)
                return sp;
        return null;
    }

    /// Loads an existing asset or creates a new one, then applies init action.
    private static T GetOrCreate<T>(string dir, string filename, System.Action<T> init)
        where T : ScriptableObject
    {
        string path = $"{dir}/{filename}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<T>(path);
        if (existing != null) { init(existing); EditorUtility.SetDirty(existing); return existing; }

        var so = ScriptableObject.CreateInstance<T>();
        init(so);
        AssetDatabase.CreateAsset(so, path);
        return so;
    }
}
