using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GotGunFuExecuteNodeLeaf : EnemyStateLeafNode, IGotGunFuExecuteNodeLeaf
{
    public float _timer { get;set; }
    public AnimationClip _animationClip { get => gunFuExecuteScriptableObject.gotExecuteClip ; set { } }
    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;
    public IGunFuAble _executerGunFu => _gotExecutedGunFu.gunFuAbleAttacker;

    private string gotExecuteStateName;
    private GunFuExecute_Single_ScriptableObject gunFuExecuteScriptableObject;

    private Transform gunFuGotAttackedTransform => enemy.transform;

    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject => this.gunFuExecuteScriptableObject;

    public GotGunFuExecuteNodeLeaf(Enemy enemy, Func<bool> preCondition,string gotExecuteStateName) : base(enemy, preCondition)
    {
        this.gotExecuteStateName = gotExecuteStateName;
    }
    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;

        if (_executerGunFu.curGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            && gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject.gotGunFuStateName == this.gotExecuteStateName)
        {
            gunFuExecuteScriptableObject = gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject as GunFuExecute_Single_ScriptableObject;
            return true;
        }

        return false;
    }
    public override void Enter()
    {
        _gotExecutedGunFu._character.animator.CrossFade(gotExecuteStateName, 0.05f, 0, this.gunFuExecuteScriptableObject.opponentAnimationOffset);
        _timer = this.gunFuExecuteScriptableObject.executeClip.length * gunFuExecuteScriptableObject.opponentAnimationOffset;
        _gotExecutedGunFu._character.enableRootMotion = true;
       
        base.Enter();
    }
    public override void Exit()
    {
        _gotExecutedGunFu._character.enableRootMotion = false;
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        float t = _timer/_animationClip.length;

        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(_timer > gunFuExecuteScriptableObject.executeClip.length)
            return true;
        return false;
    }

}
