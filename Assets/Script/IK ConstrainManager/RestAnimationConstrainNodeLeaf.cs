using System;
using UnityEngine;

public class RestAnimationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    public RestAnimationConstrainNodeLeaf(Func<bool> precondition) : base(precondition)
    {
    }
}
