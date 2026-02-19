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

        this.primaryWeaponDataPackage.weaponAttachmentData = new AttachmentDataScriptableObject[loadoutData.primaryWeaponAttachmentDataID.Length];

        for(int i = 0;i < this.primaryWeaponDataPackage.weaponAttachmentData.Length; i++)
        {
            this.primaryWeaponDataPackage.weaponAttachmentData[i] = StaticDataBased.Instance.weaponAttachmentDataBased.GetObjectDataFormID(loadoutData.primaryWeaponAttachmentDataID[i]);
        }

        this.secondaryWeaponDataPackage.weaponDataScriptableObject
            = StaticDataBased.Instance.weaponDataBased.GetObjectDataFormID(loadoutData.secondaryWeaponDataSaveID);

        this.secondaryWeaponDataPackage.weaponAttachmentData = new AttachmentDataScriptableObject[loadoutData.secondaryWeaponAttachmentDataID.Length];

        for (int i = 0; i < this.secondaryWeaponDataPackage.weaponAttachmentData.Length; i++)
        {
            this.secondaryWeaponDataPackage.weaponAttachmentData[i] = StaticDataBased.Instance.weaponAttachmentDataBased.GetObjectDataFormID(loadoutData.secondaryWeaponAttachmentDataID[i]);
        }
    }

    public WeaponDataPackage primaryWeaponDataPackage;
    public WeaponDataPackage secondaryWeaponDataPackage;
}
