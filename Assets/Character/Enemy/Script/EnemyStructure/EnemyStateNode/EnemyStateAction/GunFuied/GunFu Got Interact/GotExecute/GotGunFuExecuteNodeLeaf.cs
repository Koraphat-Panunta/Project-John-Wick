using System;
using System.Collections.Generic;
using UnityEngine;

public class GotGunFuExecuteNodeLeaf : EnemyStateLeafNode, IGotGunFuExecuteNodeLeaf
{
    public float _timer { get;set; }
    public AnimationClip _animationClip { get ; set ; }
    private IGunFuAble gunFuAbleAttacker => enemy.gunFuAbleAttacker;
    private IGotGunFuAttackedAble gunFuGotAttackedAble => enemy;

    private string gotExecuteStateName;
    private GunFuExecute_Single_ScriptableObject gunFuExecuteScriptableObject;
    public GotGunFuExecuteNodeLeaf(Enemy enemy, Func<bool> preCondition,string gotExecuteStateName) : base(enemy, preCondition)
    {
        this.gotExecuteStateName = gotExecuteStateName;
    }
    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;

        if (gunFuAbleAttacker.curGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            && gunFuExecuteNodeLeaf._gunFuExecute_Single_ScriptableObject.gotGunFuStateName == this.gotExecuteStateName)
        {
            gunFuExecuteScriptableObject = gunFuExecuteNodeLeaf._gunFuExecute_Single_ScriptableObject;
            return true;
        }

        return false;
    }
    public override void Enter()
    {
        gunFuGotAttackedAble._animator.CrossFade(gotExecuteStateName, 0.05f, 0, this.gunFuExecuteScriptableObject.opponentAnimationOffset);
        _timer = this.gunFuExecuteScriptableObject.executeClip.length * gunFuExecuteScriptableObject.opponentAnimationOffset;
        base.Enter();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(_timer > gunFuExecuteScriptableObject.executeClip.length)
            return true;
        return false;
    }

}
