using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{
    public bool isPlaying;
    [SerializeField] private AudioClip tarck;
    [SerializeField] private AudioSource source;
    // Start is called before the first frame update
    private void Start()
    {
        isPlaying = false;
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        
    }

    private void TriggerSoundTrack()
    {
        if (isPlaying == false)
        {
            source.clip = tarck;
            source.loop = true;
            source.Play();
            isPlaying = true;
        }
    }
}
