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

    [SerializeField] protected bool isEnableSaveAndLoad;

    protected void InitializedData()
    {
        PlayerProfileSaveData playerProfileSaveData = null;

        if(this.LoadPlayerProfileSaveData(out playerProfileSaveData) && this.isEnableSaveAndLoad)
        {

            Debug.Log("Found Save files = " + playerProfileSaveData);

            this.playerLoadoutData.LoadData(playerProfileSaveData.loadoutData);
            this.gameProgressionData.LoadData(playerProfileSaveData.levelClearProgressionData);
            this.continueData.LoadData(playerProfileSaveData.continueLevelSaveData);
            this.settingDataScriptableObject.LoadData(playerProfileSaveData.settingSaveData);
        }
        else
        {
            Debug.LogWarning("Load Default Data Found Save files ");

            this.playerLoadoutData.LoadData(StaticDataBased.Instance.defaultLoadoutData);
            this.gameProgressionData.LoadData(StaticDataBased.Instance.defaultGameProgressionData);
            this.continueData.LoadData(StaticDataBased.Instance.defaultContinueData);
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
        Instance = this;

        Debug.Log("Initialized" + this);

        this.InitializedData();

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        if(this.isEnableSaveAndLoad)
            SaveAsPlayerProfile();
        Debug.Log("Application quit");
    }
}
