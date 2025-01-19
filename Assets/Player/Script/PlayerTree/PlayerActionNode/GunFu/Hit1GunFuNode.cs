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
        player._triggerGunFu = false;
        _timer = 0;
        gunFuDamagedAble = null;
        isHiting = false;
        gunFuTriggerBuufer = false;

        DetectTarget();

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
        
        if(_timer >= _animationClip.length * 0f 
            && player._triggerGunFu)
            gunFuTriggerBuufer = true;

        LerpingToTargetPos();

        if(_timer>=_animationClip.length*hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized
            && isHiting == false)
        {
            if (gunFuDamagedAble != null)
                gunFuDamagedAble.TakeGunFuAttacked(this,player.transform.position);
            isHiting = true;
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

        if (Vector3.Angle(player.RayCastPos.transform.forward, player._gunFuAimDir) <= player._limitAimAngleDegrees)
        {
            casrDir = new Vector3(player._gunFuAimDir.x, 0, player._gunFuAimDir.z);
        }
        else
        {
            casrDir = player.RayCastPos.transform.forward;
        }

        if (Physics.SphereCast(player.RayCastPos.transform.position , player._shpere_Raduis_Detecion, casrDir, out RaycastHit hitInfo, player._sphere_Distance_Detection, player._layerTarget))
        {
            if (hitInfo.collider.TryGetComponent<IGunFuDamagedAble>(out IGunFuDamagedAble gunFuDamagedAble))
            {
                this.gunFuDamagedAble = gunFuDamagedAble;
                 targetPos = new Vector3(hitInfo.point.x, player.transform.position.y, hitInfo.point.z);
                return true;
            }
            targetPos = new Vector3(hitInfo.point.x,player.transform.position.y,hitInfo.point.z);
            return false;
        }
        targetPos = player.transform.position + new Vector3(casrDir.x,0, casrDir.z) * player._sphere_Distance_Detection;
        return false;



    }
    RotateObjectToward rotateObjectToward = new RotateObjectToward();
    private void LerpingToTargetPos()
    {
        Debug.Log("target Pos = "+targetPos);
        rotateObjectToward.RotateTowardsObjectPos(targetPos, player.gameObject, 12);
        Vector3 lerpingPos = targetPos + (player.transform.position - targetPos).normalized*1.25f;
        player.playerMovement.WarpingMovementCharacter(lerpingPos, Vector3.zero, 600 * Time.deltaTime);
    }

}
