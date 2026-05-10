#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BodyRotationConstrainScriptableObject))]
public class BodyRotationConstrainScriptableObjectEditor : Editor
{
    private bool showSpine0 = true;
    private bool showSpine1 = true;
    private bool showSpine2 = true;
    private bool showRotationSettings = true;

    public override void OnInspectorGUI()
    {
        var data = (BodyRotationConstrainScriptableObject)target;
        SaveEditorChanged.SaveEditorChangedObject(data);

        showSpine0 = EditorGUILayout.Foldout(showSpine0, "Spine 0", true);
        if (showSpine0)
        {
            EditorGUI.indentLevel++;
            data.weightConstraint = EditorGUILayout.Slider("Weight", data.weightConstraint, 0f, 1f);
            data.offsetConstraint = EditorGUILayout.Vector3Field("Offset", data.offsetConstraint);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(4);

        showSpine1 = EditorGUILayout.Foldout(showSpine1, "Spine 1", true);
        if (showSpine1)
        {
            EditorGUI.indentLevel++;
            data.weightConstraint1 = EditorGUILayout.Slider("Weight", data.weightConstraint1, 0f, 1f);
            data.offsetConstraint1 = EditorGUILayout.Vector3Field("Offset", data.offsetConstraint1);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(4);

        showSpine2 = EditorGUILayout.Foldout(showSpine2, "Spine 2", true);
        if (showSpine2)
        {
            EditorGUI.indentLevel++;
            data.weightConstraint2 = EditorGUILayout.Slider("Weight", data.weightConstraint2, 0f, 1f);
            data.offsetConstraint2 = EditorGUILayout.Vector3Field("Offset", data.offsetConstraint2);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(4);

        showRotationSettings = EditorGUILayout.Foldout(showRotationSettings, "Rotation Settings", true);
        if (showRotationSettings)
        {
            EditorGUI.indentLevel++;
            data.rotateRefDirOffset = EditorGUILayout.Vector3Field("Rotate Ref Dir Offset", data.rotateRefDirOffset);
            data.maxHorizontalDeg = EditorGUILayout.Slider("Max Horizontal Deg", data.maxHorizontalDeg, 0f, 180f);
            data.maxVerticalDeg = EditorGUILayout.Slider("Max Vertical Deg", data.maxVerticalDeg, 0f, 180f);
            data.offsetChangedRate = EditorGUILayout.Slider("Offset Changed Rate", data.offsetChangedRate, 0f, 500f);
            EditorGUI.indentLevel--;
        }
    }
}
#endif
