using System;
using System.Linq;
using UnityEngine;

public class CameraThridPersonControllerDynamicTrackingNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{
    Transform[] trackTransform;
    float[] trackTransformWeight;
    Transform[] lookTransform;
    float[] lookTransformWeight;

    public override Vector3 targetTrackPos => CalculateAveragePosition.WeightedAverage(trackTransform.ToList<Transform>(), trackTransformWeight.ToList<float>());
    public override Vector3 targetLookPos => CalculateAveragePosition.WeightedAverage(lookTransform.ToList<Transform>(), lookTransformWeight.ToList<float>());

    public CameraThridPersonControllerDynamicTrackingNodeLeaf(
        CameraController cameraController,
        CameraThirdPersonControllerViewScriptableObject cameraThirdPersonViewScriptableObject,
        Func<bool> preCondition)
        : base(cameraController, cameraThirdPersonViewScriptableObject, preCondition)
    {
        this.trackTransform = new Transform[2];
        this.trackTransformWeight = new float[2];
        this.lookTransform = new Transform[2];
        this.lookTransformWeight = new float[2];
    }

    
   
    public void SetTrackTransform(Transform[] trackTransform, float[] trackWeight)
    {
        this.trackTransform = trackTransform;
        this.trackTransformWeight = trackWeight;
    }

    public void SetLookTransform(Transform[] lookTransform, float[] lookWeight)
    {
        this.lookTransform = lookTransform;
        this.lookTransformWeight = lookWeight;
    }
}
