using System;
using System.Collections.Generic;
using UnityEngine;

public class GotGunFuHitNodeLeaf : EnemyStateLeafNode,IGotGunFuAttackNode,INodeLeafTransitionAble
{
    protected Animator animator;
    public string gotHitstateName { get; protected set; }
    public IGunFuAble gunFuAble => enemy.gunFuAbleAttacker;
    public IGotGunFuAttackedAble gotGunFuAttackedAble => enemy;
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
    float forceStop => enemy.hitedForceStop;
    public GotGunFuHitScriptableObject gotGunFuHitScriptableObject { get => _gotGunFuHitScriptableObject; }
    private GotGunFuHitScriptableObject _gotGunFuHitScriptableObject { get; set; }
    public INodeManager nodeManager { get => enemy.enemyStateManagerNode ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    private float legnhtOffset => _animationClip.length*gotGunFuHitScriptableObject.enterAnimationOffsetNormalized;
    public enum GotHitPhase
    {
        Enter,
        Exit,
    }
    public GotHitPhase curGotHitPhase { get;protected set; }
    public GotGunFuHitNodeLeaf(Enemy enemy,Func<bool> preCondition,GotGunFuHitScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition)
    {
        this._gotGunFuHitScriptableObject = gunFu_GotHit_ScriptableObject;
        _exitTime_Normalized = gunFu_GotHit_ScriptableObject.exitTimeNormalized;
        _animationClip = gunFu_GotHit_ScriptableObject.AnimationClip;
        this.animator = enemy.animator;
        gotHitstateName = gunFu_GotHit_ScriptableObject.gotHitStateName;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
       
        _timer = 0;
        enemy._movementCompoent.SetRotation(Quaternion.LookRotation((gunFuAble._character.transform.position - enemy.transform.position).normalized
            ,Vector3.up));
        animator.CrossFade(gotHitstateName, 0.005f, 0,_gotGunFuHitScriptableObject.enterAnimationOffsetNormalized);
        curGotHitPhase = GotHitPhase.Enter;
        base.Enter();
    }
    public override void Exit()
    {
        curGotHitPhase = GotHitPhase.Exit;
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        if(_timer >= _animationClip.length*_exitTime_Normalized - legnhtOffset)
            isComplete = true;

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        enemy._movementCompoent.MoveToDirWorld(Vector3.zero, forceStop, forceStop, MoveMode.MaintainMomentum);    
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {

        if (IsComplete())
            return true;

        if(enemy.isDead)
            return true;

        if(enemy._isPainTrigger
            ||enemy._triggerHitedGunFu)
            return true;

        return false;
    }

    public bool Transitioning()
    {
        return nodeLeafTransitionBehavior.Transitioning(this);
    }

    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.AddTransistionNode(this,node);
    }
}
