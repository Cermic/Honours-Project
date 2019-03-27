using UnityEngine;
using UnityEditor;

public class DebugMenu : EditorWindow
{
    Vector3 coords = new Vector3 (0.0f,0.0f,0.0f);
    [MenuItem("Window/Coords Viewer")]
    public static void ShowWindow()
    {
        GetWindow<DebugMenu>("World Space Coordinates Viewer");
    }

    private void OnGUI()
    {
        GUILayout.Label("World Space Cordinates", EditorStyles.boldLabel);
        coords = Selection.activeGameObject.transform.position;
        coords = EditorGUILayout.Vector3Field("Coords", coords);
    }
    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }
}