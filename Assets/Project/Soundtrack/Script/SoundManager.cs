using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour,IInitializedAble
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioSource globalAudioSource;
    private float settingVolume;
    [SerializeField] private string curTrack;
    // Start is called before the first frame update

    public void Initialized()
    {
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

    public void SetVolume(AudioSetting audioSetting)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.audioSetting = audioSetting;

        this.audioMixer.SetFloat("Master",this.GetDecibel(audioSetting.MasterVolume));
        this.audioMixer.SetFloat("Music", this.GetDecibel(audioSetting.MusicVolume));
        this.audioMixer.SetFloat("SFX", this.GetDecibel(audioSetting.SoundEffectVolume));
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
        if(Camera.main != null)
        {
            this.audioListener.transform.position = Camera.main.transform.position;
        }
        else
            this.audioListener.transform.position = this.transform.position;
    }

   
}
