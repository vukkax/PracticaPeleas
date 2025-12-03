#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitBox))]
public class HitBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var saver = (HitBox)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();

        GUI.color = Color.blue;
        if (GUILayout.Button("Enable Block"))
        {
            saver.EnableBlock();
        }
        GUI.color = Color.white;
        EditorGUILayout.Space();
        if (GUILayout.Button("Disable Block"))
        {
            saver.DisableBlock();
        }
    }
}

#endif
