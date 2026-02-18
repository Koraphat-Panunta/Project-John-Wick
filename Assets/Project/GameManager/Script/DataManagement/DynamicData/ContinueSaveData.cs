using System;
using UnityEngine;

[Serializable]
public class ContinueSaveData 
{
    public LevelDataScriptableObject continueLevelDataSCRP;
    public int continueAtCheckPoint;

    public WeaponDataPackage[] continueWeaponPackage;
    public ContinueSaveData()
    {

    }

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
        
    }

}
