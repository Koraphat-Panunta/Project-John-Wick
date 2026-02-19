using UnityEngine;
using UnityEngine.UI;

public class AudioSettingOptionDisplay : OptionUIDisplayer
{

    [SerializeField] public Slider masterVolume;
    [SerializeField] public Slider musicVolume;
    [SerializeField] public Slider sfxVolume;


    protected override void Load(StaticDataBased.SettingData dataBased)
    {
        this.masterVolume.value = dataBased.volumeMaster;
        this.musicVolume.value = dataBased.volumeMusic;
        this.sfxVolume.value = dataBased.volumeEffect;

    }
}
