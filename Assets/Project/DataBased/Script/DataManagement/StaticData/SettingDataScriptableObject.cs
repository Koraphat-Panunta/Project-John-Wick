using System;
using UnityEngine;
using static ResolutionDisplay;

[CreateAssetMenu(fileName = "SettingDataScriptableObject", menuName = "ScriptableObjects/GameData/SettingDataSCRP")]
public class SettingDataScriptableObject : DataScriptableObject
{
    public AudioSetting audioSetting;
    public GraphicSetting graphicSetting;
    public GameSetting gameSetting;
    public KeyBindingSetting keyBindingSetting;

    public void LoadData(PlayerProfileSaveData.SettingSaveData settingSaveData)
    {
        this.audioSetting = settingSaveData.audioSetting;
        this.graphicSetting = settingSaveData.graphicSetting;
        this.gameSetting = settingSaveData.gameSetting;
        this.keyBindingSetting = settingSaveData.keyBindingSetting;
    }

    public void LoadData(SettingDataScriptableObject settingDataScriptable)
    {
        this.audioSetting = settingDataScriptable.audioSetting;
        this.graphicSetting= settingDataScriptable.graphicSetting;
        this.gameSetting= settingDataScriptable.gameSetting;
        this.keyBindingSetting = settingDataScriptable.keyBindingSetting;
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
    [Range(1, 10)]
    public float lookSensitivity;

    [Range(1, 10)]
    public float aimDownSightSensitivity;

    public bool displayEnemyHP;
    public bool displayInteract;
    public bool displayHitIndicator;
    public bool displayPlayerAmmo;
    public bool displayPlayerHP;
    public bool displayPlayerStamina;
    public bool displayCrosshair;
}

[Serializable]
public struct KeyBindingSetting
{
    // Unity InputActionAsset binding override JSON — populated at runtime when the player remaps keys.
    // Empty string means use the default bindings defined in UserInput.inputactions.
    public string bindingOverridesJson;
}
