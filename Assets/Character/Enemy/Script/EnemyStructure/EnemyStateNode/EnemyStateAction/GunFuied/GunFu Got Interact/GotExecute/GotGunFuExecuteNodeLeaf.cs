using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GotGunFuExecuteNodeLeaf : EnemyStateLeafNode, IGotGunFuExecuteNodeLeaf
{
    public float _timer { get;set; }
    public AnimationClip _animationClip { get => gunFuExecuteScriptableObject.gotExecuteClip ; set { } }
    private IGunFuAble gunFuAbleAttacker => enemy.gunFuAbleAttacker;
    private IGotGunFuAttackedAble gunFuGotAttackedAble => enemy;

    private string gotExecuteStateName;
    private GunFuExecute_Single_ScriptableObject gunFuExecuteScriptableObject;
    private List<RootMotionKeyframe> opponentGotGunFuRootMotionKeyframeDelta;

    private bool isPopulateRootMotionKeyframe;

    private Transform gunFuGotAttackedTransform => enemy.transform;
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

        if(isPopulateRootMotionKeyframe == false)
        {
            PopulateRootMotionKeyframe();
            isPopulateRootMotionKeyframe = true;
        }
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        float t = _timer/_animationClip.length;

        RootMotionDeltaSampler.GetDeltaAtTime(
           opponentGotGunFuRootMotionKeyframeDelta
           , t
           , out Vector3 deltaOpponentPos
           , out Quaternion deltaOpponentRot);

        Debug.Log("deltaOpponentPos = " + deltaOpponentPos);


        gunFuGotAttackedTransform.position
          = gunFuGotAttackedTransform.position
          + gunFuGotAttackedTransform.forward * deltaOpponentPos.z
          + gunFuGotAttackedTransform.right * deltaOpponentPos.x
          + gunFuGotAttackedTransform.up * deltaOpponentPos.y;

        gunFuGotAttackedTransform.rotation *= deltaOpponentRot;

        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(_timer > gunFuExecuteScriptableObject.executeClip.length)
            return true;
        return false;
    }
    private async void PopulateRootMotionKeyframe()
    {
        await WaitForNextFrame();
        gunFuGotAttackedAble._animator.applyRootMotion = true;
        opponentGotGunFuRootMotionKeyframeDelta = RootMotionBaker.BakeRootMotion(gunFuExecuteScriptableObject.gotExecuteClip, gunFuGotAttackedAble._animator.gameObject, 60);
        gunFuGotAttackedAble._animator.applyRootMotion = false;


    }
    private async Task WaitForNextFrame()
    {
        await Task.Yield();
    }

}
