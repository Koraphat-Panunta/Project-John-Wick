using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadoutData", menuName = "ScriptableObjects/LoadoutData")]
public class LoadoutData : DataScriptableObject
{
   

    public void LoadData(PlayerProfileSaveData.LoadoutSaveData loadoutData)
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

    public void LoadData(LoadoutData loadoutData)
    {
        this.primaryWeaponDataPackage = loadoutData.primaryWeaponDataPackage;
        this.secondaryWeaponDataPackage = loadoutData.secondaryWeaponDataPackage;
    }

    public WeaponDataPackage primaryWeaponDataPackage;
    public WeaponDataPackage secondaryWeaponDataPackage;
}
