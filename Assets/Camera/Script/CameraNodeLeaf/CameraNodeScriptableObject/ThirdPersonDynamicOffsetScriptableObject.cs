using UnityEngine;
using System.Collections.Generic;
public class ThirdPersonDynamicOffsetScriptableObject : ScriptableObject
{
    [SerializeField] public List<Vector3> offsetKeyFrame;
    [SerializeField] public AnimationCurve animationCurve;
}
