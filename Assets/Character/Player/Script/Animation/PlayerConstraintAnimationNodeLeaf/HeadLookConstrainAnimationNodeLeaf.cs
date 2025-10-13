using System;
using UnityEngine;

public class HeadLookConstrainAnimationNodeLeaf : AnimationConstrainNodeLeaf
{

    private HeadLookConstraintManager headLookConstrain;
    private HeadLookConstrainScriptableObject headLookConstrainScriptableObject;
    public HeadLookConstrainAnimationNodeLeaf(
        HeadLookConstraintManager splineLookConstrain
        , HeadLookConstrainScriptableObject aimSplineLookConstrainScriptableObject
        , Func<bool> precondition) : base(precondition)
    {
        this.headLookConstrain = splineLookConstrain;
        this.headLookConstrainScriptableObject = aimSplineLookConstrainScriptableObject;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        headLookConstrain.SetWeight(headLookConstrainScriptableObject.weight, headLookConstrainScriptableObject);
        base.UpdateNode();
    }

}
