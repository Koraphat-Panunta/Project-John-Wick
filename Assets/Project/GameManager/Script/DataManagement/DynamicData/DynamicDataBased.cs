using System.Collections.Generic;
using System;
using UnityEngine;

public class DynamicDataBased : MonoBehaviour
{
    public static DynamicDataBased Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.playerLoadoutData = new LoadoutData();
        this.levelClearProgressionData = new GameProgressionData();
        this.continueLevelSaveData = new ContinueSaveData();

        this.InitializedData();
    }

    public LoadoutData playerLoadoutData;
    public GameProgressionData levelClearProgressionData;
    public ContinueSaveData continueLevelSaveData;
    public SettingDataScriptableObject settingDataScriptableObject;

    protected void InitializedData()
    {

    }
}
