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


    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;

    public IGunFuAble _executerGunFu => _gotExecutedGunFu.gunFuAbleAttacker;

    GotExecutedStateName IGotGunFuExecuteNodeLeaf._gotExecutedStateName => this.gotExecutedStateName;
    private GotExecutedStateName gotExecutedStateName;
    public string gotExecuteStateName { get => this.gotExecuteStateName.ToString(); }

    public enum ExecutedPhase
    {
        None,
        PoppulateStartBoneTransform,
        ResetingBone,
        Animate,
    }
    public ExecutedPhase executedPhase { get; set; }


    protected AnimationTriggerEventSCRP gunFuExecuteSInteractSCRP;
    public AnimationTriggerEventPlayer animationTriggerEventPlayer { get; protected set; }
    protected AnimationClip animationClip { get => this.gunFuExecuteSInteractSCRP.clip; }
    protected float opponentAnimationOffset { get => gunFuExecuteSInteractSCRP.enterNormalizedTime; }

    public float resetBoneTimer { get; protected set; }

    public float reserBoneDuration = .5f;

    public GotExecuteOnGround_NodeLeaf(
        Enemy enemy,AnimationTriggerEventSCRP gunFuExecuteSInteractSCRP, Transform root,Transform hipsBone, Transform[] bones,GotExecutedStateName gotExecuteStateName, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this._root = root;
        this._hipsBone =hipsBone;
        this._bones = bones;
        this.gotExecutedStateName = gotExecuteStateName;

        this._ragdollBoneTransforms = new BoneTransform[_bones.Length];
        this._startAnimBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            this._ragdollBoneTransforms[i] = new BoneTransform();
            this._startAnimBoneTransforms[i] = new BoneTransform();
        }


        this.gunFuExecuteSInteractSCRP = gunFuExecuteSInteractSCRP;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(this.gunFuExecuteSInteractSCRP);

    }
  
    public override bool IsComplete()
    {
        if(animationTriggerEventPlayer.IsPlayFinish())
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
        this.animationTriggerEventPlayer.Rewind();
    
        resetBoneTimer = 0;
   
        _gotExecutedGunFu._character._movementCompoent.CancleMomentum();

        executedPhase = ExecutedPhase.None;
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
        switch (this.executedPhase)
        {
            case ExecutedPhase.None:
                {
                    RagdollBoneBehavior.PopulateAnimationStartBoneTransforms(
                        animationClip,
                        enemy.gameObject,
                        this._bones,
                        _startAnimBoneTransforms,
                        enemy.transform,
                        animationClip.length * this.opponentAnimationOffset
                        );

                    executedPhase = ExecutedPhase.PoppulateStartBoneTransform;
                    break;
                }
            case ExecutedPhase.PoppulateStartBoneTransform:
                {
                    RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
                    RagdollBoneBehavior.AlignPositionToHips(enemy._root, _hipsBone, enemy.transform, _startAnimBoneTransforms[0]);
                    RagdollBoneBehavior.PopulateBoneTransforms(_bones, _ragdollBoneTransforms);
                    executedPhase = ExecutedPhase.ResetingBone;
                    break;
                }
            case ExecutedPhase.ResetingBone:
                {
                    this.resetBoneTimer = Mathf.Clamp01(this.resetBoneTimer + Time.deltaTime);
                    RagdollBoneBehavior.LerpBoneTransforms(_bones, _ragdollBoneTransforms, _startAnimBoneTransforms, resetBoneTimer/reserBoneDuration);
                    if (resetBoneTimer >= reserBoneDuration)
                    {
                        executedPhase = ExecutedPhase.Animate;
                        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
                        _animator.CrossFade(gotExecuteStateName, 0, 0, 0);
                        enemy.NotifyObserver(enemy, this);
                    }
                    break;
                }
            case ExecutedPhase.Animate:
                {
                    this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
                    break;
                }
        }

        base.UpdateNode();
    }
}
 
