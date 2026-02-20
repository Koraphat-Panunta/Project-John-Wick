using UnityEngine;
using UnityEngine.UI;

public class AudioSettingOptionDisplay : OptionUIDisplayer
{

    [SerializeField] public Slider masterVolume;
    [SerializeField] public Slider musicVolume;
    [SerializeField] public Slider sfxVolume;


    protected override void Load(SettingDataScriptableObject dataBased)
    {
        this.masterVolume.value = dataBased.audioSetting.MasterVolume;
        this.musicVolume.value = dataBased.audioSetting.MusicVolume;
        this.sfxVolume.value = dataBased.audioSetting.SoundEffectVolume;

    }
}
