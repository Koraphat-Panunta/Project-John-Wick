using System;
using UnityEngine;

public class GotRestrictNodeLeaf : EnemyStateLeafNode,IGotGunFuAttackAbleNode
{
    public float _exitTime_Normalized { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float _timer { get ; set ; }
    public AnimationClip _animationClip { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override bool isComplete { get  ; protected set ; }
    private IGunFuAble attacker => enemy.gunFuAbleAttacker;
    public enum GotRestrictPhase
    {
        Enter,
        Stay,
        Exit
    }
    public GotRestrictPhase curGotHitRestrictPhase { get; private set; }
    private GotRestrictScriptableObject gotRestrictScriptableObject;
    private RestrictGunFuStateNodeLeaf attackerRestrict ;
    private Animator animator { get => enemy.animator; }

    private string gotRestrictEnter = "GotRestrictEnter";
    private string gotRestrictStay = "GotRestricting_Stay";
    private string gotRestrictExit = "GotRestrict_Exit";
    public GotRestrictNodeLeaf(GotRestrictScriptableObject gotRestrictScriptableObject,Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.gotRestrictScriptableObject = gotRestrictScriptableObject;
    }

    public override void Enter()
    {
        curGotHitRestrictPhase = GotRestrictPhase.Enter;
        _timer = 0;
        this.attackerRestrict = enemy.gunFuAbleAttacker.curGunFuNode as RestrictGunFuStateNodeLeaf;

        animator.CrossFade(gotRestrictEnter, 0.075f, 0,gotRestrictScriptableObject.gotRestrictEnter_enterNormalized);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        enemy.friendlyFirePreventingBehavior.DisableFriendlyFirePreventing();
        enemy.NotifyObserver(enemy, this); 
        base.Enter();
    }

    public override void Exit()
    {
        enemy.friendlyFirePreventingBehavior.EnableFriendlyFirePreventing();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {

        if (IsComplete())
            return true;

        if (enemy.isDead)
            return true;

        if(curGotHitRestrictPhase == GotRestrictPhase.Exit
            && enemy._isPainTrigger)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        switch (curGotHitRestrictPhase)
        {
            case GotRestrictPhase.Enter: 
                {
                    _timer += Time.deltaTime;
                    if(attacker.curGunFuNode is RestrictGunFuStateNodeLeaf == false)
                    {
                        isComplete = true;
                    }
                    if(_timer >= gotRestrictScriptableObject.gotRestrictEnter_exitNormalized)
                    {
                        _timer = 0;
                        curGotHitRestrictPhase = GotRestrictPhase.Stay;
                        animator.CrossFade(gotRestrictStay, 0.075f, 0);
                        enemy.NotifyObserver(enemy,this);
                    }
                }
                break;
            case GotRestrictPhase.Stay:
                {
                    if (attacker.curGunFuNode is RestrictGunFuStateNodeLeaf == false)
                    {
                        isComplete = true;
                    }
                    if (attackerRestrict.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                    {
                        _timer = 0;
                        curGotHitRestrictPhase = GotRestrictPhase.Exit;
                        animator.CrossFade(gotRestrictExit, 0.075f, 0,gotRestrictScriptableObject.gotRestrictExit_enterNormalized);
                        enemy.NotifyObserver(enemy, this);
                    }
                }
                break;
            case GotRestrictPhase.Exit:
                {
                    _timer += Time.deltaTime;

                    enemy.enemyMovement.MoveToDirWorld(Vector3.zero,gotRestrictScriptableObject.gotRestrictExit_BreakForce,gotRestrictScriptableObject.gotRestrictExit_BreakForce,IMovementCompoent.MoveMode.MaintainMomentum);
                    if(_timer >= gotRestrictScriptableObject.gotRestrictExitClip.length * gotRestrictScriptableObject.gotRestrictExit_exitNormalized)
                    {
                        isComplete = true;
                        _timer = 0;
                        enemy.NotifyObserver(enemy, this);
                    }
                }
                break;
        }
        base.UpdateNode();
    }
}
