using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SubjectPlayer;

public class PlayerAudio : MonoBehaviour,IObserverPlayer
{
    [SerializeField] private Player player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footStep;

    void Start()
    {
        player = GetComponent<Player>();
        player.AddObserver(this);
    }
    private void Update()
    {
        MoveSound();
    }
    private void MoveSound()
    {
        PlayerStateManager playerStateManager = player.playerStateManager;
        if (playerStateManager.Current_state == playerStateManager.move)
        {
            float timingRate = 2.8f;
            footStepTiming += Time.deltaTime * timingRate;
            if (footStepTiming >= 1)
            {
                audioSource.clip = footStep;
                audioSource.Play();
                footStepTiming = 0;
            }
        }
        else if (playerStateManager.Current_state == playerStateManager.sprint)
        {
            float timingRate = 4.2f;
            footStepTiming += Time.deltaTime * timingRate;
            if (footStepTiming >= 1)
            {
                audioSource.clip = footStep;
                audioSource.Play();
                footStepTiming = 0;
            }
        }
        else
        {
            footStepTiming = 0;
        }
    }
    float footStepTiming = 0;
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        
    }
}
