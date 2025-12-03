#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HurtBox))]
public class HurtBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var saver = (HurtBox)target;
        DrawDefaultInspector();

        EditorGUILayout.Space();

        GUI.color = Color.red;
        if (GUILayout.Button("Enable Attack"))
        {
            saver.EnableAttack();
        }
        GUI.color = Color.white;
        EditorGUILayout.Space();
        if (GUILayout.Button("Disable Attack"))
        {
            saver.DisableAttack();
        }
    }
}

#endif