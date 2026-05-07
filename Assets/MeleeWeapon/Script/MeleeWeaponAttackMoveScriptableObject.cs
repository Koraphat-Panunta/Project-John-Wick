using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponAttackMoveScriptableObject",menuName = "ScriptableObjects/MeleeWeapon/AttackMoveScriptableObject")]
public class MeleeWeaponAttackMoveScriptableObject : ScriptableObject
{
    [Range(0, 10)]
    public float _beginAttackDistance;
    [Range(0, 10)]
    public float _attackRange;
    [SerializeField] public AnimationTriggerEventSCRP animationTriggerEventSCRP;
}
