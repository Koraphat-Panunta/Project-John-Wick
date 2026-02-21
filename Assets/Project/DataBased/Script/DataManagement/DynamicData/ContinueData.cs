using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ContinueData", menuName = "ScriptableObjects/ContinueData")]
public class ContinueData : DataScriptableObject
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

        if(continueLevelSaveData.weaponSaveDataPackages == null
            || continueLevelSaveData.weaponSaveDataPackages.Length <= 0)
            return;

        this.continueWeaponPackage = new WeaponDataPackage[continueLevelSaveData.weaponSaveDataPackages.Length];

        for (int i = 0; i < continueLevelSaveData.weaponSaveDataPackages.Length; i++)
        {
            this.continueWeaponPackage[i].weaponDataScriptableObject = StaticDataBased.Instance.weaponDataBased.GetObjectDataFormID(continueLevelSaveData.weaponSaveDataPackages[i].weaponID);

            string[] attachmentID = continueLevelSaveData.weaponSaveDataPackages[i].weaponAttachmentID;


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

    public void LoadData(ContinueData continueData)
    {
        this.continueLevelDataSCRP = continueData.continueLevelDataSCRP;
        this.continueAtCheckPoint = continueData.continueAtCheckPoint;
        this.continueWeaponPackage = continueData.continueWeaponPackage;

    }
}
