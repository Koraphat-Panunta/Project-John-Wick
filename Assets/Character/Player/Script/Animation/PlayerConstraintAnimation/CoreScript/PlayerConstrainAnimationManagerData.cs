using UnityEngine;

public partial class PlayerConstrainAnimationManager
{
    [Header("── Constraint Components ──────────────────────────────────")]
    public BodyConstraintManager bodyRotateConstraintManager;
    public HandArmIKConstraintManager leftHandConstraintManager;
    public HandArmIKConstraintManager rightHandIKConstriantManager;
    public LegsConstrainManager legsConstraintManager;
    public HeadRotationConstraintManager headLookConstraintManager;

    [Header("── Body ADS ScriptableObjects ──────────────────────────────")]
    public BodyRotationScriptableObjectBlend body_ADS_Prone_Constrain_SCRP;
    public BodyRotationConstrainScriptableObject body_Restrain_ConstrainSCRP;
    public BodyRotationConstrainScriptableObject quickSwitchAimSplineLookConstrainScriptableObject;
    public BodyRotationConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public BodyRotationConstrainScriptableObject standPistolAim_CAR_SplineLookConstrainScriptableObject;
    public BodyRotationConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;
    public BodyRotationConstrainScriptableObject standRifleAim_CAR_SplineLookConstrainScriptableObject;

    [Header("── Lean ScriptableObjects ──────────────────────────────────")]
    public LeaningRotaionScriptableObject leaningConstrainScriptableObject;


    [Header("── Right Hand IK ScriptableObjects ────────────────────────")]
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_HumanShield_Primary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_HumanShield_Secondary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_Restrain_Primary_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_Restrain_Secondary_SCRP;
    public TwoBoneIKBlendingConstrainScriptableObject rightHand_AimDownSight_Prone_PrimaryWeapon_SCRP;
    public TwoBoneIKBlendingConstrainScriptableObject rightHand_AimDownSight_Prone_SecondaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_PrimaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_AimDownSight_QuickSwitch_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP;
    public WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_SecondaryWeapon_SCRP;

    public WeaponHandRecoilSCRP rifileHandRecoilData;
    public WeaponHandRecoilSCRP shotgunHandRecoilData;
    public WeaponHandRecoilSCRP pistolHandRecoilData;

    public WeaponHandBlockSCRP primaryWeaponBlockData;
    public WeaponHandBlockSCRP secondaryWeaponBlockData;

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
