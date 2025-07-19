using System;
using System.Collections.Generic;
using UnityEngine;
    
public abstract class PlayerGunFu_Interaction_NodeLeaf : PlayerStateNodeLeaf, IGunFuNode,INodeLeafTransitionAble
{

    #region ImplementIGunFuNode
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }
    #endregion

    #region ImplementINodeTransitionAble
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    #endregion

    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
    public string stateName { get; protected set; }
    protected Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;

    public string _stateName { get => stateName; }
    public INodeLeaf transitioningNodeLeaf { get; set; }

    protected PlayerGunFu_Interaction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition)
    {
        gunFuAble = player as IGunFuAble;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

        _animationClip = gunFuInteraction_ScriptableObject.AinimnationClip;
        _transitionAbleTime_Nornalized = gunFuInteraction_ScriptableObject.TransitionAbleTime_Normalized;
        stateName = gunFuInteraction_ScriptableObject.StateName;
    }

    protected PlayerGunFu_Interaction_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        gunFuAble = player as IGunFuAble;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();


    }

    public override void Enter()
    {
        transitioningNodeLeaf = null;

        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        _timer = 0;
        gotGunFuAttackedAble = gunFuAble.attackedAbleGunFu;

        player.NotifyObserver(player,this);

        base.Enter();
    }
    public override void Exit()
    {
       

        player.NotifyObserver(player,this);
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
       

        if (IsComplete())
            return true;

        return base.IsReset();
    }
    public override bool IsComplete()
    {

        return base.IsComplete();
    }
    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);
    
    public void AddTransitionNode(INode node)=> nodeLeafTransitionBehavior.AddTransistionNode(this, node);
   

 
}

