

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HumanoidBone))]
public class HumanoidBoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HumanoidBone script = (HumanoidBone)target;

        if (GUILayout.Button("Populate Bones (Mixamo)"))
        {
            script.PopulateBone();
            EditorUtility.SetDirty(script);
        }
    }
}
#endif
