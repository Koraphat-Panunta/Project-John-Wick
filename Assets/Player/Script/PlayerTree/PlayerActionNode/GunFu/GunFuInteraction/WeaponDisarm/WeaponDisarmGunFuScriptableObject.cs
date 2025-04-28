using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDisarmGunFuScriptableObject", menuName = "ScriptableObjects/GunFuInteract/WeaponDisarmGunFuScriptableObject")]
public class WeaponDisarmGunFuScriptableObject : ScriptableObject
{
   public AnimationClip animationClip;

    [Range(0, 1)]
    public float pullTimeNormalized;

    [Range(0, 1)]
    public float disarmTimeNormalized;

    [Range(0, 1)]
    public float transitionAbleTimeNormalized;

    [Range(0, 1)]
    public float exitTimeNormalized;
}
