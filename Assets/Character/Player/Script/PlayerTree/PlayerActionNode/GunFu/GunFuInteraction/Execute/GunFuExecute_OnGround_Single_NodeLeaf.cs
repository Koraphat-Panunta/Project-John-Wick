using System;
using UnityEngine;

public class GunFuExecute_OnGround_Single_NodeLeaf : PlayerStateNodeLeaf,IGunFuExecuteNodeLeaf
{
    IGotGunFuAttackedAble gunFuExecuteAble;
    private BulletExecute bulletExecute;

    private bool isNextFrame;

    //private float elapesTime;
    private float warpingTime => _animationClip.length*_gunFuExecute_Single_ScriptableObject.warpingPhaseTimeNormalized - lenghtOffset;
    private float executeTime => _animationClip.length * _gunFuExecute_Single_ScriptableObject.executeTimeNormalized - lenghtOffset;
    //private float duration = 1.25f;
    //private float transitionAbleTime = 1;
    private bool isExecute;
    private bool isWarping;

    private float lenghtOffset => _animationClip.length*_gunFuExecute_Single_ScriptableObject.playerAnimationOffset;
    public enum GunFuExecutePhase
    {
        Enter,
        Execute,
        Exit,
    }
    public GunFuExecutePhase curExecutePhase;

    public string _stateName => throw new NotImplementedException();

    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => isExecute; set => isExecute = value; }

    public GunFuExecute_Single_ScriptableObject _gunFuExecute_Single_ScriptableObject => this.gunFuExecute_Single_ScriptableObject;
    private GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player;set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get => gunFuAble.executedAbleGunFu; set { } }
    public AnimationClip _animationClip { get => _gunFuExecute_Single_ScriptableObject.executeClip; set { } }

    public GunFuExecute_OnGround_Single_NodeLeaf(Player player, Func<bool> preCondition,GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject) : base(player, preCondition)
    {
        this.gunFuExecute_Single_ScriptableObject = gunFuExecute_Single_ScriptableObject;
    }

    public override void Enter()
    {
        curExecutePhase = GunFuExecutePhase.Enter;

        gunFuExecuteAble = gunFuAble.executedAbleGunFu;

        isNextFrame = false;

        _timer = 0;

        isExecute = false;

        isWarping = false;

        bulletExecute = new BulletExecute(player._currentWeapon);

        gunFuExecuteAble.TakeGunFuAttacked(this,player);

        base.Enter();
    }

    public override void Exit()
    {
        curExecutePhase = GunFuExecutePhase.Exit;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return _timer >= _animationClip.length*_gunFuExecute_Single_ScriptableObject.executeAnimationExitNormarlized - lenghtOffset;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        this._timer += Time.deltaTime;

        if (isNextFrame == false)
        {
            isNextFrame = true;
            return;
        }

        if(_timer <= warpingTime)
        {
            if (gunFuExecuteAble.curNodeLeaf is GotExecuteOnGround_NodeLeaf gotEx)
            {
                if (isWarping == false)
                {
                    float offsetForward = 0.6f;
                    float offsetRight = 0.3f;
                    Vector3 warpPos;
                    if (gotEx.isFacingUp)
                    
                        warpPos = gunFuExecuteAble._character.transform.position + gunFuExecuteAble._character.transform.forward * offsetForward - gunFuExecuteAble._character.transform.right * offsetRight;
                    
                    else
                        warpPos = gunFuExecuteAble._character.transform.position + gunFuExecuteAble._character.transform.forward * -offsetForward + gunFuExecuteAble._character.transform.right * offsetRight;


                    (player._movementCompoent as PlayerMovement).StartWarpingLinear(player.transform.position, warpPos, warpingTime, player.moveWarping, player._movementCompoent);
                    isWarping = true;
                }
                if (gotEx.isFacingUp)
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(gunFuExecuteAble._character.transform.forward * -1), _timer / warpingTime);
                else
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(gunFuExecuteAble._character.transform.forward), _timer / warpingTime);
            }
            
        }

        if(_timer >= executeTime && isExecute == false)
        {
            gunFuExecuteAble._damageAble.TakeDamage(bulletExecute);
            player._currentWeapon.PullTrigger();
            isExecute = true;
            curExecutePhase = GunFuExecutePhase.Execute;
            player.NotifyObserver(player,this);
        }
        
        base.UpdateNode();
    }
}

