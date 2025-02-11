using System;
using UnityEngine;

public class FallDown_EnemyState_NodeLeaf : EnemyStateLeafNode
{
    private float getUpTime = 3f;
    private float fallDownTime = 2f;
    private float timerGetDown;
    private float timerGetUp;
    private bool isFacingUp;

    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _standUpBoneTransforms;
    private BoneTransform[] _pushUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;
    private float _elapsedResetBonesTime;

    private Animator _animator;
    private float _timeToResetBones = 0.5f;

    private enum FallDownState
    {
        RagdollState,
        ResettingBones,
        StandingUp
    }

    private FallDownState curState;

    private AnimationClip standingUpClip;
    private AnimationClip pushingUpClip;

    public FallDown_EnemyState_NodeLeaf(Enemy enemy, IFallDownGetUpAble fallDownGetUpAble, Func<bool> preCondition) : base(enemy, preCondition)
    {
        standingUpClip = fallDownGetUpAble._standUpClip;
        pushingUpClip = fallDownGetUpAble._pushUpClip;
        _animator = fallDownGetUpAble._animator;

        _root = fallDownGetUpAble._root;
        _hipsBone = fallDownGetUpAble._hipsBone;
        _bones = fallDownGetUpAble._bones;

        _standUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];
        _pushUpBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _standUpBoneTransforms[i] = new BoneTransform();
            _ragdollBoneTransforms[i] = new BoneTransform();
            _pushUpBoneTransforms[i] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(standingUpClip, _standUpBoneTransforms);
        PopulateAnimationStartBoneTransforms(pushingUpClip, _pushUpBoneTransforms);
    }

    public override void Enter()
    {
        enemy.enemyMovement.CancleMomentum();

        if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.ragdollMotionState)
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
        isComplete = false;
        curState = FallDownState.RagdollState;

        _elapsedResetBonesTime = 0;

        timerGetUp = 0f;
        timerGetDown = 0f;

        enemy._posture = 0;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.FallDown);
    }

    public override void Exit()
    {
        enemy._painPart = IPainState.PainPart.None;
        enemy._posture = 100;
    }

   
    Vector3 beforeRootPos;
    public override void UpdateNode()
    {
        if(enemy._isPainTrigger)
        {
            PopulateBoneTransforms(_ragdollBoneTransforms);
            Enter();
        }
        switch (curState)
        {
            case FallDownState.RagdollState:
                timerGetDown += Time.deltaTime;

                if (timerGetDown >= fallDownTime)
                {
                    isFacingUp = IsFacingUp();

                    AlignRotationToHips();
                    AlignPositionToHips();
                    PopulateBoneTransforms(_ragdollBoneTransforms);

                    curState = FallDownState.ResettingBones;
                    _elapsedResetBonesTime = 0f;
                }
                break;

            case FallDownState.ResettingBones:
                _elapsedResetBonesTime += Time.deltaTime;
                float elapsedPercentage = Mathf.Clamp01(_elapsedResetBonesTime / _timeToResetBones);

                if (isFacingUp)

                    for (int i = 0; i < _bones.Length; i++)
                    {
                        _bones[i].localPosition = Vector3.Lerp(
                        _ragdollBoneTransforms[i].Position,
                        _standUpBoneTransforms[i].Position,
                        elapsedPercentage);

                        _bones[i].localRotation = Quaternion.Lerp(
                            _ragdollBoneTransforms[i].Rotation,
                            _standUpBoneTransforms[i].Rotation,
                            elapsedPercentage);
                    }
                else
                    for (int i = 0; i < _bones.Length; i++)
                    {
                        _bones[i].localPosition = Vector3.Lerp(
                            _ragdollBoneTransforms[i].Position,
                            _pushUpBoneTransforms[i].Position,
                            elapsedPercentage);

                        _bones[i].localRotation = Quaternion.Lerp(
                            _ragdollBoneTransforms[i].Rotation,
                            _pushUpBoneTransforms[i].Rotation,
                            elapsedPercentage);
                    }

                if (elapsedPercentage >= 1f)
                {
                    beforeRootPos = enemy.transform.position;
                    curState = FallDownState.StandingUp;
                    enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetUp);
                    enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);

                    if (isFacingUp)
                        _animator.CrossFade("StandUp", 0,0,0);
                    else
                        _animator.CrossFade("PushUp", 0,0,0);
                }
                break;

            case FallDownState.StandingUp:

                enemy.transform.position = beforeRootPos;
                timerGetUp += Time.deltaTime;
                if (timerGetUp >= getUpTime)
                    isComplete = true;
                break;
        }

    }

    private bool IsFacingUp()
    {
        return Vector3.Dot(_hipsBone.forward, Vector3.up) > 0;
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Vector3 hipOffset = enemy.transform.position - _root.transform.position;
        enemy.transform.position = _hipsBone.position + hipOffset;

        Vector3 positionOffset;

        if (isFacingUp) 
            positionOffset = _standUpBoneTransforms[0].Position;
        else
            positionOffset = _pushUpBoneTransforms[0].Position;

        
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
    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if(enemy.isDead)
            return true;

        return false;
    }

    private class BoneTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}


