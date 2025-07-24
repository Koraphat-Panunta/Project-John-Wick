using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ThirdPersonDynamicOffsetScriptableObject", menuName = "ScriptableObjects/CameraScriptableObject/ThirdPersonDynamicOffsetScriptableObject")]
public class ThirdPersonDynamicOffsetScriptableObject : ScriptableObject
{
    [SerializeField] public List<float> offsetKeyFrameNormalizedTime;
    [SerializeField] public List<Vector3> offsetKeyFrame;
    [SerializeField] public AnimationCurve animationCurve;
}
