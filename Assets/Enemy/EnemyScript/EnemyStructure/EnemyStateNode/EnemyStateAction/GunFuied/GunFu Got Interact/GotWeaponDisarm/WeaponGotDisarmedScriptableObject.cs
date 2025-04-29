using UnityEngine;

[CreateAssetMenu(fileName = "WeaponGotDisarmedScriptableObject", menuName = "ScriptableObjects/GunFuInteract/WeaponGotDisarmedScriptableObject")]
public class WeaponGotDisarmedScriptableObject : ScriptableObject
{
    public string StateName;
    public AnimationClip animationClip;
    [Range(0,1)]
    public float enterNormalized;
    [Range(0,1)]
    public float exitNormalized;
}
