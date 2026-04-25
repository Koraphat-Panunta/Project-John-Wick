using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BodySetup))]
public class AutoBodyPartSetUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("SetUpBodyPart"))
        {
            ((BodySetup)target).SetupBodyPart();
        }
        if (GUILayout.Button("SetOwnerBodyPart"))
        {
            ((BodySetup)target).SetBodyOwner();
        }

        DrawDefaultInspector();
    }
}
