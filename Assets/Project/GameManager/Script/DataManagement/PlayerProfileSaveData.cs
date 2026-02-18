using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerProfileSaveData 
{
    public LoadoutData loadoutData;
    public LevelClearProgressionData levelClearProgressionData;
    public ContinueLevelSaveData continueLevelSaveData;
    public SettingSaveData settingSaveData;
    public PlayerProfileSaveData()
    {
        this.loadoutData = new LoadoutData();
        this.levelClearProgressionData = new LevelClearProgressionData();
        this.continueLevelSaveData = new ContinueLevelSaveData();
    }

    [Serializable]
    public class LoadoutData
    {
        public string primaryWeaponDataSaveID;
        public string[] primaryWeaponAttachmentData;

        public string secondaryWeaponDataSave;
        public string[] secondaryWeaponAttachmentData;
    }

    [Serializable]
    public class LevelClearProgressionData
    {
        public Dictionary<string, bool> levelIsClear;
    }

    [Serializable]
    public class ContinueLevelSaveData
    {
        public string continueLevelDataSCRP;
        public int continueAtCheckPoint;
    }

    [Serializable]
    public class SettingSaveData
    {
        public AudioSetting audioSetting;
        public GraphicSetting graphicSetting;
        public GameSetting gameSetting;
    }
    
}


