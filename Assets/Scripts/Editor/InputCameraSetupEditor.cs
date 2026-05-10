using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// Menu: HF > Setup Input & Camera
/// Adds InputReader and CameraController to the active scene.
/// Safe to re-run — skips components that are already present.
public static class InputCameraSetupEditor
{
    [MenuItem("HF/Setup Input & Camera")]
    public static void Setup()
    {
        // ── 1. InputReader ────────────────────────────────────────────────────
        var inputReaderGo = GameObject.Find("InputReader");
        if (inputReaderGo == null)
        {
            inputReaderGo = new GameObject("InputReader");
            Debug.Log("[HF] Created InputReader GameObject.");
        }
        if (inputReaderGo.GetComponent<InputReader>() == null)
        {
            inputReaderGo.AddComponent<InputReader>();
            Debug.Log("[HF] Added InputReader component.");
        }

        // ── 2. CameraController on Main Camera ────────────────────────────────
        var camGo = GameObject.Find("Main Camera");
        if (camGo == null)
        {
            Debug.LogError("[HF] 'Main Camera' not found in scene. Run HF > Setup Game Scene first.");
            return;
        }
        if (camGo.GetComponent<CameraController>() == null)
        {
            var cc = camGo.AddComponent<CameraController>();

            // Mirror grid size from GridManager if it's in the scene
            var gm = Object.FindFirstObjectByType<GridManager>();
            if (gm != null)
            {
                var so = new SerializedObject(cc);
                so.FindProperty("gridWidth").intValue  = 16;
                so.FindProperty("gridHeight").intValue = 16;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
            Debug.Log("[HF] Added CameraController to Main Camera.");
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[HF] Input & Camera setup complete. Save the scene (Ctrl+S).");
        EditorUtility.DisplayDialog("HF Setup",
            "InputReader and CameraController added.\n\n" +
            "Controls:\n" +
            "  Touch: 1-finger drag = pan, 2-finger pinch = zoom, tap = action\n" +
            "  Mouse: right-click drag = pan, scroll = zoom, left-click = tap",
            "OK");
    }
}
