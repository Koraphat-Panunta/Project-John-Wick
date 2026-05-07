using UnityEngine;

public interface IMeleeWeaponUserAble 
{
    public IMeleeAttackNodeLeaf _curMeleeNodeLeaf { get; }
    public MeleeAttackingPhase _curMeleeAttackPhase { get; }
    public MeleeWeapon _curMeleeWeapon { get; }
    public Transform _meleeWeaponUserTransform { get; }
    public Transform _targetTransform { get; }
    public bool _isPerformAttackAble { get; }

    public void OnNotifyMeleeAttack<T>(T var);
    
}
