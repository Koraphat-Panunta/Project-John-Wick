using System;
using UnityEngine;

[Serializable]
public class ContinueSaveData
{
    public LevelDataScriptableObject continueLevelDataSCRP;
    public int continueAtCheckPoint;

    public WeaponDataPackage[] continueWeaponPackage;
   

    public void SaveContinue(LevelDataScriptableObject levelDataScriptableObject, int atCheckPoint, WeaponDataPackage[] continueWeaponPackage)
    {
        this.continueLevelDataSCRP = levelDataScriptableObject;
        this.continueAtCheckPoint = atCheckPoint;

        this.continueWeaponPackage = continueWeaponPackage;
    }
    public void SaveContinue(LevelDataScriptableObject levelDataScriptableObject, int atCheckPoint)
    {
        this.SaveContinue(levelDataScriptableObject, atCheckPoint, this.continueWeaponPackage);
    }
    public void LoadData(PlayerProfileSaveData.ContinueLevelSaveData continueLevelSaveData)
    {
        this.continueLevelDataSCRP = StaticDataBased.Instance.levelDataBased.GetObjectDataFormID(continueLevelSaveData.continueLevelDataSCRP_ID);
        this.continueAtCheckPoint = continueLevelSaveData.continueAtCheckPoint;

        if(continueLevelSaveData.continueWeaponDataID == null
            || continueLevelSaveData.continueWeaponDataID.Length <= 0)
            return;

        this.continueWeaponPackage = new WeaponDataPackage[continueLevelSaveData.continueWeaponDataID.Length];

        for (int i = 0; i < continueLevelSaveData.continueWeaponDataID.Length; i++)
        {
            this.continueWeaponPackage[i].weaponDataScriptableObject = StaticDataBased.Instance.weaponDataBased.GetObjectDataFormID(continueLevelSaveData.continueWeaponDataID[i]);

            string[] attachmentID = continueLevelSaveData.continueWeaponAttachmentDataID[continueLevelSaveData.continueWeaponDataID[i]];


            if(attachmentID == null
                || attachmentID.Length <= 0)
                continue;

            this.continueWeaponPackage[i].weaponAttachmentData = new AttachmentDataScriptableObject[attachmentID.Length];

            for (int j = 0; j < attachmentID.Length; j++)
            {
                this.continueWeaponPackage[i].weaponAttachmentData[j] = StaticDataBased.Instance.weaponAttachmentDataBased.GetObjectDataFormID(attachmentID[j]);
            }

        }

    }
}
