using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFuHitNodeLeaf : PlayerActionNodeLeaf ,IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _exitTime_Normalized { get ; set ; }
    public float _timer { get ; set ; }
    public float hitAbleTime_Normalized;
    public float endHitableTime_Normalized;
    public AnimationClip _animationClip { get; set; }
    public bool _isExit { get => _timer >= _animationClip.length * _exitTime_Normalized ; set { } }
    public bool _isTransitionAble { get => _timer >= _transitionAbleTime_Nornalized * _animationClip.length ; set { } }

    protected bool isDetectTarget;

    public IGunFuDamagedAble gunFuDamagedAble;
 
    public GunFuHitNodeLeaf(Player player,GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player)
    {
        this._transitionAbleTime_Nornalized = gunFuNodeScriptableObject.TransitionAbleTime_Normalized;
        this._exitTime_Normalized = gunFuNodeScriptableObject.ExitTime_Normalized;
        this.hitAbleTime_Normalized = gunFuNodeScriptableObject.HitAbleTime_Normalized;
        this.endHitableTime_Normalized = gunFuNodeScriptableObject.EndHitAbleTime_Normalized;
        this._animationClip = gunFuNodeScriptableObject.animationClip;
    }
    public override void Enter()
    {
        _timer = 0;

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _transitionAbleTime_Nornalized* _animationClip.length)
            _isTransitionAble = true;
    }

    RotateObjectToward rotateObjectToward = new RotateObjectToward();
    protected Vector3 targetPos;

    protected void LerpingToTargetPos()
    {
        Debug.Log("target Pos = " + targetPos);

        if (Vector3.Distance(targetPos, player.transform.position) > 0.25f)
        {
            rotateObjectToward.RotateTowardsObjectPos(targetPos, player.gameObject, 12);
            player.playerMovement.SnapingMovement(targetPos, Vector3.zero, 600 * Time.deltaTime);
        }
    }

    protected bool DetectTarget()
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

        if (Physics.SphereCast(player.RayCastPos.transform.position, player._shpere_Raduis_Detecion, casrDir, out RaycastHit hitInfo, player._sphere_Distance_Detection, player._layerTarget))
        {
            if (hitInfo.collider.TryGetComponent<IGunFuDamagedAble>(out IGunFuDamagedAble gunFuDamagedAble))
            {
                this.gunFuDamagedAble = gunFuDamagedAble;

                targetPos = new Vector3(gunFuDamagedAble._gunFuHitedAble.position.x, player.transform.position.y, gunFuDamagedAble._gunFuHitedAble.position.z);
                return true;
            }
            targetPos = player.transform.position ;
            return false;
        }
        targetPos = player.transform.position ;

        return false;



    }
}
