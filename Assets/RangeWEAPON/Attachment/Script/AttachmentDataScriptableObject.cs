using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class AttachmentDataScriptableObject : DataScriptableObject
{

    [SerializeField] public WeaponAttachment weaponAttachmentPrefab;
    public AttachmentType attachmentType;

    public Vector3 offsetAnchorAttachPos;
    public Vector3 offsetAnchorAttachRot;

}
