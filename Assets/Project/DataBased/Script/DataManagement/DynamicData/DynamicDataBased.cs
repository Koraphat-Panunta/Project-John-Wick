using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DynamicDataBased : MonoBehaviour,IInitializedAble
{
    public static DynamicDataBased Instance;

    public LoadoutData playerLoadoutData;
    public GameProgressionData gameProgressionData;
    public ContinueData continueData;
    public SettingDataScriptableObject settingDataScriptableObject;

    public string savePath => Application.persistentDataPath + "/PlayerSaveProfileData.ocm";
    protected void InitializedData()
    {
        PlayerProfileSaveData playerProfileSaveData = null;

        if(this.LoadPlayerProfileSaveData(out playerProfileSaveData))
        {
            this.playerLoadoutData.LoadData(playerProfileSaveData.loadoutData);
            this.gameProgressionData.LoadData(playerProfileSaveData.levelClearProgressionData);
            this.continueData.LoadData(playerProfileSaveData.continueLevelSaveData);
            this.settingDataScriptableObject.LoadData(playerProfileSaveData.settingSaveData);
        }
        else
        {
            this.playerLoadoutData.LoadData(StaticDataBased.Instance.defualtLoadOut);
            this.gameProgressionData.LoadData(StaticDataBased.Instance.defaultGameProgressionData);
            this.continueData.LoadData(StaticDataBased.Instance.defualtContinueData);
            this.settingDataScriptableObject.LoadData(StaticDataBased.Instance.defaultSettingData);
        }
    }

    private bool LoadPlayerProfileSaveData(out PlayerProfileSaveData playerProfileSaveData)
    {
        playerProfileSaveData = null;

        if(File.Exists(this.savePath) == false)
            return false;

        string json = File.ReadAllText(savePath);
        playerProfileSaveData = JsonUtility.FromJson<PlayerProfileSaveData>(json);

        return true;
    }
  

    public void SaveAsPlayerProfile()
    {

        PlayerProfileSaveData playerProfileSaveData = new PlayerProfileSaveData(
            this.playerLoadoutData
            ,this.gameProgressionData
            ,this.continueData
            ,this.settingDataScriptableObject
            );

        string json = JsonUtility.ToJson(playerProfileSaveData, true);
        File.WriteAllText(savePath, json);
    }

    public void Initialized()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.playerLoadoutData = new LoadoutData();
        this.gameProgressionData = new GameProgressionData();
        this.continueData = new ContinueData();

        this.InitializedData();

        DontDestroyOnLoad(gameObject);
    }
}
