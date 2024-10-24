using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioListener audio_listener;
    private Player player;

    void Start()
    {
        audio_listener = GetComponent<AudioListener>();
        player = FindObjectOfType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position;
        gameObject.transform.rotation = player.transform.rotation;
    }
}
