using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerProfileSaveData 
{
    public LoadoutSaveData loadoutData;
    public LevelClearProgressionSaveData levelClearProgressionData;
    public ContinueLevelSaveData continueLevelSaveData;
    public SettingSaveData settingSaveData;
    public PlayerProfileSaveData(
        LoadoutData loadoutData
        ,GameProgressionData gameProgressionData
        ,ContinueData continueData
        ,SettingDataScriptableObject settingDataScriptableObject
        )
    {

        this.loadoutData = new LoadoutSaveData(loadoutData);
        this.levelClearProgressionData = new LevelClearProgressionSaveData(gameProgressionData);
        this.continueLevelSaveData = new ContinueLevelSaveData(continueData);
        this.settingSaveData = new SettingSaveData(settingDataScriptableObject);
    }

    [Serializable]
    public class LoadoutSaveData
    {
        public LoadoutSaveData(LoadoutData loadoutData)
        {
            if (loadoutData.primaryWeaponDataPackage.weaponDataScriptableObject != null)
            {
                this.primaryWeaponDataSaveID = loadoutData.primaryWeaponDataPackage.weaponDataScriptableObject.ObjectID;

                if (loadoutData.primaryWeaponDataPackage.weaponAttachmentData != null
                    && loadoutData.primaryWeaponDataPackage.weaponAttachmentData.Length > 0)
                {
                    this.primaryWeaponAttachmentDataID = new string[loadoutData.primaryWeaponDataPackage.weaponAttachmentData.Length];

                    for (int i = 0; i < loadoutData.primaryWeaponDataPackage.weaponAttachmentData.Length; i++)
                    {
                        this.primaryWeaponAttachmentDataID[i] = loadoutData.primaryWeaponDataPackage.weaponAttachmentData[i].ObjectID;
                    }
                }
            }

            if(loadoutData.secondaryWeaponDataPackage.weaponDataScriptableObject != null)
            {
                this.secondaryWeaponDataSaveID = loadoutData.secondaryWeaponDataPackage.weaponDataScriptableObject.ObjectID;

                if(loadoutData.secondaryWeaponDataPackage.weaponAttachmentData != null
                    && loadoutData.secondaryWeaponDataPackage.weaponAttachmentData.Length > 0)
                {
                    this.secondaryWeaponAttachmentDataID = new string[loadoutData.secondaryWeaponDataPackage.weaponAttachmentData.Length];

                    for(int i = 0; i < loadoutData.secondaryWeaponDataPackage.weaponAttachmentData.Length; i++)
                    {
                        this.secondaryWeaponAttachmentDataID[i] = loadoutData.secondaryWeaponDataPackage.weaponAttachmentData [i].ObjectID;
                    }
                }
            }
        }

        

        [SerializeField] public string primaryWeaponDataSaveID;
        [SerializeField] public string[] primaryWeaponAttachmentDataID;

        [SerializeField] public string secondaryWeaponDataSaveID;
        [SerializeField] public string[] secondaryWeaponAttachmentDataID;
    }

    [Serializable]
    public class LevelClearProgressionSaveData
    {
        public LevelClearProgressionSaveData(GameProgressionData gameProgressionData) 
        {
            Debug.Log("LevelClearProgressionSaveData Construtor");
            this.saveLevelClearProgression = new GameProgrsstionSaveDetail[gameProgressionData.gameProgressionDetails.Length];

            for (int i = 0; i < gameProgressionData.gameProgressionDetails.Length; i++) 
            {
                this.saveLevelClearProgression[i].levelID = gameProgressionData.gameProgressionDetails[i].levelDataScriptableObject.ObjectID;
                this.saveLevelClearProgression[i].isClear = gameProgressionData.gameProgressionDetails[i].isClear;
            }
        }

        [Serializable]
        public struct GameProgrsstionSaveDetail
        {
            public string levelID;
            public bool isClear;
        }
        [SerializeField] public GameProgrsstionSaveDetail[] saveLevelClearProgression;
    }

    [Serializable]
    public class ContinueLevelSaveData
    {
        public ContinueLevelSaveData(ContinueData continueData)
        {
            this.continueLevelDataSCRP_ID = continueData.continueLevelDataSCRP.ObjectID;
            this.continueAtCheckPoint = continueData.continueAtCheckPoint;

            if(continueData.continueWeaponPackage == null
                || continueData.continueWeaponPackage.Length <= 0)
                return;

            this.SaveWeaponAndAttachment(continueData.continueWeaponPackage);
        }

        private void SaveWeaponAndAttachment(WeaponDataPackage[] continueWeaponPackage)
        {
            this.weaponSaveDataPackages = new WeaponSaveDataPackage[continueWeaponPackage.Length];

            for (int i = 0; i < continueWeaponPackage.Length; i++)
            {
                this.weaponSaveDataPackages[i].weaponID = continueWeaponPackage[i].weaponDataScriptableObject.ObjectID;

                if (continueWeaponPackage[i].weaponAttachmentData == null
                    ||continueWeaponPackage[i].weaponAttachmentData.Length <= 0)
                    continue;

                this.weaponSaveDataPackages[i].weaponAttachmentID = new string[continueWeaponPackage[i].weaponAttachmentData.Length];
           
                for (int j = 0; j < continueWeaponPackage[i].weaponAttachmentData.Length; j++)
                {
                    this.weaponSaveDataPackages[i].weaponAttachmentID[j] = continueWeaponPackage[i].weaponAttachmentData[j].ObjectID;
                }
            }
        }

        [SerializeField] public string continueLevelDataSCRP_ID;
        [SerializeField] public int continueAtCheckPoint;

        [SerializeField] public WeaponSaveDataPackage[] weaponSaveDataPackages;
    }

    [Serializable]
    public class SettingSaveData
    {
        public SettingSaveData(SettingDataScriptableObject settingDataScriptableObject) 
        {
            this.audioSetting = settingDataScriptableObject.audioSetting;
            this.gameSetting = settingDataScriptableObject.gameSetting;
            this.graphicSetting = settingDataScriptableObject.graphicSetting;
        }

        public AudioSetting audioSetting;
        public GraphicSetting graphicSetting;
        public GameSetting gameSetting;
    }
    
}


