using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SubjectPlayer;

public class PlayerAudio : MonoBehaviour,IObserverPlayer
{
    [SerializeField] private Player player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footsStep;

    [SerializeField] private AudioClip dodgeRollSound;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip kick;

    [Range(0,100)]
    [SerializeField] private float timingRateWalk;

    [Range(0, 100)]
    [SerializeField] private float timingRateSprint;

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
        PlayerStateNodeLeaf playerState = player.playerStateNodeManager.curNodeLeaf as PlayerStateNodeLeaf;
        if (playerState is PlayerStandMoveNodeLeaf
            || playerState is PlayerCrouch_Move_NodeLeaf
            || playerState is PlayerInCoverStandMoveNodeLeaf)
        {

            footStepTiming += Time.deltaTime * timingRateWalk;
            if (footStepTiming >= 1)
            {
                audioSource.clip = footsStep[Random.Range(0,footsStep.Length-1)];
                audioSource.Play();
                footStepTiming = 0;
            }
        }
        else if (playerState is PlayerSprintNode)
        {

            footStepTiming += Time.deltaTime * timingRateSprint;
            if (footStepTiming >= 1)
            {
                audioSource.clip = footsStep[Random.Range(0, footsStep.Length - 1)];
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
        if (playerAction == PlayerAction.Dodge)
            PlayAudio(dodgeRollSound);

        if(playerAction == PlayerAction.GunFuAttack)
        {
            if (player.playerStateNodeManager.curNodeLeaf is Hit1GunFuNode
                || player.playerStateNodeManager.curNodeLeaf is Hit2GunFuNode
                || player.playerStateNodeManager.curNodeLeaf is RestrictGunFuStateNodeLeaf)
                PlayAudio(hit);

            else if (player.playerStateNodeManager.curNodeLeaf is DodgeSpinKicklGunFuNodeLeaf
                || player.playerStateNodeManager.curNodeLeaf is KnockDown_GunFuNode)
                PlayAudio(kick);
                
        }


    }
    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void OnNotify(Player player)
    {
    }
}
