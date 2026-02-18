using System;
using UnityEngine;

[Serializable]
public class LoadoutData 
{
    public LoadoutData() 
    {
        this.primaryWeaponDataPackage = new WeaponDataPackage();
        this.secondaryWeaponDataPackage = new WeaponDataPackage();

        this.LoadDefaultData();
    }

    private void LoadDefaultData()
    {
        this.secondaryWeaponDataPackage.weaponDataScriptableObject = StaticDataBased.Instance.weaponDataBased.GetObjectDataFormIndex(0);
    }

    public void LoadData(PlayerProfileSaveData.LoadoutData loadoutData)
    {
        this.primaryWeaponDataPackage.weaponDataScriptableObject 
            = StaticDataBased.Instance.weaponDataBased.GetObjectDataFormID(loadoutData.primaryWeaponDataSaveID);

        this.primaryWeaponDataPackage.weaponAttachmentData = new AttachmentDataScriptableObject[loadoutData.primaryWeaponAttachmentData.Length];
    }

    public WeaponDataPackage primaryWeaponDataPackage;
    public WeaponDataPackage secondaryWeaponDataPackage;
}
