using System.Collections;
using UnityEngine;

public interface IMovementMotionWarping 
{
    public bool isWarping { get; set; }
    public Coroutine motionWarping { get; set; }
    public void StartMotionWarpingCurve(Vector3 start,
        Vector3 cT1,
        Vector3 cT2,
        Vector3 exit,
        float duration,
        AnimationCurve animationCurve);
    public IEnumerator MotionWarpingCurve(
        Vector3 start,
        Vector3 cT1,
        Vector3 cT2,
        Vector3 exit,
        float duration,
        AnimationCurve animationCurve);
}
