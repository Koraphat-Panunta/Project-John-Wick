using UnityEngine;

public class AudioSettingMenuSector : OptionMenuSector
{

    public override OptionUIDisplayer optionUIDisplayer => this.audioSettingOptionDisplay;
    public AudioSettingOptionDisplay audioSettingOptionDisplay { get; protected set; }

    public AudioSettingMenuSector(OptionUICanvas optionUICanvas, AudioSettingOptionDisplay optionUIDisplayer, GameMaster gameMaster) : base(optionUICanvas, gameMaster)
    {
        this.audioSettingOptionDisplay = optionUIDisplayer;

        this.audioSettingOptionDisplay.masterVolume.onValueChanged.AddListener(this.OnMasterVolumeValueChange);
        this.audioSettingOptionDisplay.musicVolume.onValueChanged.AddListener(this.OnMusicVolumeValueChange);
        this.audioSettingOptionDisplay.sfxVolume.onValueChanged.AddListener(this.OnSoundEffectVolumeValueChange);
    }

    public override void ResetToDefault()
    {

    }

    protected void OnMasterVolumeValueChange(float value) 
    {
        SoundManager.Instance.SetMasterVolume(value);
    }
    protected void OnMusicVolumeValueChange(float value) 
    {
        SoundManager.Instance.SetMusicVolume(value);
    }
    protected void OnSoundEffectVolumeValueChange(float value) 
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
