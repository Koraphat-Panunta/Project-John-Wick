using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ObjectPlacementSelector))]

public class ObjectPlacementSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectPlacementSelector selector = (ObjectPlacementSelector)target;

        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Placement Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("◀ Back"))
        {
            selector.BackObject();
        }
        if (GUILayout.Button("Next ▶"))
        {
            selector.NextObject();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

       

        serializedObject.ApplyModifiedProperties();
    }
}
