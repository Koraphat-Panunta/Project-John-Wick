using System;
using UnityEngine;

public class RightHandLookControlAnimationConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    private RightHandConstrainLookAtManager rightHandConstrainLookAtManager;
    private RightHandConstrainLookAtScriptableObject rightHandConstrainLookAtScriptableObject;
    public RightHandLookControlAnimationConstraintNodeLeaf( RightHandConstrainLookAtManager rightHandConstrainLookAtManager
        ,RightHandConstrainLookAtScriptableObject rightHandConstrainLookAtScriptableObject
        ,Func<bool> precondition) : base(precondition)
    {
        this.rightHandConstrainLookAtManager = rightHandConstrainLookAtManager;
        this.rightHandConstrainLookAtScriptableObject = rightHandConstrainLookAtScriptableObject;
    }

    public override void Enter()
    {
        rightHandConstrainLookAtManager.SetWeight(1, rightHandConstrainLookAtScriptableObject);
        base.Enter();
    }
   
}
