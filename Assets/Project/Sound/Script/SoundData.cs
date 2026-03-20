using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundData 
{
    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;
    public float volume;
    [Range(0,1)]
    public float _3DWeight;

}
