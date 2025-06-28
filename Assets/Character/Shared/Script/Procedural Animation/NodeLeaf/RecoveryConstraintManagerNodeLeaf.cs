using System;
using UnityEngine;

public class RecoveryConstraintManagerNodeLeaf : AnimationConstrainNodeLeaf
{
    public RecoveryConstraintManagerNodeLeaf(Func<bool> precondition) : base(precondition)
    {
    }
}
