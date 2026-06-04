using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager
{
    [Header("── Constraint Components ──────────────────────────────────")]
    public HandArmIKConstraintManager leftHandIKConstraint;
    public HandArmIKConstraintManager rightHandIKConstraint;
    public LegsConstrainManager legsConstrainManager;

    [Header("── Body Rotation Constraint ────────────────────────────────")]
    public BodyConstraintManager bodyRotateConstraintManager;

    [Header("── Rig ──────────────────────────────────────────────────────")]
    [SerializeField] private Rig rig;

    [Header("── Body ADS ScriptableObjects ──────────────────────────────")]
    [SerializeField] private BodyRotationConstrainScriptableObject painStateBodyConstraintSCRP;

    [Header("── Body ADS Rotation ScriptableObjects ────────────────────")]
    [SerializeField] private BodyRotationConstrainScriptableObject primaryAimBodyRotationConstrainSCRP;
    [SerializeField] private BodyRotationConstrainScriptableObject secondaryAimBodyRotationConstrainSCRP;
    [SerializeField] private BodyRecoilSCRP shotGun_BodyRecoil_SCRP;

    [Header("── Arm Pain State ScriptableObjects ─────────────────────────")]
    [SerializeField] private TransformOffsetSCRP armAnchorSwingOffsetPosition;
    [SerializeField] private TransformOffsetSCRP armBalancePointOffset;
    [SerializeField] private AnimationCurve painBodyRespondCurve;

    [Header("── Left Hand Grip ScriptableObjects ─────────────────────────")]
    [SerializeField] private TwoBoneIK_ConstraintSCRP primaryWeaponGripLeftHandScrp;
    [SerializeField] private TwoBoneIK_ConstraintSCRP secondaryWeaponGripLeftHandScrp;

    [Header("── Right Hand Weapon Aim ScriptableObjects ─────────────────")]
    [SerializeField] private WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_PrimaryWeapon_SCRP;
    [SerializeField] private WeaponHandIK_ConstraintSCRP rightHand_Target_AimDownSight_SecondaryWeapon_SCRP;
    [SerializeField] private WeaponHandRecoilSCRP rifileHandRecoilData;
    [SerializeField] private WeaponHandRecoilSCRP shotgunHandRecoilData;
    [SerializeField] private WeaponHandRecoilSCRP pistolHandRecoilData;
    [SerializeField] private WeaponHandBlockSCRP primaryWeaponBlockData;
    [SerializeField] private WeaponHandBlockSCRP secondaryWeaponBlockData;

    [Header("── Legs ScriptableObjects ───────────────────────────────────")]
    [SerializeField] private ProceduralLegsWalkConstrainSCRP proceduralLegsPainStateWalkConstrainSCRP;
}
