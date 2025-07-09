using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "RestrictScriptableObject",menuName = "ScriptableObjects/GunFuObject/RestrictScriptableObject")]
public class RestrictScriptableObject : ScriptableObject
{
    public Vector3 offset;
    public Vector3 rotationOffset;

    public string stateName;

    [Range(0,1)]
    public float restrictEnter_exitNormalized;

    [Range(0,1)]
    public float restrictExit_exitNormalized;
    [Range(0,1)]
    public float restrictExit_hitNormalized;
    [Range(0, 100)]
    public float restrictExit_HitForce;

    [Range(0, 50)]
    public float StayDuration;

    [Range(0, 10)]
    public float restrictEnterSnapDistanceTarget;

    public AnimationClip restrictEnterClip;
    public AnimationClip restrictExitClip;
}
