using System;
using UnityEngine;

public class RightHandLookControlAnimationConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    private RightHandConstrainLookAtManager humandShieldRightHandConstrainLookAtManager;
    private RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject;
    public RightHandLookControlAnimationConstraintNodeLeaf( RightHandConstrainLookAtManager humandShieldRightHandConstrainLookAtManager
        ,RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject
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
