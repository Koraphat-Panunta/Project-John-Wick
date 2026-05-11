using UnityEngine;

public interface IMeleeWeaponUserAble : IMeleeAttackerAble
{
    public IMeleeAttackNodeLeaf _curMeleeNodeLeaf { get; }
    public MeleeAttackingPhase _curMeleeAttackPhase { get; }
    public IGrabMeleeWeaponAble _grabMeleeWeaponAble { get; }
    public MeleeWeapon _curMeleeWeapon { get; }
    public Transform _meleeWeaponUserTransform { get; }
    public Transform _targetTransform { get; }
    public bool _isPerformAttackAble { get; }
    public float _attackRange { get; }
    public void OnNotifyMeleeAttack<T>(T var);
    
}
