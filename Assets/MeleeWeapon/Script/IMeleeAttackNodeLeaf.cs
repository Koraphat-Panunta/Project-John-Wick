using UnityEngine;

public interface IMeleeAttackNodeLeaf : INodeLeaf
{
    protected IMeleeWeaponUserAble _MeleeWeaponUserAble { get; }
    public MeleeAttackingPhase _attackingPhase { get;  }
}
