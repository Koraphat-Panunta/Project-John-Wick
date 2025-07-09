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

    public GunFuExecute_Single_ScriptableObject _gunFuExecute_Single_ScriptableObject => this.gunFuExecute_Single_ScriptableObject;

    public float _timer { get; set; }
    public AnimationClip _animationClip { get => _gunFuExecute_Single_ScriptableObject.gotExecuteClip; set { } }

    private GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject ;
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
            && gunFuExecuteNodeLeaf._gunFuExecute_Single_ScriptableObject.gotGunFuStateName == this.gotExecuteStateName)
        {
            gunFuExecute_Single_ScriptableObject = gunFuExecuteNodeLeaf._gunFuExecute_Single_ScriptableObject;
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
            PopulateAnimationStartBoneTransforms(_animationClip, _startAnimBoneTransforms);
            isPopulateBone = true;
        }

        elapsedTime = 0;
        _elapsedResetBonesTime = 0;
        executedPhase = ExecutedPhase.ResetingBone;

        AlignRotationToHips();
        AlignPositionToHips();
        PopulateBoneTransforms(_ragdollBoneTransforms);

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

                    for (int i = 0; i < _bones.Length; i++)
                    {
                        _bones[i].localPosition = Vector3.Lerp(
                        _ragdollBoneTransforms[i].Position,
                        _startAnimBoneTransforms[i].Position,
                        elapsedPercentage);

                        _bones[i].localRotation = Quaternion.Lerp(
                            _ragdollBoneTransforms[i].Rotation,
                            _startAnimBoneTransforms[i].Rotation,
                            elapsedPercentage);
                    }

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

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Vector3 hipOffset = enemy.transform.position - _root.transform.position;
        enemy.transform.position = _hipsBone.position + hipOffset;

        Vector3 positionOffset;
    
        positionOffset = _startAnimBoneTransforms[0].Position;//HipsBonePosition
  


        positionOffset.y = 0;
        positionOffset = enemy.transform.rotation * positionOffset;

        enemy.transform.position -= positionOffset;

        if (Physics.Raycast(enemy.transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, hitInfo.point.y, enemy.transform.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up;

        if (Vector3.Dot(_hipsBone.forward, Vector3.up) > 0)
        {
            desiredDirection *= -1;
        }

        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(enemy.transform.forward, desiredDirection);

        enemy.transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }
    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {

        for (int i = 0; i < _bones.Length; i++)
        {
            boneTransforms[i].Position = _bones[i].localPosition;
            boneTransforms[i].Rotation = _bones[i].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(AnimationClip clip, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = enemy.transform.position;
        Quaternion rotationBeforeSampling = enemy.transform.rotation;

        clip.SampleAnimation(enemy.gameObject, 0);
        PopulateBoneTransforms(boneTransforms);

        enemy.transform.position = positionBeforeSampling;
        enemy.transform.rotation = rotationBeforeSampling;
    }
}
 
