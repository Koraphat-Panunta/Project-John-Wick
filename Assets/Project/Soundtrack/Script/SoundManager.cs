using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour,IInitializedAble
{

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioSource globalAudioSource;
    private float settingVolume;
    [SerializeField] private string curTrack;
    // Start is called before the first frame update

    public void Initialized()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        DontDestroyOnLoad(this);
        settingVolume = globalAudioSource.volume;
    }
   
   
    public void PlaySoundTrack(AudioClip audioClip)
    {
        globalAudioSource.clip = audioClip;
        globalAudioSource.loop = true;
        globalAudioSource.Play();
    }
    public void StopSoundTrack(float fadeDuration)
    {
        StartCoroutine(Stop(fadeDuration));
    }

    public void SetMasterVolume(float value)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.audioSetting.MasterVolume = value;

        this.audioMixer.SetFloat("MasterVolume", this.GetDecibel(value));
        this.audioMixer.GetFloat("MasterVolume", out float v);
        Debug.Log("Master V = " + v);


    }
    public void SetMusicVolume(float value)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.audioSetting.MusicVolume = value;
        this.audioMixer.SetFloat("MusicVolume", this.GetDecibel(value));
    }
    public void SetSFXVolume(float value)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.audioSetting.SoundEffectVolume = value;
        this.audioMixer.SetFloat("SFXVolume", this.GetDecibel(value));
    }

    private float GetDecibel(float audioValue)//Scale 0-10
    {
        return Mathf.Log10(Mathf.Clamp(audioValue / 10f, 0.0001f, 1f)) * 20f;
    }

    public AudioClip GetCurSoundTrack() => globalAudioSource.clip; 
    private float fadeElapesTime = 0;
    private IEnumerator Stop(float fadeDuration)
    {
        for(fadeElapesTime = 0;fadeElapesTime <= fadeDuration;fadeElapesTime += Time.deltaTime)
        {
            globalAudioSource.volume = Mathf.Lerp(settingVolume, 0, fadeElapesTime / fadeDuration);
            yield return null;
        }
        globalAudioSource.Stop();
        globalAudioSource.volume = settingVolume;
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            this.audioListener.transform.SetParent(Camera.main.transform,false);
        }
        else
            this.audioListener.transform.SetParent(null);
    }

   
}
