using System;
using UnityEngine;

public class RightHandHoldWeaponConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    private TransformOffsetSCRP targetOffsetAddition;
    private TransformOffsetSCRP hintOffsetAddition;
    private Transform anchor;
    public RightHandHoldWeaponConstraintNodeLeaf(Func<bool> precondition,TransformOffsetSCRP targetOffsetAddition,TransformOffsetSCRP hintOffsetAddition,Transform anchor) : base(precondition)
    {
        this.targetOffsetAddition = targetOffsetAddition;
        this.hintOffsetAddition = hintOffsetAddition;
        this.anchor = anchor;
    }
}
