using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CameraThirdPersonDynamicOffsetNodeLeaf : CameraNodeLeaf
{
    public ThirdPersonDynamicOffsetScriptableObject thirdPersonDynamicOffsetScriptableObject { get; protected set; }
    public Vector3 targetOffset {get; protected set; }
    public float transitionSpeedIn { get; protected set; }
    public float duration { get; protected set; }
    protected float transitionInNormalized;
    protected float _timer;
    protected AnimationCurve animationCurve => thirdPersonDynamicOffsetScriptableObject.animationCurve;
    protected List<Vector3> offsetKeyFrames => thirdPersonDynamicOffsetScriptableObject.offsetKeyFrame;

    protected ThirdPersonCinemachineCamera thirdPersonCinemachineCamera => cameraController.thirdPersonCinemachineCamera;
    public CameraThirdPersonDynamicOffsetNodeLeaf(
        CameraController cameraController
        ,ThirdPersonDynamicOffsetScriptableObject thirdPersonDynamicOffsetScriptableObject
        ,float transitionInSpeed
        ,float duration
        , Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.transitionSpeedIn = transitionInSpeed;
        this.duration = duration;
        this.thirdPersonDynamicOffsetScriptableObject = thirdPersonDynamicOffsetScriptableObject;
    }
    public override void Enter()
    {
        _timer = 0;
        transitionInNormalized = 0;
        this.TransitionIn();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override bool IsComplete()
    {
        if(_timer > duration)
            return true;

        return base.IsComplete();
    }
    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        this.UpdateTargetOffset(out Vector3 targetOffset);
        
        float offsetX = Vector3.Lerp(,,transitionInNormalized);
        float offsetY;
        float offsetZ;

        base.UpdateNode();
    }
    private async void TransitionIn()
    {
        while (transitionInNormalized < 0)
        {
            transitionInNormalized += transitionSpeedIn * Time.deltaTime;
            await Task.Yield();
            transitionInNormalized = 1;
        }
    }
    private void UpdateTargetOffset(out Vector3 targetOffset)
    {
        targetOffset = this.targetOffset;
        targetOffset = BezierurveMove.GetPointOnBezierCurve(this.offsetKeyFrames, _timer / duration);
    }
}
