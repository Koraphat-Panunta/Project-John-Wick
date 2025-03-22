using System;
using UnityEngine;

public class GunFuExecuteNodeLeaf : GunFu_Interaction_NodeLeaf,INodeLeafTransitionAble
{
    IGunFuGotAttackedAble gunFuExecuteAble;
    private BulletExecute bulletExecute;

    private bool isNextFrame;
    private bool isEnableTransitionAble;

    private float elapesTime;
    private float warpingTime = 0.167f;
    private float executeTime = 0.75f;
    private float duration = 1.25f;
    private float transitionAbleTime = 1;
    private bool isExecute;
    private bool isWarping;

    AnimationCurve warpingCurve;
    public GunFuExecuteNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        gunFuExecuteAble = player.executedAbleGunFu;

        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);

        isNextFrame = false;

        isEnableTransitionAble = false;

        elapesTime = 0;

        isExecute = false;

        isWarping = false;

        bulletExecute = new BulletExecute(player.currentWeapon);

        gunFuExecuteAble.TakeGunFuAttacked(this,player);

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return elapesTime >= duration;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        this.elapesTime += Time.deltaTime;

        if (isNextFrame == false)
        {
            isNextFrame = true;
            return;
        }

        if(elapesTime <= warpingTime)
        {
            if (gunFuExecuteAble.curNodeLeaf is GotExecuteOnGround_GotInteract_NodeLeaf gotEx)
            {
                if (isWarping == false)
                {
                    float offsetForward = 0.6f;
                    float offsetRight = 0.3f;
                    Vector3 warpPos;
                    if (gotEx.isFacingUp)
                    
                        warpPos = gunFuExecuteAble._gunFuAttackedAble.position + gunFuExecuteAble._gunFuAttackedAble.forward * offsetForward - gunFuExecuteAble._gunFuAttackedAble.right * offsetRight;
                    
                    else
                        warpPos = gunFuExecuteAble._gunFuAttackedAble.position + gunFuExecuteAble._gunFuAttackedAble.forward * -offsetForward + gunFuExecuteAble._gunFuAttackedAble.right * offsetRight;


                    player.playerMovement.StartWarpingLinear(player.transform.position, warpPos, warpingTime, player.moveWarping, player.playerMovement);
                    isWarping = true;
                }
                if (gotEx.isFacingUp)
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(gunFuExecuteAble._gunFuAttackedAble.forward * -1), elapesTime / warpingTime);
                else
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(gunFuExecuteAble._gunFuAttackedAble.forward), elapesTime / warpingTime);
            }
            
        }

        if (elapesTime >= transitionAbleTime && isEnableTransitionAble == false)
        {
            isEnableTransitionAble = true;
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
        }

        if(elapesTime >= executeTime && isExecute == false)
        {
            gunFuExecuteAble._damageAble.TakeDamage(bulletExecute);
            player.currentWeapon.PullTrigger();
            isExecute = true;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuAttack);
        }
        
        base.UpdateNode();
    }
}
public class BulletExecute : Bullet
{
    public BulletExecute(Weapon weapon) : base(weapon)
    {
        myType = weapon.bullet.myType;
    }

    public override float hpDamage { get => 10000 ; set { } }
    public override float impactDamage { get => 0 ; set { } }
    public override float recoilKickBack { get => 0; set { } }
    public override BulletType myType { get ; set; }
}
