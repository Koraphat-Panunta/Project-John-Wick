using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class AR15_AttachmentData : IWeaponAttachmentData
{
    public Dictionary<OpticAttachmentData, bool> opticUnlockAble;
    public Dictionary<MuzzleAttachmentData, bool> muzzleUnlockAble;
    public AR15_AttachmentData(/*List<WeaponAttachment> sights,List<WeaponAttachment> muzzles*/)
    {
        //if (sights != null)
        //    foreach (WeaponAttachment sight in sights)
        //        sightUnlockAble.Add(sight, false);

        //if(muzzles != null)
        //    foreach(WeaponAttachment muzzle in muzzles)
        //        muzzleUnlockAble.Add(muzzle, false);
    }
    public void AddNewAttachment(OpticAttachmentData weaponAttachment)
    {
        opticUnlockAble.Add(weaponAttachment, false);
    }
    public void AddNewAttachment(MuzzleAttachmentData weaponAttachment)
    {
        muzzleUnlockAble.Add(weaponAttachment, false);
    }


}
