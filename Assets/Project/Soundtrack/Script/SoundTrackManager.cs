using System.Collections;
using UnityEngine;

public class SoundTrackManager : MonoBehaviour
{
    [SerializeField] private AudioClip Opening;
  
    [SerializeField] private AudioClip LeveMansion;
    [SerializeField] private AudioClip LevelHotel;
    [SerializeField] private AudioClip prologueLevelMusic;
    public AudioClip openingTrack { get => Opening; }
    public AudioClip theMansionTrack { get => LeveMansion; }
    public AudioClip theHotelTrack { get => LevelHotel; }
    public AudioClip prologueTrack { get => prologueLevelMusic; }

    [SerializeField] private AudioSource source;
    private float settingVolume;
    [SerializeField] private string curTrack;
    // Start is called before the first frame update
    private void Start()
    {
        settingVolume = source.volume;
        DontDestroyOnLoad(this);
    }
   
    public void PlaySoundTrack(AudioClip audioClip)
    {
        Debug.Log("PlaySoundTrack");
        source.clip = audioClip;
        source.loop = true;
        source.Play();
    }
    public void StopSoundTrack(float fadeDuration)
    {
        StartCoroutine(Stop(fadeDuration));
    }
    public AudioClip GetCurSoundTrack() => source.clip; 
    private float fadeElapesTime = 0;
    private IEnumerator Stop(float fadeDuration)
    {
        for(fadeElapesTime = 0;fadeElapesTime <= fadeDuration;fadeElapesTime += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(settingVolume, 0, fadeElapesTime / fadeDuration);
            yield return null;
        }
        source.Stop();
        source.volume = settingVolume;
    }
}
