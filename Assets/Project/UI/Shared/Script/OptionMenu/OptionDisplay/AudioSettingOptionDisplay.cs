using UnityEngine;
using UnityEngine.UI;

public class AudioSettingOptionDisplay : OptionUIDisplayer
{

    private Slider masterVolume;
    private Slider musicVolume;
    private Slider sfxVolume;

    public AudioSettingOptionDisplay(GameObject canvasSector
        ,Slider masterVolume
        ,Slider musicVolume
        ,Slider sfxVolume) : base(canvasSector)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
    }

    protected override void Load(DataBased dataBased)
    {
        this.masterVolume.value = dataBased.settingData.volumeMaster;
        this.musicVolume.value = dataBased.settingData.volumeMusic;
        this.sfxVolume.value = dataBased.settingData.volumeEffect;

    }
}
