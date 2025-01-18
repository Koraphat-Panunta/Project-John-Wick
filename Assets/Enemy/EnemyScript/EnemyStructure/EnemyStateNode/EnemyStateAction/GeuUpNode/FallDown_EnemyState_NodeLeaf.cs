using System;
using UnityEngine;

public class FallDown_EnemyState_NodeLeaf : EnemyStateLeafNode
{
    private bool isComplete;
    private float getUpTime = 3;
    private float getDownTime = 2;
    private float timerGetDown;
    private float timerGetUp;
    private bool isFacingUp;

    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _standUpBoneTransforms;
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

    private AnimationClip standingUpClip; // When Character FaceDown to the ground
    private AnimationClip pushingUpClip; // When Character FaceUp from the ground

    public FallDown_EnemyState_NodeLeaf(Enemy enemy,IFallDownGetUpAble fallDownGetUpAble ,Func<bool> preCondition) : base(enemy, preCondition)
    {
        standingUpClip = fallDownGetUpAble._standUpClip;
        pushingUpClip = fallDownGetUpAble._pushUpClip;
        _animator = fallDownGetUpAble._animator;

        _hipsBone = fallDownGetUpAble._hipsBone;
        _bones = fallDownGetUpAble._bones;

        _standUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _standUpBoneTransforms[i] = new BoneTransform();
            _ragdollBoneTransforms[i] = new BoneTransform();
        }
    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
        isComplete = false;
        curState = FallDownState.RagdollState;

        timerGetUp = 0;
        timerGetDown = 0;

        enemy.posture = 0;

    }

    public override void Exit()
    {
        enemy.posture = 100;
    }

    public override bool IsReset()
    {
        if(isComplete)
        { return true; }

        if(enemy.isDead)
        { return true; }

        return false;
    }

    public override void Update()
    {
        if (enemy._isPainTrigger)
        {
            Enter();
        }
        switch (curState)
        {
            case FallDownState.RagdollState:
                timerGetDown += Time.deltaTime;

                if (timerGetDown >= getDownTime)
                {
                    isFacingUp = IsFacingUp();
                    if (isFacingUp)
                        PopulateAnimationStartBoneTransforms(standingUpClip, _standUpBoneTransforms);
                    else
                        PopulateAnimationStartBoneTransforms(pushingUpClip, _standUpBoneTransforms);

                    AlignRotationToHips();
                    AlignPositionToHips();
                    PopulateBoneTransforms(_ragdollBoneTransforms);

                    curState = FallDownState.ResettingBones;
                    _elapsedResetBonesTime = 0;
                }
                break;

            case FallDownState.ResettingBones:
                _elapsedResetBonesTime += Time.deltaTime;
                float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

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

                if (elapsedPercentage >= 1)
                {
                    curState = FallDownState.StandingUp;
                    enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);

                    if (isFacingUp)
                        _animator.CrossFade("StandUp",0f,0);
                    else
                        _animator.CrossFade("PushUp", 0f, 0);
                }
                break;

            case FallDownState.StandingUp:
                timerGetUp += Time.deltaTime;
                if (timerGetUp >= getUpTime)
                    isComplete = true;
                break;
        }
    }

    public override void FixedUpdate()
    {
        // No fixed-update logic required here for this state
    }

    private bool IsFacingUp()
    {
        return Vector3.Dot(_hipsBone.up, Vector3.up) > 0; 
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        enemy.transform.position = _hipsBone.position;

        Vector3 positionOffset = _standUpBoneTransforms[0].Position;
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

    private class BoneTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}

