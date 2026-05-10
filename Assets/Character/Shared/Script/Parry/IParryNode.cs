using UnityEngine;

public interface IParryNode : INodeLeaf
{
    string _stateName { get; }
    IDefendMeleeAttackAble _parrier { get; }
    IMeleeAttackerAble _parriedAttacker { get; }
}
