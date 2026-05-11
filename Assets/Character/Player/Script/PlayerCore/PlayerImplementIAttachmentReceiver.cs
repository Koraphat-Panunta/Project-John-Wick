using UnityEngine;

public partial class Player : IAttachmentReceiver
{
    public bool HasAttachmentSocket(AttachmentType type)
    {
        var weapon = _currentWeapon;
        if (weapon == null || weapon.weaponAttachmentSocket == null) return false;
        foreach (var socket in weapon.weaponAttachmentSocket)
            if (socket != null && socket.socketAttachmentType == type) return true;
        return false;
    }

    public void ReceiveAttachment(AttachmentEffect effect)
    {
        var weapon = _currentWeapon;
        if (weapon == null || weapon.weaponAttachmentSocket == null) return;

        var data = effect?.attachmentData;
        if (data == null || data.weaponAttachmentPrefab == null) return;

        WeaponAttachmentSocket targetSocket = null;
        foreach (var socket in weapon.weaponAttachmentSocket)
        {
            if (socket != null && socket.socketAttachmentType == data.attachmentType)
            {
                targetSocket = socket;
                break;
            }
        }

        if (targetSocket == null) return;

        if (targetSocket.curWeaponAttach != null)
        {
            var old = targetSocket.curWeaponAttach;
            targetSocket.Detach();
            Object.Destroy(old.gameObject);
        }

        var newAttachment = Object.Instantiate(data.weaponAttachmentPrefab);
        targetSocket.Attach(newAttachment);

        NotifyObserver(this, NotifyEvent.ReceiveItem);
    }
}
