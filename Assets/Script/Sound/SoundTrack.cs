using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour,IObseverLevel
{
    public bool isPlaying;
    [SerializeField] private AudioClip tarck;
    [SerializeField] private AudioSource source;
    public void OnNotify(LevelManager level, LevelSubject.LevelEvent levelEvent)
    {
        if(levelEvent == LevelSubject.LevelEvent.ObjectiveUpdate)
        {
            if(isPlaying == false)
            {
                source.clip = tarck;
                source.loop = true;
                source.Play();
                isPlaying = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<LevelManager>().AddObserver(this);
        isPlaying = false;
    }
    private void OnDisable()
    {
        //FindObjectOfType<LevelManager>().RemoveObserver(this);
    }
    // UpdateNode is called once per frame
    void Update()
    {
        
    }
}
