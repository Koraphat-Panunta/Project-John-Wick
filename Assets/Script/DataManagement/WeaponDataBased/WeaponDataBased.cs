using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponDataBased 
{
    public WeaponDataBased()
    {
        AddWeapon(PrimaryWeaponType.AR15, new AR15_AttachmentData());
    }

    public Dictionary<PrimaryWeaponType, bool> primaryWeaponUnlockable = new Dictionary<PrimaryWeaponType, bool>();
    public Dictionary<SecondaryWeaponType, bool> secondaryWeaponUnlockable = new Dictionary<SecondaryWeaponType, bool>();

    public Dictionary<PrimaryWeaponType, IWeaponAttachmentData> primaryWeaponAttachmentData = new Dictionary<PrimaryWeaponType, IWeaponAttachmentData>();
    public Dictionary<SecondaryWeaponType, IWeaponAttachmentData> secondaryWeaponAttachmentData = new Dictionary<SecondaryWeaponType, IWeaponAttachmentData>();

    public void AddWeapon(PrimaryWeaponType primaryWeapon ,IWeaponAttachmentData weaponAttachmentData)
    {
        primaryWeaponUnlockable.Add(primaryWeapon, false);

        primaryWeaponAttachmentData.Add(primaryWeapon, weaponAttachmentData);
    }
    public void AddWeapon(SecondaryWeaponType secondaryWeapon, IWeaponAttachmentData weaponAttachmentData)
    {
        secondaryWeaponUnlockable.Add(secondaryWeapon, false);

        secondaryWeaponAttachmentData.Add(secondaryWeapon, weaponAttachmentData);
    }
}
