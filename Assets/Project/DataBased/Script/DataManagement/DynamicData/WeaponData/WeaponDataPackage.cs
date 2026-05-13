using System;
using UnityEngine;

[Serializable]
public struct WeaponDataPackage 
{
    public RangeWeaponDataScriptableObject weaponDataScriptableObject;
    public AttachmentDataScriptableObject[] weaponAttachmentData;
}
