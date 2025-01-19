using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : GunFuHitNodeLeaf
{


    protected Vector3 targetPos;
    protected IGunFuDamagedAble gunFuDamagedAble;
    private bool isHiting;

    private bool gunFuTriggerBuufer;

    public Hit1GunFuNode(Player player, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player, gunFuNodeScriptableObject)
    {
        
    }

    public override void Enter()
    {
        _timer = 0;
        gunFuDamagedAble = null;
        isHiting = false;
        gunFuTriggerBuufer = false;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
        base.Exit();
    }
    public override void Update()
    {
        //if(_timer>=hitAbleTime_Normalized * _animationClip.length && isHiting == false)
        //{
        //    if (gunFuDamagedAble != null)
        //        gunFuDamagedAble.TakeGunFuAttacked(this);
        //    isHiting=true;
        //}
        if(_timer >= _animationClip.length * 0f 
            && player._triggerGunFu)
            gunFuTriggerBuufer = true;
        

        
        if(_timer >= _animationClip.length * _transitionAbleTime_Nornalized)
        {

        }
        if (_isTransitionAble && 
            (player._triggerGunFu||gunFuTriggerBuufer))
            player.ChangeNode(player.Hit2GunFuNode);
        base.Update();
    }
    public override void FixedUpdate()
    {
        player.playerMovement.FreezingCharacter();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (_isExit)
        {
            if (player.inputMoveDir_Local.magnitude > 0)
                return true;
        }

        if (_isExit)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if(player._triggerGunFu)
            return true;

        return false;
    }

    private bool DetectTarget()
    {
        Vector3 casrDir;
       
        if (Vector3.Angle(player.transform.forward, player._gunFuAimDir) <= player._limitAimAngleDegrees)
        {
            Debug.Log(Vector3.Angle(player.transform.forward,player._gunFuAimDir));

            casrDir = new Vector3(player._gunFuAimDir.x, 0, player._gunFuAimDir.z);
        }
        else
        {
            casrDir = player.transform.forward;
        }

        if (Physics.SphereCast(player.transform.position + new Vector3(0, 1, 0), player._shpere_Raduis_Detecion, casrDir, out RaycastHit hitInfo,player._sphere_Distance_Detection, 0))
        {
            if(hitInfo.collider.TryGetComponent<IGunFuDamagedAble>( out IGunFuDamagedAble gunFuDamagedAble))
            {
                this.gunFuDamagedAble = gunFuDamagedAble;
                return true;
            }
            return false;
        }
        return false;



    }


}
