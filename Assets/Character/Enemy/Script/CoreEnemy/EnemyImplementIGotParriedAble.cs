using UnityEngine;

public partial class Enemy : IGotParriedAble
{
    public bool _triggerGotParried { get; set; }
    public IDefendMeleeAttackAble _parrier { get; set; }

    [SerializeField] public AnimationTriggerEventSCRP gotParriedScriptableObject;

    public void OnParried(IDefendMeleeAttackAble parrier)
    {
        this._parrier = parrier;
        this._triggerGotParried = true;
    }
}
