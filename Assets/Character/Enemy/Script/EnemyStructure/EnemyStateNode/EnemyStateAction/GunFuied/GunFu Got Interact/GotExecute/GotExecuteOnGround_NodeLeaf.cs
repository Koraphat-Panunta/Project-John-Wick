using System;
using UnityEngine;

public class GotExecuteOnGround_NodeLeaf : EnemyStateLeafNode,IGotGunFuExecuteNodeLeaf
{
    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _ragdollBoneTransforms;
    private BoneTransform[] _startAnimBoneTransforms;

    private Animator _animator => enemy.animator;
    private float duration => _animationClip.length;

    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;

    public IGunFuAble _executerGunFu => _gotExecutedGunFu.gunFuAbleAttacker;
    public string gotExecuteStateName { get; private set; }

    public enum ExecutedPhase
    {
        None,
        ResetingBone,
        Animate,
    }
    public ExecutedPhase executedPhase { get; set; }


    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }

    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject => this.gunFuExecute_OnGround_Single_ScriptableObject;

    private GunFuExecute_Single_ScriptableObject gunFuExecute_OnGround_Single_ScriptableObject ;
    private GunFuExecute_OnGround_Single_NodeLeaf _executerGunFuNodeLeaf;

    private float resetBoneTime 
        => ((_executerGunFuNodeLeaf._animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.warpingPhaseTimeNormalized)
                        - _executerGunFuNodeLeaf.lenghtOffset)*0.25f;

    private float warpingTime => _executerGunFuNodeLeaf._animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.warpingPhaseTimeNormalized;
    public GotExecuteOnGround_NodeLeaf(Enemy enemy,AnimationClip gotExecuteClip,Transform root,Transform hipsBone, Transform[] bones,string gotExecuteStateName, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this._root = root;
        this._hipsBone =hipsBone;
        this._bones = bones;
        this.gotExecuteStateName = gotExecuteStateName;

        this._ragdollBoneTransforms = new BoneTransform[_bones.Length];
        this._startAnimBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            this._ragdollBoneTransforms[i] = new BoneTransform();
            this._startAnimBoneTransforms[i] = new BoneTransform();
        }


        this._animationClip = gotExecuteClip;

    }
    public override bool Precondition()
    {
        if (_executerGunFu.curGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            && gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject.gotGunFuStateName == this.gotExecuteStateName)
        {
            gunFuExecute_OnGround_Single_ScriptableObject = gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject as GunFuExecute_Single_ScriptableObject;
            RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(
                _animationClip,
                enemy.gameObject,
                this._bones,
                _startAnimBoneTransforms,
                enemy.transform,
                _animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.opponentAnimationOffset);
        }

        return base.Precondition();
    }
    public override bool IsComplete()
    {
        if(this._timer >= duration)
            return true;

        return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if(enemy.isDead)
            return true;

        return false;
    }

    public override void Enter()
    {
        RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
        RagdollBoneBehavior.AlignPositionToHips(enemy._root, _hipsBone, enemy.transform, _startAnimBoneTransforms[0]);
        RagdollBoneBehavior.PopulateBoneTransforms(_bones, _ragdollBoneTransforms);

        _timer = 0;
        executedPhase = ExecutedPhase.ResetingBone;

        _gotExecutedGunFu._character._movementCompoent.CancleMomentum();

        _executerGunFuNodeLeaf = _executerGunFu.curGunFuNode as GunFuExecute_OnGround_Single_NodeLeaf;

        enemy.NotifyObserver(enemy, this); 

        base.Enter();
    }

    public override void Exit()
    {
        executedPhase = ExecutedPhase.None;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        switch (executedPhase)
        {
            case ExecutedPhase.ResetingBone:
                {
                    float elapsedPercentage = Mathf.Clamp01(
                        _timer 
                        / resetBoneTime);

                    RagdollBoneBehavior.LerpBoneTransforms(_bones,_ragdollBoneTransforms,_startAnimBoneTransforms,elapsedPercentage);

                  
                    if (_timer >= resetBoneTime)
                    {
                        enemy.NotifyObserver(enemy, this);
                        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
                        _animator.CrossFade(gotExecuteStateName, 0, 0, 0);
                        executedPhase = ExecutedPhase.Animate;
                    }
                }
                break;
                
            case ExecutedPhase.Animate:
                {

                }
                break ;
        }
        base.UpdateNode();
    }
}
 
