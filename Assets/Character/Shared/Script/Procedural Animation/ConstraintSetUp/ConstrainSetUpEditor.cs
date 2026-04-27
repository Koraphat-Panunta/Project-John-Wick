using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConstrainSetUp))]
public class ConstrainSetUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("SetUpConstrainBone"))
        {
            ((ConstrainSetUp)target).SetUpConstrainBone();
        }

        DrawDefaultInspector();
    }
}
