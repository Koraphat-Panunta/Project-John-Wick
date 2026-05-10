using UnityEngine;

public interface IGotParriedAble
{
    Character _character { get; }
    bool _triggerGotParried { get; set; }
    IDefendMeleeAttackAble _parrier { get; set; }
    void OnParried(IDefendMeleeAttackAble parrier);
}
