using System;
using UnityEngine;

public class HumanShieldAnimationConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    private HumandShieldRightHandConstrainLookAtManager humandShieldRightHandConstrainLookAtManager;
    private HumanShieldRightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject;
    public HumanShieldAnimationConstraintNodeLeaf( HumandShieldRightHandConstrainLookAtManager humandShieldRightHandConstrainLookAtManager
        ,HumanShieldRightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject
        ,Func<bool> precondition) : base(precondition)
    {
        this.humandShieldRightHandConstrainLookAtManager = humandShieldRightHandConstrainLookAtManager;
        this.humanShieldRightHandConstrainLookAtScriptableObject = humanShieldRightHandConstrainLookAtScriptableObject;
    }

    public override void Enter()
    {
        humandShieldRightHandConstrainLookAtManager.SetWeight(1, humanShieldRightHandConstrainLookAtScriptableObject);
        base.Enter();
    }
}
