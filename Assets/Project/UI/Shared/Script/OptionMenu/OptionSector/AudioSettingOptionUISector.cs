using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingOptionUISector : OptionUISector
{
    private GameManager gameManager;

    private AudioMixer audioMixer;

    private Slider masterVolume;
    private Slider musicVolume;
    private Slider sfxVolume;

    private string masterVolumeText = "Master";
    private string musicVolumeText = "Music";
    private string sfxVolumeText = "SFX";
    public AudioSettingOptionUISector(OptionUICanvas optionUICanvas, Button applyButton, GameObject canvasSector
        , GameManager gameManager
        , Slider masterVolume
        , Slider musicVolume
        , Slider sfxVolume) : base(optionUICanvas, applyButton, canvasSector)
    {
        this.gameManager = gameManager;

        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
    }

    public override void Apply()
    {
        if(gameManager != null)
        {
            gameManager.dataBased.settingData.volumeMaster = this.masterVolume.value;
            gameManager.dataBased.settingData.volumeMusic = this.musicVolume.value;
            gameManager.dataBased.settingData.volumeEffect = this.sfxVolume.value;
        }

        audioMixer.SetFloat(masterVolumeText, this.masterVolume.value);
        audioMixer.SetFloat(musicVolumeText, this.musicVolume.value);
        audioMixer.SetFloat(sfxVolumeText, this.sfxVolume.value);
        
    }

    public override void Hide()
    {
        applyButton.gameObject.SetActive(false);
        canvasSector.SetActive(false);
    }

    public override void Load()
    {
        if (gameManager != null)
        {
            masterVolume.value = gameManager.dataBased.settingData.volumeMaster;
            musicVolume.value = gameManager.dataBased.settingData.volumeMusic;
            sfxVolume.value = gameManager.dataBased.settingData.volumeEffect;
        }
        else
        {
            audioMixer.GetFloat(masterVolumeText,out float masterVolume);
            this.masterVolume.value = masterVolume;

            audioMixer.GetFloat(musicVolumeText, out float musicVolume);
            this.musicVolume.value = musicVolume;

            audioMixer.GetFloat(sfxVolumeText, out float sfxVolume);
            this.sfxVolume.value = sfxVolume;
        }
    }

    public override void Show()
    {
        applyButton.gameObject.SetActive(true);
        canvasSector.SetActive(true);
    }
}
