using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponAttackMoveScriptableObject",menuName = "ScriptableObjects/MeleeWeapon/AttackMoveScriptableObject")]
public class MeleeWeaponAttackMoveScriptableObject : ScriptableObject
{
    
    [Range(0, 10)]
    public float _beginAttackDistance;
    [Range(0, 10)]
    public float _attackMove_Range;
    [Range(0, 10)]
    public float _maxAttackMove_Range;


    [SerializeField]
    public AnimationCurve _moveVelocityCurve;
    [SerializeField]
    public float _topVelocityMove;

    [Range(0, 500)]
    public float _rotateVelocity;

    public string attackMoveName;
    [SerializeField] public AnimationTriggerEventSCRP animationTriggerEventSCRP;
}
