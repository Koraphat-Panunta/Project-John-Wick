using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class AR15_AttachmentData : IWeaponAttachmentData
{
    public Dictionary<WeaponAttachment, bool> sightUnlockAble;
    public Dictionary<WeaponAttachment, bool> muzzleUnlockAble;
    public AR15_AttachmentData(List<WeaponAttachment> sights,List<WeaponAttachment> muzzles)
    {
        if (sights != null)
            foreach (WeaponAttachment sight in sights)
                sightUnlockAble.Add(sight, false);

        if(muzzles != null)
            foreach(WeaponAttachment muzzle in muzzles)
                muzzleUnlockAble.Add(muzzle, false);
    }
    public void AddNewAttachment(WeaponAttachment weaponAttachment)
    {
        AttachmentSlot attachmentSlot = weaponAttachment.myAttachmentSlot;

        switch (attachmentSlot)
        {
            case AttachmentSlot.SCOPE 
                : sightUnlockAble.Add(weaponAttachment, false);
                break;

            case AttachmentSlot.MUZZLE
                : muzzleUnlockAble.Add(weaponAttachment, false);
                break;

        }
    }

  
}
