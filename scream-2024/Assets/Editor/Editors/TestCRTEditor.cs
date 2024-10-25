using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TestCRT), editorForChildClasses: true)]
public class TestCRTEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var test = (TestCRT)target;

        if (GUILayout.Button("Test"))
        {
            test.Test();
        }
    }

    private IEnumerator DebugRoutine(NVLComponent nvl)
    {
        yield return nvl.ShowRoutine();
    }
}
