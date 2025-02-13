using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFuHitNodeLeaf : PlayerStateNodeLeaf ,IGunFuNode,INodeLeafTransitionAble
{
    public float _timer { get; set; }
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _exitTime_Normalized { get ; set ; }
    public float hitAbleTime_Normalized;
    public float endHitableTime_Normalized;
    public AnimationClip _animationClip { get; set; }

    protected bool isHiting;

    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public IGunFuAble gunFuAble { get; set; }
    public INodeManager nodeManager { get ; set ; }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public GunFuHitNodeLeaf(Player player,Func<bool> preCondition,GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition)
    {
        this._transitionAbleTime_Nornalized = gunFuNodeScriptableObject.TransitionAbleTime_Normalized;
        this._exitTime_Normalized = gunFuNodeScriptableObject.ExitTime_Normalized;
        this.hitAbleTime_Normalized = gunFuNodeScriptableObject.HitAbleTime_Normalized;
        this.endHitableTime_Normalized = gunFuNodeScriptableObject.EndHitAbleTime_Normalized;
        this._animationClip = gunFuNodeScriptableObject.animationClip;

        gunFuAble = player as IGunFuAble;
        nodeManager = player.playerStateNodeManager;
        transitionAbleNode = new Dictionary<INodeLeaf, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
        this.attackedAbleGunFu = gunFuAble.attackedAbleGunFu;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        _timer = 0;
        isHiting = false;
        player._triggerGunFu = false;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);

        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        Transitioning();

        _timer += Time.deltaTime;
        if (_timer >= _transitionAbleTime_Nornalized * _animationClip.length)
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
            

        if(_timer >= _exitTime_Normalized * _animationClip.length)
            isComplete = true;

        base.UpdateNode();
    }

    RotateObjectToward rotateObjectToward = new RotateObjectToward();
    protected Vector3 targetPos;

    protected void LerpingToTargetPos()
    {
        if (Vector3.Distance(targetPos, player.transform.position) > 0.25f)
        {
            rotateObjectToward.RotateTowardsObjectPos(targetPos, player.gameObject, 12);
            player.playerMovement.SnapingMovement(targetPos, Vector3.zero, 600 * Time.deltaTime);
        }
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);
    public void AddTransitionNode(INodeLeaf nodeLeaf) => nodeLeafTransitionBehavior.AddTransistionNode(this, nodeLeaf);
   
}
