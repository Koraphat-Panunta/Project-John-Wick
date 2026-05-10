#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CustomEditor(typeof(ConstrainSetUp))]
public class ConstrainSetUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("SetUpConstrainBone"))
        {
            var setup = (ConstrainSetUp)target;
            setup.SetUpConstrainBone();

            foreach (var mb in setup.transform.root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (mb is IRigConstraint)
                    EditorUtility.SetDirty(mb);
            }
        }

        DrawDefaultInspector();
    }
}
#endif
