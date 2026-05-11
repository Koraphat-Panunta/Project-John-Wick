using System;
using UnityEngine;

[Serializable]
public class AttachmentEffect : IPickupEffect
{
    [Tooltip("ScriptableObject that defines the attachment prefab, type, and socket offsets.")]
    public AttachmentDataScriptableObject attachmentData;

    public bool CanBeReceivedBy(IItemReceiver receiver)
    {
        if (attachmentData == null || attachmentData.weaponAttachmentPrefab == null) return false;
        if (receiver is IAttachmentReceiver ar)
            return ar.HasAttachmentSocket(attachmentData.attachmentType);
        return false;
    }

    public void Apply(IItemReceiver receiver, object source)
    {
        if (receiver is IAttachmentReceiver ar)
            ar.ReceiveAttachment(this);
    }
}
