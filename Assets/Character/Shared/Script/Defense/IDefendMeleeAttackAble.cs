using UnityEngine;

public interface IDefendMeleeAttackAble
{
    Character _character { get; }
    Transform _defenderTransform { get; }
    MeleeAttack_Defensive_DetectAttacker _defensiveDetector { get; }
    bool _isDefendingAble { get; }

    void OnIncomingMeleeAttack(IMeleeAttackerAble attacker);
    void OnIncomingMeleeAttackEnded(IMeleeAttackerAble attacker);
}
