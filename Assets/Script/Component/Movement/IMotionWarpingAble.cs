using UnityEngine;

public interface IMotionWarpingAble 
{
   public IMovementMotionWarping movementMotionWarping { get; set; }
    public void StartWarping(Vector3 start,
        Vector3 cT1,
        Vector3 cT2,
        Vector3 exit,
        float duration,
        AnimationCurve animationCurve,IMovementCompoent movementCompoent);
}
