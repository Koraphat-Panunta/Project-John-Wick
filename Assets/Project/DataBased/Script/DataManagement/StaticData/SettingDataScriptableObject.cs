using System;
using UnityEngine;
using static ResolutionDisplay;

[CreateAssetMenu(fileName = "SettingDataScriptableObject", menuName = "ScriptableObjects/GameData/SettingDataSCRP")]
public class SettingDataScriptableObject : DataScriptableObject
{
    public AudioSetting audioSetting;
    public GraphicSetting graphicSetting;
    public GameSetting gameSetting;

    public void LoadData(PlayerProfileSaveData.SettingSaveData settingSaveData)
    {
        this.audioSetting = settingSaveData.audioSetting;
        this.graphicSetting = settingSaveData.graphicSetting;
        this.gameSetting = settingSaveData.gameSetting; 
    }

    public void LoadData(SettingDataScriptableObject settingDataScriptable)
    {
        this.audioSetting = settingDataScriptable.audioSetting;
        this.graphicSetting= settingDataScriptable.graphicSetting;
        this.gameSetting= settingDataScriptable.gameSetting;
    }
    
}

[Serializable]
public struct AudioSetting
{
    [Range(0,10)]
    public float MasterVolume;
    [Range(0, 10)]
    public float SoundEffectVolume;
    [Range(0, 10)]
    public float MusicVolume;
}

[Serializable]
public struct GraphicSetting
{
    public enum Quality
    {
        Low,
        Medium,
        High,
        Ultra,
    }

    

    public FullScreenMode screenMode;
    public Quality quality;
    public ResolutionPreset resolution;
    public TargetFPS targetFPS;


    public ResolutionDisplay resolutionDisplay;

}

[Serializable]
public struct GameSetting
{
    public bool displayEnemyHP;
    public bool displayInteract;
    public bool displayHitIndicator;
    public bool displayPlayerAmmo;
    public bool displayPlayerHP;
    public bool displayPlayerStamina;
    public bool displayCrosshair;
}
