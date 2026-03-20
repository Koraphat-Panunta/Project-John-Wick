using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundEmitter : MonoBehaviour
{
    public AudioSource audioSource;

    protected Coroutine coroutine;

    public void Build(SoundData soundData)
    {
        this.audioSource.clip = soundData.audioClip;
        this.audioSource.outputAudioMixerGroup = soundData.audioMixerGroup;
        this.audioSource.volume = soundData.volume;
        this.audioSource.spatialBlend = soundData._3DWeight;
    }

    public void Play()
    {
        this.audioSource.Play();

        if(this.coroutine != null)
            this.StopCoroutine(this.coroutine);

        this.coroutine = this.StartCoroutine(this.ReturnAudioEmitter());
    }

    public void Clear()
    {
        this.audioSource.clip = null;
        this.audioSource.outputAudioMixerGroup = null;
    }

    IEnumerator ReturnAudioEmitter()
    {
        yield return new WaitWhile(()=> this.audioSource.isPlaying);
        SoundEmitterManager.Instance.ReturnSoundEmitter(this);
        this.Clear();
        this.coroutine = null;
    }

}
