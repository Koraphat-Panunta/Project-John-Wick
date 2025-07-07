using UnityEngine;

[CreateAssetMenu(fileName = "GotGunFuHitScrp", menuName = "ScriptableObjects/GotGunFu/GotGunFuHit")]
public class GotGunFuHitScriptableObject : ScriptableObject
{
    public AnimationClip AnimationClip;
    [Range(0, 1)]
    public float enterAnimationOffsetNormalized;
    [Range(0, 1)]
    public float exitTimeNormalized;
    public string gotHitStateName;
}
