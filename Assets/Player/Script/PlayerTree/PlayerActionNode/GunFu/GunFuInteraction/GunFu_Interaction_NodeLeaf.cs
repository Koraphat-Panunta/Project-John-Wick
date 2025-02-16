using System;
using System.Collections.Generic;
using UnityEngine;
    
public abstract class GunFu_Interaction_NodeLeaf : PlayerStateNodeLeaf, IGunFuNode,INodeLeafTransitionAble
{

    #region ImplementIGunFuNode
    public IGunFuAble gunFuAble { get; set; }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    #endregion

    #region ImplementINodeTransitionAble
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    #endregion

    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
    public string stateName { get; protected set; }
    protected Transform targetAdjustTransform;

    protected GunFu_Interaction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition)
    {
        gunFuAble = player as IGunFuAble;
        transitionAbleNode = new Dictionary<INodeLeaf, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

        _animationClip = gunFuInteraction_ScriptableObject.AinimnationClip;
        _transitionAbleTime_Nornalized = gunFuInteraction_ScriptableObject.TransitionAbleTime_Normalized;
        stateName = gunFuInteraction_ScriptableObject.StateName;

        targetAdjustTransform = gunFuAble._targetAdjustTranform;
    }

    public override void Enter()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        _timer = 0;
        attackedAbleGunFu = gunFuAble.attackedAbleGunFu;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

        base.Enter();
    }
    public override void Exit()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
        base.Exit();
    }
    public override void UpdateNode()
    {
        Transitioning();

        _timer += Time.deltaTime;
        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return base.IsReset();
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);
    
    public void AddTransitionNode(INodeLeaf nodeLeaf)=> nodeLeafTransitionBehavior.AddTransistionNode(this, nodeLeaf);
   

 
}

