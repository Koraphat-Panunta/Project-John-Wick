using System;
using UnityEngine;

public class GotExecuteOnGround_GotInteract_NodeLeaf : GunFu_GotInteract_NodeLeaf
{
    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _ragdollBoneTransforms;
    private BoneTransform[] _layUpBoneTransforms;
    private BoneTransform[] _layDownBoneTransforms;
    private float _elapsedResetBonesTime;

    private Animator _animator;
    private float _timeToResetBones = 0.16f;

    private float elapsedTime;
    private float duration { get => 1 + _timeToResetBones; }

    public bool isFacingUp { get =>   Vector3.Dot(_hipsBone.forward, Vector3.up) > 0;}

    public enum ExecutedPhase
    {
        None,
        ResetingBone,
        Animate,
    }
    public ExecutedPhase executedPhase { get; set; }

    public GotExecuteOnGround_GotInteract_NodeLeaf(Enemy enemy,AnimationClip layUp,AnimationClip layDown, Func<bool> preCondition) : base(enemy, preCondition)
    {
        _animator = enemy.animator;

        _root = enemy._root;
        _hipsBone = enemy._hipsBone;
        _bones = enemy._bones;

        _ragdollBoneTransforms = new BoneTransform[_bones.Length];
        _layUpBoneTransforms = new BoneTransform[_bones.Length];
        _layDownBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _ragdollBoneTransforms[i] = new BoneTransform();
            _layUpBoneTransforms[i] = new BoneTransform();
            _layDownBoneTransforms[i] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(layUp, _layUpBoneTransforms);
        PopulateAnimationStartBoneTransforms(layDown, _layDownBoneTransforms);
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
        elapsedTime = 0;
        _elapsedResetBonesTime = 0;
        executedPhase = ExecutedPhase.ResetingBone;

        AlignRotationToHips();
        AlignPositionToHips();
        PopulateBoneTransforms(_ragdollBoneTransforms);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract); 

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

                    if (isFacingUp)

                        for (int i = 0; i < _bones.Length; i++)
                        {
                            _bones[i].localPosition = Vector3.Lerp(
                            _ragdollBoneTransforms[i].Position,
                            _layUpBoneTransforms[i].Position,
                            elapsedPercentage);

                            _bones[i].localRotation = Quaternion.Lerp(
                                _ragdollBoneTransforms[i].Rotation,
                                _layUpBoneTransforms[i].Rotation,
                                elapsedPercentage);
                        }
                    else
                        for (int i = 0; i < _bones.Length; i++)
                        {
                            _bones[i].localPosition = Vector3.Lerp(
                                _ragdollBoneTransforms[i].Position,
                                _layDownBoneTransforms[i].Position,
                                elapsedPercentage);

                            _bones[i].localRotation = Quaternion.Lerp(
                                _ragdollBoneTransforms[i].Rotation,
                                _layDownBoneTransforms[i].Rotation,
                                elapsedPercentage);
                        }
                    if (_elapsedResetBonesTime >= _timeToResetBones)
                    {
                        beforeRootPos = enemy.transform.position;
                        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetUp);
                        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);

                        if (isFacingUp)
                            _animator.CrossFade("Enemy_LayUp_Executed", 0, 0, 0);
                        else
                            _animator.CrossFade("Enemy_LayDown_Executed", 0, 0, 0);

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

        if (isFacingUp)
            positionOffset = _layUpBoneTransforms[0].Position;//HipsBonePosition
        else
            positionOffset = _layDownBoneTransforms[0].Position;//HipsBonePosition


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

        if (isFacingUp)
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
 
