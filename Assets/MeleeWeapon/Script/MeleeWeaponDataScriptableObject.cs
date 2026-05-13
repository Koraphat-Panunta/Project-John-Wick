using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponSCRP", menuName = "ScriptableObjects/MeleeWeapon/WeaponData")]
public class MeleeWeaponDataScriptableObject : WeaponDataScriptableObject
{
    [SerializeField] public MeleeWeapon meleeWeaponPrefab;

    [Range(0, 250)]
    public float damage;

    public override Weapon _weaponPrefab => this.meleeWeaponPrefab;
}
