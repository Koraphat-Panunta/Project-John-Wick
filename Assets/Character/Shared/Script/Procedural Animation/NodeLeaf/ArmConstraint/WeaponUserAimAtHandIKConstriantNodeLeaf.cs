using System;
using UnityEngine;

public class WeaponUserAimAtHandIKConstriantNodeLeaf : AimAtHandIKConstriantNodeLeaf
{
    public WeaponUserAimAtHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform aimingAtTransform
        , Transform handIK_Transform_Ref_Pos
        , Transform handIK_Transform_Ref_Rot
        , Transform rootHintHand
        , Transform rootCharacter
        , HandIK_ConstraintSCRP rightHandIK_ConstraintSCRP
        , Func<bool> precondition) : 
        base(
            handArmIKConstraintManager
            , aimingAtTransform
            , handIK_Transform_Ref_Pos
            , handIK_Transform_Ref_Rot
            , rootHintHand
            , rootCharacter
            , rightHandIK_ConstraintSCRP
            , precondition
            )
    {
    }
}
