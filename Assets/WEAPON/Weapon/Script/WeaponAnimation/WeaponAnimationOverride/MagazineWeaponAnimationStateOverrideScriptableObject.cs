using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[CreateAssetMenu(fileName = "MagazineWeaponAnimationStateOverrideScriptableObject", menuName = "ScriptableObjects" +
    "/Weapon" +
    "/WeaponAnimationOverride" +
    "/MagazineWeaponAnimationStateOverrideScriptableObject")]
public class MagazineWeaponAnimationStateOverrideScriptableObject : WeaponAnimationStateOverrideScriptableObject
{
    [SerializeField] public AnimationClip Reload;
    [SerializeField] public AnimationClip TacticalReload;
}
