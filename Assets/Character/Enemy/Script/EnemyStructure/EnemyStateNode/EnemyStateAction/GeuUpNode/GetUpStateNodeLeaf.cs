using System;
using UnityEngine;
using static EnemyStateLeafNode;
using static FallDown_EnemyState_NodeLeaf;

public class GetUpStateNodeLeaf : EnemyStateLeafNode,IRagdollTransitionAnimatorAbleStateNodeLeaf
{
    private IRagdollAble ragdollAble;

    public float resetBoneTimer;
    public float resetBoneDuration = 0.5f;

    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _getUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;

    private Animator _animator;

    private string getUpAnimatorStateName;
    public IRagdollTransitionAnimatorAbleStateNodeLeaf.RagdollTransitionAnimatorState curRagdollAnimatorState { get; set; }

    private Vector3 beforeRootPos;

    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    public AnimationTriggerEventPlayer animationTriggerEventPlayer { get;protected set; }

    public bool isStandingComplete;

    public GetUpStateNodeLeaf(
        Enemy enemy
        , Func<bool> preCondition
        ,IRagdollAble ragdollAble
        , AnimationTriggerEventSCRP animationTriggerEventSCRP
        , string getUpAnimatorStateName
        ) : base(enemy, preCondition)
    {
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.getUpAnimatorStateName = getUpAnimatorStateName;
        this.ragdollAble = ragdollAble;
        this._animator = ragdollAble._animator;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(this.animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent("Standing", this.Standing);

        _root = ragdollAble._root;
        _hipsBone = ragdollAble._hipsBone;
        _bones = ragdollAble._bones;

        _getUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _getUpBoneTransforms[i] = new BoneTransform();
            _ragdollBoneTransforms[i] = new BoneTransform();
        }

        RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(animationTriggerEventPlayer.animationClip, enemy.gameObject, _bones, _getUpBoneTransforms, enemy.transform, 0);

    }
    public override void Enter()
    {
        Debug.Log("Getup state nodeleaf enter");

        this.isStandingComplete = false;
        this.animationTriggerEventPlayer.Rewind();
        curRagdollAnimatorState = IRagdollTransitionAnimatorAbleStateNodeLeaf.RagdollTransitionAnimatorState.ResetingBone;
        this.resetBoneTimer = 0;
        RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
        RagdollBoneBehavior.AlignPositionToHips(_root, _hipsBone, enemy.transform, _ragdollBoneTransforms[0]);
        RagdollBoneBehavior.PopulateBoneTransforms(_bones, _ragdollBoneTransforms);

        base.Enter();
    }

    public override void UpdateNode()
    {
        switch (curRagdollAnimatorState)
        {
            case IRagdollTransitionAnimatorAbleStateNodeLeaf.RagdollTransitionAnimatorState.ResetingBone:
                {
                    this.resetBoneTimer += Time.deltaTime;
                    float elapsedPercentage = Mathf.Clamp01(this.resetBoneTimer / this.resetBoneDuration);

                    RagdollBoneBehavior.LerpBoneTransforms(_bones, _ragdollBoneTransforms, _getUpBoneTransforms, elapsedPercentage);

                    if (elapsedPercentage >= 1f)
                    {
                        beforeRootPos = enemy.transform.position;
                        curRagdollAnimatorState = IRagdollTransitionAnimatorAbleStateNodeLeaf.RagdollTransitionAnimatorState.PlayAnimation;

                        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);

                        _animator.CrossFade(this.getUpAnimatorStateName, 0, 0, 0);
                        enemy.transform.position = beforeRootPos;

                        enemy.NotifyObserver(enemy, this);
                    }
                    break;
                }
            case IRagdollTransitionAnimatorAbleStateNodeLeaf.RagdollTransitionAnimatorState.PlayAnimation:
                {
                    animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
                    break;
                }
        }
        base.UpdateNode();
    }
    private void Standing()
    {
        isStandingComplete = true;
        enemy.NotifyObserver(enemy, this);
    }
    public override bool IsComplete()
    {
        return animationTriggerEventPlayer.IsPlayFinish();
    }

    public override bool IsReset()
    {
        if (IsComplete())
        {
            Debug.Log("GetUpStateNodeLeaf IsComplete()");
            return true;
        }

        if(enemy.isDead)
            return true;

        if (enemy._isPainTrigger)
        {
            Debug.Log("GetUpStateNodeLeaf _isPainTrigger");
            return true;
        }

        if (enemy._triggerHitedGunFu)
        {
            return true;
        }

        return false;
    }

}
