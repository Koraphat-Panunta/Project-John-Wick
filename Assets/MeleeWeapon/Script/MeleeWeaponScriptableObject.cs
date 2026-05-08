using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponSCRP",menuName = "Weapon/MeleeWeaponScriptableObject")]
public class MeleeWeaponScriptableObject : ScriptableObject
{
    [Range(0,250)]
    public float damage;
}
