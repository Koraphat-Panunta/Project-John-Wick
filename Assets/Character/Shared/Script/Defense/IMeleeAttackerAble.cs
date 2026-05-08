using UnityEngine;

public interface IMeleeAttackerAble
{
    Character _character { get; }
    Transform _attackerTransform { get; }
    Vector3 _attackAimDir { get; }
    MeleeAttackingPhase _curAttackPhase { get; }
}
