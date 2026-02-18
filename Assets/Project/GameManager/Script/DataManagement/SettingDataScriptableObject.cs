using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingDataScriptableObject", menuName = "ScriptableObjects/GameData/SettingDataSCRP")]
public class SettingDataScriptableObject : DataScriptableObject
{
    public AudioSetting audioSetting;
    public GraphicSetting graphicSetting;
    public GameSetting gameSetting;
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
    public enum ScreenMode
    {
        Windown,
        FullScreen
    }

    public enum Quality
    {
        Low,
        Medium,
        High,
        Ultra,
    }

    public ScreenMode screenMode;
    public Quality quality;

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
