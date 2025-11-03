using System;
using System.Collections.Generic;
using UnityEngine;

public class FiringNode : WeaponLeafNode,INodeLeafTransitionAble
{
    bool isFiring;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public FiringNode(Weapon weapon,INodeManager nodeManager, Func<bool> preCondition) : base(weapon, preCondition)
    {
        this.nodeManager = nodeManager;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    public override void Enter()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        isFiring = false;
        Weapon.bulletStore[BulletStackType.Chamber] -= 1;
        Weapon.bulletSpawner.SpawnBullet(Weapon);
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.Firing);
        Weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
             <FiringNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        isFiring = true;
    }

    public override void Exit()
    {
       
    }


    public override void UpdateNode()
    {
        this.TransitioningCheck();
    }
    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return isFiring;
    }

    public override bool IsReset()
    {
        return IsComplete();
    }

    public bool TransitioningCheck()
    {
        return nodeLeafTransitionBehavior.TransitioningCheck(this);
    }

    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
    }

    //public override bool Precondition()
    //{
    //    return Weapon.bulletStore[BulletStackType.Chamber] > 0 ;
    //}



}
