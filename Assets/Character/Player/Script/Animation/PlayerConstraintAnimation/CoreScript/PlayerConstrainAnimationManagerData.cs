using UnityEngine;

public partial class PlayerConstrainAnimationManager
{
    [Header("── Constraint Components ──────────────────────────────────")]
    public BodyLookConstrainManager standSplineLookConstrain;
    public LeaningRotation leaningRotation;
    public HandArmIKConstraintManager leftHandConstraintManager;
    public HandArmIKConstraintManager rightHandIKConstriantManager;
    public LegsConstrainManager legsConstraintManager;
    public HeadRotationConstraintManager headLookConstraintManager;

    [Header("── Body ADS ScriptableObjects ──────────────────────────────")]
    public AimBodyConstrainScriptableObject body_ADS_Prone_Constrain_SCRP;
    public AimBodyConstrainScriptableObject quickSwitchAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standPistolAim_CAR_SplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standRifleAim_CAR_SplineLookConstrainScriptableObject;

    [Header("── Lean ScriptableObjects ──────────────────────────────────")]
    public LeaningRotaionScriptableObject quickSwitchlLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaning_CAR_ConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaning_CAR_ConstrainScriptableObject;

    [Header("── Right Hand IK ScriptableObjects ────────────────────────")]
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_HumanShield_Primary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_HumanShield_Secondary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_Restrain_Primary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_Restrain_Secondary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_ProneUp_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_ProneUp_SecondaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_ProneDown_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_ProneDown_SecondaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_QuickSwitch_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_SecondaryWeapon_SCRP;

    [Header("── Left Hand IK ScriptableObjects ─────────────────────────")]
    public TwoBoneIK_ConstraintSCRP lowReadyProne_LeftHand_IK_ConstrainSCRP;
    public TwoBoneIK_ConstraintSCRP leftHandIK_QuickSwitch_SCRP;
    public TwoBoneIK_ConstraintSCRP primaryWeaponGripLeftHandScrp;
    public TwoBoneIK_ConstraintSCRP secondaryWeaponGripLeftHandScrp;
    public TwoBoneIK_ConstraintSCRP lowReadyWeaponGripLeftHandScrp;

    [Header("── Legs ScriptableObjects ───────────────────────────────────")]
    public LegsBlendingConstrainScriptableObject proneLegsBlendingConstrainSCRP;
    public LegsBlendingConstrainScriptableObject diveStallLegsBlendingConstrainSCRP;
}
