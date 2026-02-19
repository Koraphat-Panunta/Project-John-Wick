using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameProgressionData 
{
    public GameProgressionData()
    {
        this.levelIsClear = new Dictionary<LevelDataScriptableObject, bool>();

        for (int i = 0; i < StaticDataBased.Instance.levelDataBased.dataScrp.Length ; i++)
        {
            this.levelIsClear.Add(StaticDataBased.Instance.levelDataBased.GetObjectDataFormIndex(i), false);
        }
    }

    public void LoadData(PlayerProfileSaveData.LevelClearProgressionSaveData levelClearProgressionData)
    {
        string[] keyLevelID = levelClearProgressionData.levelIsClear.Keys.ToArray();

        for (int i = 0; i < keyLevelID.Length; i++) 
        {
            this.levelIsClear[StaticDataBased.Instance.levelDataBased.GetObjectDataFormID(keyLevelID[i])] = levelClearProgressionData.levelIsClear[keyLevelID[i]];
        }

    }

    public void LoadData(GameProgressionData gameProgressionData)
    {
        this.levelIsClear = gameProgressionData.levelIsClear;
    }

    public void SetLevelIsClear(LevelDataScriptableObject levelClearProgressionData,bool isClear)
    {
        this.levelIsClear[levelClearProgressionData] = isClear;
    }

    public Dictionary<LevelDataScriptableObject, bool> levelIsClear;
}
