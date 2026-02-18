using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameProgressionData 
{
    public GameProgressionData()
    {
        this.levelIsClear = new Dictionary<LevelDataScriptableObject, bool>();
    }

    public void SetLevelIsClear(LevelDataScriptableObject levelClearProgressionData,bool isClear)
    {
        this.levelIsClear[levelClearProgressionData] = isClear;
    }

    public Dictionary<LevelDataScriptableObject, bool> levelIsClear;
}
