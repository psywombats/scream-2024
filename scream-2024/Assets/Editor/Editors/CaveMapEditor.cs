using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CaveMap), editorForChildClasses: true)]
public class CaveMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var cave = (CaveMap)target;

        if (GUILayout.Button("Regenerate"))
        {
            cave.Regenerate(0);
        }
        if (GUILayout.Button("Destroy"))
        {
            cave.DestroyChunks();
        }
    }

    private IEnumerator DebugRoutine(NVLComponent nvl)
    {
        yield return nvl.ShowRoutine();
    }
}
