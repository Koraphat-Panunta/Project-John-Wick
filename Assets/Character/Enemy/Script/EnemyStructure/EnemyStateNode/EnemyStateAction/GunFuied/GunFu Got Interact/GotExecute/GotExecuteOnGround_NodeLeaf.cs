using System;
using UnityEngine;

public class GotExecuteOnGround_NodeLeaf : EnemyStateLeafNode,IGotGunFuExecuteNodeLeaf
{
    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _ragdollBoneTransforms;
    private BoneTransform[] _startAnimBoneTransforms;
    private float _elapsedResetBonesTime;

    private Animator _animator => enemy.animator;
    private float _timeToResetBones = 0.16f;

    private float elapsedTime;
    private float duration { get => 1 + _timeToResetBones; }

    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;

    public IGunFuAble _executerGunFu => _gotExecutedGunFu.gunFuAbleAttacker;
    private string gotExecuteStateName;

    public enum ExecutedPhase
    {
        None,
        ResetingBone,
        Animate,
    }
    public ExecutedPhase executedPhase { get; set; }


    public float _timer { get; set; }
    public AnimationClip _animationClip { get => gunFuExecute_OnGround_Single_ScriptableObject.gotExecuteClip; set { } }

    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject => this.gunFuExecute_OnGround_Single_ScriptableObject;

    private GunFuExecute_OnGround_Single_ScriptableObject gunFuExecute_OnGround_Single_ScriptableObject ;
    private bool isPopulateBone;
    public GotExecuteOnGround_NodeLeaf(Enemy enemy,Transform root,Transform hipsBone, Transform[] bones,string gotExecuteStateName, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this._root = root;
        this._hipsBone =hipsBone;
        this._bones = bones;
        this.gotExecuteStateName = gotExecuteStateName;

        this._ragdollBoneTransforms = new BoneTransform[_bones.Length];
        this._startAnimBoneTransforms = new BoneTransform[_bones.Length];

        this.isPopulateBone = false;

        for (int i = 0; i < _bones.Length; i++)
        {
            this._ragdollBoneTransforms[i] = new BoneTransform();
            this._startAnimBoneTransforms[i] = new BoneTransform();
        }
    }
    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;

        if (_executerGunFu.curGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            && gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject.gotGunFuStateName == this.gotExecuteStateName)
        {
            gunFuExecute_OnGround_Single_ScriptableObject = gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject as GunFuExecute_OnGround_Single_ScriptableObject;
            return true;
        }

        return false;
    }
    public override bool IsComplete()
    {
        if(elapsedTime >= duration)
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
        if(isPopulateBone == false)
        {
            RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(_animationClip,enemy.gameObject,this._bones,_startAnimBoneTransforms, enemy.transform);
            isPopulateBone = true;
        }

        elapsedTime = 0;
        _elapsedResetBonesTime = 0;
        executedPhase = ExecutedPhase.ResetingBone;

        RagdollBoneBehavior.AlignRotationToHips(_hipsBone,enemy.transform);
        RagdollBoneBehavior.AlignPositionToHips(enemy._root, _hipsBone, enemy.transform, _startAnimBoneTransforms[0]);
        RagdollBoneBehavior.PopulateBoneTransforms(_bones,_ragdollBoneTransforms);

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

    private Vector3 beforeRootPos;
    public override void UpdateNode()
    {
        switch (executedPhase)
        {
            case ExecutedPhase.ResetingBone:
                {
                    _elapsedResetBonesTime += Time.deltaTime;
                    float elapsedPercentage = Mathf.Clamp01(_elapsedResetBonesTime / _timeToResetBones);

                    RagdollBoneBehavior.LerpBoneTransforms(_bones,_ragdollBoneTransforms,_startAnimBoneTransforms,elapsedPercentage);

                  
                    if (_elapsedResetBonesTime >= _timeToResetBones)
                    {
                        beforeRootPos = enemy.transform.position;
                        enemy.NotifyObserver(enemy, this);
                        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
                        _animator.CrossFade(gotExecuteStateName, 0, 0, 0);
                        executedPhase = ExecutedPhase.Animate;
                    }
                }
                break;
                
            case ExecutedPhase.Animate:
                {
                    enemy.transform.position = beforeRootPos;
                }
                break ;
        }

        elapsedTime += Time.deltaTime;
        base.UpdateNode();
    }
}
 
