using UnityEngine;

public partial class Player : IDefendMeleeAttackAble
{
    public Character _character => this;

    public Transform _defenderTransform { get => this.transform; }

    [SerializeField] private MeleeAttack_Defensive_DetectAttacker defensiveDetector;
    public MeleeAttack_Defensive_DetectAttacker _defensiveDetector { get => this.defensiveDetector; }

    public bool _isDefendingAble 
    {
        get 
        {
            if(this.stateNodeManager.GetCurNodeLeaf() is PlayerStandMoveNodeLeaf
                || this.stateNodeManager.GetCurNodeLeaf() is PlayerStandIdleNodeLeaf)
                return true;

            return false;
        }
    }

    public bool isInComingMeleeAttack;

    public IMeleeAttackerAble meleeAttackerAble;

    public void OnIncomingMeleeAttack(IMeleeAttackerAble attacker)
    {
        Debug.Log("InComingAttack");
    }

    public void OnIncomingMeleeAttackEnded(IMeleeAttackerAble attacker)
    {

    }
}
