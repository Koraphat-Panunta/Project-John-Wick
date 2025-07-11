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

    public enum FallDownState
    {
        RagdollState,
        ResettingBones,
        StandingUp
    }

    public FallDownState curState { get; private set; }

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

        RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(standingUpClip, enemy.gameObject, _bones, _standUpBoneTransforms, enemy.transform);
        RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(pushingUpClip, enemy.gameObject, _bones, _pushUpBoneTransforms, enemy.transform);

    }

    public override void Enter()
    {
        (enemy._movementCompoent as MovementCompoent).CancleMomentum();

        if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.ragdollMotionState)
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
        isComplete = false;
        curState = FallDownState.RagdollState;

        _elapsedResetBonesTime = 0;

        timerGetUp = 0f;
        timerGetDown = 0f;

        enemy._posture = 0;

        enemy.NotifyObserver(enemy,this);
        enemy.NotifyObserver(enemy,this);
    }

    public override void Exit()
    {
        enemy._painPart = IPainStateAble.PainPart.None;
        enemy._posture = 100;
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }


    Vector3 beforeRootPos;
    public override void UpdateNode()
    {
        if(enemy._isPainTrigger || enemy._tiggerThrowAbleObjectHit)
        {
            RagdollBoneBehavior.PopulateBoneTransforms(_bones, _ragdollBoneTransforms);
            (enemy._movementCompoent as MovementCompoent).CancleMomentum();

            if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.ragdollMotionState)
                enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
            isComplete = false;
            curState = FallDownState.RagdollState;

            _elapsedResetBonesTime = 0;

            timerGetUp = 0f;
            timerGetDown = 0f;

            enemy._posture = 0;

            enemy.NotifyObserver(enemy, this);
            enemy.NotifyObserver(enemy, this);
        }
        switch (curState)
        {
            case FallDownState.RagdollState:
                timerGetDown += Time.deltaTime;
                RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
                RagdollBoneBehavior.AlignPositionToHips(_root, _hipsBone, enemy.transform, _ragdollBoneTransforms[0]);
                if (timerGetDown >= fallDownTime)
                {
                    isFacingUp = IsFacingUp();

                    RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
                    RagdollBoneBehavior.AlignPositionToHips(_root, _hipsBone, enemy.transform, _ragdollBoneTransforms[0]);
                    RagdollBoneBehavior.PopulateBoneTransforms(_bones, _ragdollBoneTransforms);

                    curState = FallDownState.ResettingBones;
                    _elapsedResetBonesTime = 0f;
                }
                break;

            case FallDownState.ResettingBones:
                _elapsedResetBonesTime += Time.deltaTime;
                float elapsedPercentage = Mathf.Clamp01(_elapsedResetBonesTime / _timeToResetBones);

                if (isFacingUp)
                    RagdollBoneBehavior.LerpBoneTransforms(_bones, _ragdollBoneTransforms, _standUpBoneTransforms,elapsedPercentage);
                else
                    RagdollBoneBehavior.LerpBoneTransforms(_bones, _ragdollBoneTransforms, _pushUpBoneTransforms, elapsedPercentage);

                if (elapsedPercentage >= 1f)
                {
                    beforeRootPos = enemy.transform.position;
                    curState = FallDownState.StandingUp;
                    enemy.NotifyObserver(enemy, this);
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
    public override bool IsReset()
    {
        if(enemy._triggerHitedGunFu && enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf)
            return true;

        if(IsComplete())
            return true;

        if(enemy.isDead)
            return true;

        return false;
    }

   
}


