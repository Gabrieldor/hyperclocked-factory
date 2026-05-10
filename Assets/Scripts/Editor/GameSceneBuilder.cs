using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// Menu: HF > Setup Game Scene
/// Creates Assets/Scenes/GameScene.unity with camera + GridManager, and the machine prefab.
public static class GameSceneBuilder
{
    private const string ScenePath  = "Assets/Scenes/GameScene.unity";
    private const string PrefabPath = "Assets/Prefabs/Machines/MachinePlaceholder.prefab";

    [MenuItem("HF/Setup Game Scene")]
    public static void SetupGameScene()
    {
        // --- 1. Create / open scene ---
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // --- 2. Camera ---
        var camGo = new GameObject("Main Camera");
        var cam   = camGo.AddComponent<Camera>();
        camGo.tag = "MainCamera";
        cam.orthographic     = true;
        cam.orthographicSize = 8f;
        cam.clearFlags       = CameraClearFlags.SolidColor;
        cam.backgroundColor  = new Color(0.07f, 0.07f, 0.10f);
        cam.transform.position = new Vector3(8f, 8f, -10f); // centred on 16×16 grid

        camGo.AddComponent<AudioListener>();

        // --- 3. Machine prefab ---
        var prefabGo = new GameObject("MachinePlaceholder");
        prefabGo.AddComponent<SpriteRenderer>();
        prefabGo.AddComponent<MachinePlaceholderView>();

        var prefab = PrefabUtility.SaveAsPrefabAsset(prefabGo, PrefabPath);
        Object.DestroyImmediate(prefabGo);

        // --- 4. GridManager ---
        var gmGo = new GameObject("GridManager");
        var gm   = gmGo.AddComponent<GridManager>();

        // Wire the prefab via serialised field
        var so = new SerializedObject(gm);
        so.FindProperty("machinePrefab").objectReferenceValue = prefab;
        so.ApplyModifiedPropertiesWithoutUndo();

        // --- 5. Save scene ---
        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.Refresh();

        Debug.Log("[HF] GameScene created at " + ScenePath);
        EditorUtility.DisplayDialog("HF Setup", "GameScene created.\nOpen it from Assets/Scenes/GameScene.unity.", "OK");
    }
}
