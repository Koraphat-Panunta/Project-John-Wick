using System.Collections.Generic;
using UnityEngine;

public class ResetingBoneMotionState : MotionState
{
    float _elapsedResetBonesTime = 0;
    float _timeToResetBones = 0.5f;

    private List<Transform> _bones = new List<Transform>();
    private MotionControlManager motionControlManager;

    private List<BoneTransform> _ragdollBoneTransforms;
    private List<BoneTransform> _standUpBoneTransforms;

    public bool IsReseting { get; private set; }
    public ResetingBoneMotionState(List<Transform> bones) 
    {
        _bones = bones;
        
    }    
    public override void Enter()
    {
        IsReseting = false;
        base.Enter();
    }
    public void Enter(MotionControlManager motionControlManager)
    {
        IsReseting = false;
       if (motionControlManager == null)
            this.motionControlManager = motionControlManager;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        this._ragdollBoneTransforms = this.motionControlManager.ragdollBoneTransform;
        this._standUpBoneTransforms = this.motionControlManager.animationBoneTransform;

        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

        for (int boneIndex = 0; boneIndex < _bones.Count; boneIndex++)
        {
            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].Position,
                _standUpBoneTransforms[boneIndex].Position,
                elapsedPercentage);

            _bones[boneIndex].localRotation = Quaternion.Lerp(
                _ragdollBoneTransforms[boneIndex].Rotation,
                _standUpBoneTransforms[boneIndex].Rotation,
                elapsedPercentage);
        }

        if (elapsedPercentage >= 1){
            motionControlManager.ChangeMotionState(motionControlManager.animationDrivenMotionState);
            IsReseting = true;
        }
        
        base.Update();
    }
}
