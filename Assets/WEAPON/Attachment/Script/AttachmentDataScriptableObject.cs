using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class AttachmentDataScriptableObject : ScriptableObject
{

    [SerializeField,ReadOnly]
    private string attatchmentID;
    public string AttatchmentID => this.attatchmentID;

    public Vector3 offsetAnchorAttachPos;
    public Vector3 offsetAnchorAttachRot;

    public WeaponAttachment weaponAttachmentPrefab;

#if UNITY_EDITOR

    private void OnValidate()
{
    if (string.IsNullOrEmpty(attatchmentID))
    {
        string path = AssetDatabase.GetAssetPath(this);
            attatchmentID = AssetDatabase.AssetPathToGUID(path);
        EditorUtility.SetDirty(this);
    }
}
#endif
}
