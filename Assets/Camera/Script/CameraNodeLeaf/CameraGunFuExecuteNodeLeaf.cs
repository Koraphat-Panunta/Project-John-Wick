using System;
using System.Linq;
using UnityEngine;

public class CameraGunFuExecuteNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{

    Transform[] trackTransform;
    float[] trackTransformWeight;
    Transform[] lookTransform;
    float[] lookTransformWeight;

    float trackRate;
    protected bool isRecovery;
    public CameraGunFuExecuteNodeLeaf(
        CameraController cameraController
        , CameraThirdPersonControllerViewScriptableObject cameraThirdPersonViewScriptableObject
        , Func<bool> preCondition
        ) : base(cameraController,cameraThirdPersonViewScriptableObject, preCondition)
    {
        this.trackTransform = new Transform[2];
        this.trackTransformWeight = new float[2];
        this.lookTransform = new Transform[2];
        this.lookTransformWeight = new float[2];

    }

    public override void Enter()
    {
        this.isRecovery = false;
        base.Enter();
    }
    public override void UpdateNode()
    {

        if (isRecovery)
            this.trackRate = Mathf.Clamp01(this.trackRate - Time.deltaTime);
        else
            this.trackRate = Mathf.Clamp01(this.trackRate + Time.deltaTime);

        base.UpdateNode();
    }
    protected override void UpdateCameraPosition()
    {
        Vector3 trackPos = CalculateAveragePosition.WeightedAverage(trackTransform.ToList<Transform>(), trackTransformWeight.ToList<float>());
        Vector3 lookPos = CalculateAveragePosition.WeightedAverage(lookTransform.ToList<Transform>(), lookTransformWeight.ToList<float>());

        trackPos = Vector3.Lerp(thirdPersonCamera.transform.position,trackPos,this.trackRate);
        lookPos = Vector3.Lerp(thirdPersonCamera.targetLookTarget.position, trackPos, this.trackRate);
        thirdPersonCamera.UpdateCameraPosition(trackPos, lookPos);
    }
}
