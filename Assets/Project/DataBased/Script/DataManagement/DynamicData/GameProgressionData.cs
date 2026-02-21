using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProgressionData", menuName = "ScriptableObjects/GameProgressionData")]
public class GameProgressionData : DataScriptableObject
{

    public GameProgressionDetail[] gameProgressionDetails;
    public void LoadData(PlayerProfileSaveData.LevelClearProgressionSaveData levelClearProgressionData)
    {

        for (int i = 0; i < levelClearProgressionData.saveLevelClearProgression.Length; i++) 
        {
            for (int j = 0; j < this.gameProgressionDetails.Length; j++)
            {

                if (this.gameProgressionDetails[j].levelDataScriptableObject
                    == StaticDataBased.Instance.levelDataBased.GetObjectDataFormID(levelClearProgressionData.saveLevelClearProgression[i].levelID))
                {
                    this.gameProgressionDetails[j].isClear = levelClearProgressionData.saveLevelClearProgression[i].isClear;
                }
            }
        }

    }

    public void LoadData(GameProgressionData gameProgressionData)
    {
        this.gameProgressionDetails = gameProgressionData.gameProgressionDetails;
    }

    public void SetLevelIsClear(LevelDataScriptableObject levelClearProgressionData, bool isClear)
    {
        for (int i = 0; i <= this.gameProgressionDetails.Length; i++)
        {
            if (this.gameProgressionDetails[i].levelDataScriptableObject == levelClearProgressionData)
            {
                this.gameProgressionDetails[i].isClear = isClear;
            }
        }
    }

  

    [Serializable]
    public struct GameProgressionDetail
    {
        public LevelDataScriptableObject levelDataScriptableObject;
        public bool isClear;
    }



}
