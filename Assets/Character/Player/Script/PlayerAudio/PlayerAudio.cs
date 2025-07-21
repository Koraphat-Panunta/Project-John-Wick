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
        PlayerStateNodeLeaf playerState = (player.playerStateNodeManager as INodeManager).GetCurNodeLeaf() as PlayerStateNodeLeaf;
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
  
    public void OnNotify<T>(Player player, T node)
    {
        switch (node)
        {
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    switch (gunFuHitNodeLeaf._stateName)
                    {
                        case "DodgeSpinKick":
                            {
                                if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                                    PlayAudio(kick);
                                break;
                            }
                        case "Hit3":
                            {
                                if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                                    PlayAudio(kick);
                                break;
                            }
                        default:
                            {
                                if(gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                                    PlayAudio(hit);
                                break;
                            }
                    }


                    
                    break;
                }
            case PlayerDodgeRollStateNodeLeaf dodgeRollStateNodeLeaf:
                {
                    if(dodgeRollStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                    PlayAudio(dodgeRollSound);
                    break;
                }
        }
    }
    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

   
}
