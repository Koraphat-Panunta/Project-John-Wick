using System;
using UnityEngine;

public class ProneLegsConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    protected LegsConstrainManager legsConstrainManager { get; set; }
    protected LegsBlendingConstrainScriptableObject legsIKConstrainScriptableObject { get; set; }
    public ProneLegsConstrainNodeLeaf(LegsConstrainManager legsConstrainManager
        ,Transform refPos
        ,Transform refDir
        ,LegsBlendingConstrainScriptableObject legsIKConstrainScriptableObject
        ,Func<bool> precondition) : base(precondition)
    {
        this.legsConstrainManager = legsConstrainManager;
        this.legsIKConstrainScriptableObject = legsIKConstrainScriptableObject;
    }
}
