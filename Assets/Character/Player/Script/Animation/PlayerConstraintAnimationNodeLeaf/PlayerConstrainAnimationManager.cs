using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class PlayerConstrainAnimationManager : AnimationConstrainNodeManager
{
    public SplineLookConstrain standSplineLookConstrain;
    public LeaningRotation leaningRotation;
    public RightHandConstrainLookAtManager RightHandConstrainLookAtManager;
    public HandArmIKConstraintManager leftHandConstraintManager;
    public HeadLookConstraintManager headLookConstraintManager;
    [SerializeField] private Transform leftHandTransformRef;
    [SerializeField] private Transform leftHandBoneTransform;

    public HeadLookConstrainScriptableObject headLookConstrainScriptableObject;

    public AimSplineLookConstrainScriptableObject quickSwitchAimSplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standPistolAim_CAR_SplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standRifleAim_CAR_SplineLookConstrainScriptableObject;

    public LeaningRotaionScriptableObject quickSwitchlLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaning_CAR_ConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaning_CAR_ConstrainScriptableObject;

    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_rifle;
    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_pistol;

    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_pistol;
    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_rifle;

    public WeaponGripLeftHandScriptableObject ar15_WeaponGripLeftHandScrp;


    [SerializeField] private Rig rig;

    [SerializeField] private string curState;

    public Player player;

    public PlayerAnimationManager playerAnimationManager;
    private bool isCAR => playerAnimationManager.isIn_C_A_R_aim;

    public NodeComponentManager playerConstriantAnimationNodeComponentManager;


    #region AimDownSightConstraint
    public NodeSelector constraintNodeStateSelector { get; set; }

    public AnimationConstrainNodeSelector gunFuConstraintSelector { get; private set; }
    public AnimationConstrainNodeSelector aimDownSightConstrainSelector { get; private set; }
    public RestNodeLeaf restAnimationConstrainNodeLeaf { get; private set; }
    public RestNodeLeaf rest_gunfu_AnimationConstrainNodeLeaf { get; private set; }

    public AimDownSightAnimationConstrainNodeLeaf quickSwitch_ADS_ConstrainNodeLeaf { get; private set; }

    public NodeSelector primaryADS_Constraint_NodeSelector;

    public AimDownSightAnimationConstrainNodeLeaf rifle_ADS_ConstrainNodeLeaf { get; private set; }


    public AimDownSightAnimationConstrainNodeLeaf rifle_CAR_ADS_ConstrainNodeLeaf { get; private set; }



    public NodeSelector secondaryADS_Constraint_NodeSelector;

    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_ConstrainNodeLeaf { get; private set; }


    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_CAR_ConstrainNodeLeaf { get; protected set; }

    public AnimationConstrainNodeSelector humanShieldConstrainSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_secondary_AnimationConstraintNodeLeaf { get; private set; }
    public AnimationConstrainNodeSelector restrictConstraintSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_pistol_AnimationConstraintNodeLeaf { get; private set; }


    private void InitialzedAimDownSightConstraintNodeManager()
    {
        constraintNodeStateSelector = new NodeSelector(() => isConstraintEnable);

        gunFuConstraintSelector = new AnimationConstrainNodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is IGunFuNode);
        humanShieldConstrainSelector = new AnimationConstrainNodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf
            );
        humanShield_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, humanShieldRightHandConstrainLookAtScriptableObject_rifle,
            () => player._currentWeapon is PrimaryWeapon);
        humanShield_secondary_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, humanShieldRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        restrictConstraintSelector = new AnimationConstrainNodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf);
        restrict_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, restrictRightHandConstrainLookAtScriptableObject_rifle,
            () => player._currentWeapon is PrimaryWeapon);
        restrict_pistol_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, restrictRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        rest_gunfu_AnimationConstrainNodeLeaf = new RestNodeLeaf(() => true);

        aimDownSightConstrainSelector = new AnimationConstrainNodeSelector(() => player._currentWeapon != null && player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0);

        quickSwitch_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , quickSwitchAimSplineLookConstrainScriptableObject
            , () => playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());

        primaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is PrimaryWeapon);
        rifle_CAR_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain, standRifleAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        rifle_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain, standRifleAimSplineLookConstrainScriptableObject
            , () => true);

        secondaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is SecondaryWeapon);
        pistol_ADS_CAR_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player
            , this.standSplineLookConstrain
            , standPistolAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , standPistolAimSplineLookConstrainScriptableObject
            , () => true);

        restAnimationConstrainNodeLeaf = new RestNodeLeaf(()=> true);

        constraintNodeStateSelector.AddtoChildNode(gunFuConstraintSelector);
        constraintNodeStateSelector.AddtoChildNode(aimDownSightConstrainSelector);
        constraintNodeStateSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

        //ADS Constraint
        aimDownSightConstrainSelector.AddtoChildNode(quickSwitch_ADS_ConstrainNodeLeaf);
        aimDownSightConstrainSelector.AddtoChildNode(primaryADS_Constraint_NodeSelector);
        aimDownSightConstrainSelector.AddtoChildNode(secondaryADS_Constraint_NodeSelector);

        primaryADS_Constraint_NodeSelector.AddtoChildNode(rifle_CAR_ADS_ConstrainNodeLeaf);
        primaryADS_Constraint_NodeSelector.AddtoChildNode(rifle_ADS_ConstrainNodeLeaf);

        secondaryADS_Constraint_NodeSelector.AddtoChildNode(pistol_ADS_CAR_ConstrainNodeLeaf);
        secondaryADS_Constraint_NodeSelector.AddtoChildNode(pistol_ADS_ConstrainNodeLeaf);

        gunFuConstraintSelector.AddtoChildNode(restrictConstraintSelector);
        gunFuConstraintSelector.AddtoChildNode(humanShieldConstrainSelector);
        gunFuConstraintSelector.AddtoChildNode(rest_gunfu_AnimationConstrainNodeLeaf);

        restrictConstraintSelector.AddtoChildNode(restrict_rifle_AnimationConstraintNodeLeaf);
        restrictConstraintSelector.AddtoChildNode(restrict_pistol_AnimationConstraintNodeLeaf);

        humanShieldConstrainSelector.AddtoChildNode(humanShield_rifle_AnimationConstraintNodeLeaf);
        humanShieldConstrainSelector.AddtoChildNode(humanShield_secondary_AnimationConstraintNodeLeaf);

        this.playerConstriantAnimationNodeComponentManager.AddNode(constraintNodeStateSelector);

    }

    #endregion

    #region LeftHandConstraint
    public WeaponGripLeftHandTwoBoneIKNodeLeaf ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf { get; private set; }
    public RecoveryConstraintManagerWeightNodeLeaf leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf { get; set; }
    public NodeSelector leftHandConstraintNodeSelector { get; private set; }
    private void InitializedLeftHandNodeManager()
    {
        this.leftHandConstraintNodeSelector = new NodeSelector(()=> true);
        ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponGripLeftHandTwoBoneIKNodeLeaf(
                   () => isWeaponGripConstraitEnable && player._currentWeapon != null && player._currentWeapon is PrimaryWeapon
                   ,this.leftHandBoneTransform
                   , this.leftHandTransformRef
                   , this.leftHandConstraintManager
                   , this.ar15_WeaponGripLeftHandScrp
                   , this.player);
        leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => true
            , leftHandConstraintManager
            , 5);

        this.leftHandConstraintNodeSelector.AddtoChildNode(ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf);

        this.playerConstriantAnimationNodeComponentManager.AddNode(this.leftHandConstraintNodeSelector);

    }
    #endregion

    #region RightHandConstraint
    public RecoveryConstraintManagerWeightNodeLeaf rightHandRecoveryWeightConstraintNodeLeaf { get; set; }
    private void InitialzedRightHandNodeManager()
    {
        rightHandRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
                   () =>
                   {
                       if (playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf)
                           return false;
                       if (playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf)
                           return false;
                       return true;
                   }
                   , this.RightHandConstrainLookAtManager, 1);

        this.playerConstriantAnimationNodeComponentManager.AddNode(rightHandRecoveryWeightConstraintNodeLeaf);
    }
    #endregion

    #region LeanConstraint
    public NodeSelector leanEnableNodeSelector { get; private set; }
    public NodeSelector leanNodeSelector { get; private set; }
    public RecoveryConstraintManagerWeightNodeLeaf leanRotationRecoveryWeightConstraintNodeLeaf { get; set; }

    public NodeSelector leanPrimaryWeaponNodeSelector { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf rifle_leaningRotationConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf rifle_CAR_leaningRotationConstrainNodeLeaf { get; private set; }

    public NodeSelector leanSecondaryWeaponNodeSelector { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf pistol_leaningRotationConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf { get; protected set; }

    public PlayerLeaningRotationConstrainNodeLeaf quickSwitch_leaningRotationConstrainNodeLeaf { get; private set; }

    private void InitializedLeanNodeManager()
    {
        this.leanEnableNodeSelector = new NodeSelector(()=> true);
        this.leanNodeSelector = new NodeSelector(
                     () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
                     && player._currentWeapon != null
                     && playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf == false
                     && playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf == false);

        this.leanRotationRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => true
            , leaningRotation, 1);

        this.leanPrimaryWeaponNodeSelector = new NodeSelector(() => player._currentWeapon is PrimaryWeapon);
        this.rifle_CAR_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
            , this.rifileLeaningConstrainScriptableObject
            , leaningRotation
            , player
            , () => isCAR);
        this.rifle_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
            , this.rifileLeaningConstrainScriptableObject
            , leaningRotation
            , player
            , () => true);

        this.leanSecondaryWeaponNodeSelector = new NodeSelector(() => player._currentWeapon is SecondaryWeapon);
        this.pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
            , this.pistolLeaning_CAR_ConstrainScriptableObject
            , leaningRotation
            , player
            , () => isCAR);
        this.pistol_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
            , this.pistolLeaningConstrainScriptableObject
            , leaningRotation
            , player
            , () => true);
        this.quickSwitch_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
            , this.quickSwitchlLeaningConstrainScriptableObject
            , this.leaningRotation
            , this.player
            , () => playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());

        this.leanNodeSelector.AddtoChildNode(this.quickSwitch_leaningRotationConstrainNodeLeaf);
        this.leanNodeSelector.AddtoChildNode(this.leanPrimaryWeaponNodeSelector);
        this.leanNodeSelector.AddtoChildNode(this.leanSecondaryWeaponNodeSelector);

        this.leanPrimaryWeaponNodeSelector.AddtoChildNode(this.rifle_CAR_leaningRotationConstrainNodeLeaf);
        this.leanPrimaryWeaponNodeSelector.AddtoChildNode(this.rifle_leaningRotationConstrainNodeLeaf);

        this.leanSecondaryWeaponNodeSelector.AddtoChildNode(this.pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf);
        this.leanSecondaryWeaponNodeSelector.AddtoChildNode(this.pistol_leaningRotationConstrainNodeLeaf);

        this.leanEnableNodeSelector.AddtoChildNode(this.leanNodeSelector);
        this.leanEnableNodeSelector.AddtoChildNode(this.leanRotationRecoveryWeightConstraintNodeLeaf);

        this.playerConstriantAnimationNodeComponentManager.AddNode(leanEnableNodeSelector);

    }
    #endregion

    #region SplineLook
    public RecoveryConstraintManagerWeightNodeLeaf splineLookConstraintRecoveryWeightConstraintNodeLeaf { get; set; }

    private void InitializedSplineLook()
    {
        splineLookConstraintRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
                    () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0 == false
                    , standSplineLookConstrain, 1);
        this.playerConstriantAnimationNodeComponentManager.AddNode(splineLookConstraintRecoveryWeightConstraintNodeLeaf);
    }
    #endregion

    #region HeadLookConstraint
    public RecoveryConstraintManagerWeightNodeLeaf headLookRecoveryConstraintManagerWeightNodeLeaf { get; set; }
    public HeadLookConstrainAnimationNodeLeaf headLookConstrainNodeLeaf { get; set; }
    public NodeSelector headLookNodeSelector { get; set; }
    private void InitializedHeadLookConstriant()
    {
        this.headLookNodeSelector = new NodeSelector(()=> true);

        this.headLookConstrainNodeLeaf = new HeadLookConstrainAnimationNodeLeaf(
            headLookConstraintManager
            , headLookConstrainScriptableObject
            , () => isHeadLookEnable);

        this.headLookRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
           () => isHeadLookEnable == false
           , headLookConstraintManager
           , 1);

        this.headLookNodeSelector.AddtoChildNode(this.headLookConstrainNodeLeaf);
        this.headLookNodeSelector.AddtoChildNode(this.headLookRecoveryConstraintManagerWeightNodeLeaf);

        this.playerConstriantAnimationNodeComponentManager.AddNode(this.headLookNodeSelector);
    }
    #endregion

    #region WeightConstraint
    public NodeSelector enableDisableConstraintWeightNodeSelector { get; set; }
    public SetWeightConstraintNodeLeaf enableConstraintWeight { get; set; }
    public SetWeightConstraintNodeLeaf disableConstraintWeight { get; set; }

    private void InitializedConstraintWeightManager()
    {
        enableDisableConstraintWeightNodeSelector = new NodeSelector(() => true, "enableDisableConstraintWeightNodeSelector");
        enableConstraintWeight = new SetWeightConstraintNodeLeaf(() => isConstraintEnable, rig, 4, 1);
        disableConstraintWeight = new SetWeightConstraintNodeLeaf(() => true, rig, 5, .2f, 0);

        this.enableDisableConstraintWeightNodeSelector.AddtoChildNode(enableConstraintWeight);
        this.enableDisableConstraintWeightNodeSelector.AddtoChildNode(disableConstraintWeight);

        this.playerConstriantAnimationNodeComponentManager.AddNode(enableDisableConstraintWeightNodeSelector);
    }
    #endregion

    public override void Initialized()
    {
        this.playerConstriantAnimationNodeComponentManager = new NodeComponentManager();

        this.InitialzedAimDownSightConstraintNodeManager();
        this.InitializedLeftHandNodeManager();
        this.InitialzedRightHandNodeManager();
        this.InitializedLeanNodeManager();
        this.InitializedSplineLook();
        this.InitializedHeadLookConstriant();
        this.InitializedConstraintWeightManager();
    }

    protected void FixedUpdate()
    {
        this.playerConstriantAnimationNodeComponentManager.FixedUpdate();

    }
    protected void Update()
    {
        UpdateConstrainLookReferencePos();
        this.playerConstriantAnimationNodeComponentManager.Update();

    }
    private void OnDrawGizmos()
    {
        if (player._currentWeapon != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(player._currentWeapon.bulletSpawner.transform.position, player._currentWeapon.bulletSpawner.transform.forward * 10);

        }
    }

    #region UpdateConstranLookReference

    private Vector3 forwardDir => player.transform.forward;
    private float maxHorizontalRotateDegrees = 30;
    private float maxVerticalRotateDegrees = 60;
    private Vector3 pointingPos;
    [SerializeField] Transform aimConstrainPositionReference;
    [SerializeField] Transform beginPos;

    private void UpdateConstrainLookReferencePos()
    {
        Vector3 poitnPos = player._lookingPos;

        Vector3 startPos = beginPos.position;

        //if (Vector3.Distance(poitnPos, pointingPos) > .5f)
        //    trackRate = 0;

        // Normalize input
        Vector3 dirToPoint = (poitnPos - startPos).normalized;

        // Basis: forward, right, up
        Vector3 fwd = forwardDir.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd).normalized;
        Vector3 up = Vector3.Cross(fwd, right).normalized;

        // Project onto local basis (dot products give angles)
        float horizontalAngle = Mathf.Atan2(Vector3.Dot(dirToPoint, right), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg;
        float verticalAngle = (Mathf.Atan2(Vector3.Dot(dirToPoint, up), Vector3.Dot(dirToPoint, new Vector3(dirToPoint.x, 0, dirToPoint.z))) * Mathf.Rad2Deg) * -1;


        // Clamp angles
        horizontalAngle = Mathf.Clamp(horizontalAngle, -maxHorizontalRotateDegrees, maxHorizontalRotateDegrees);
        verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalRotateDegrees, maxVerticalRotateDegrees);

        // Rebuild direction from clamped angles
        Quaternion rot = Quaternion.AngleAxis(horizontalAngle, Vector3.up) *
                         Quaternion.AngleAxis(verticalAngle, right);
        Vector3 clampedDir = rot * fwd;

        // Final pointing position (you can scale as needed)

        pointingPos = Vector3.Lerp(pointingPos, startPos + (clampedDir.normalized) * 10, 1);
        aimConstrainPositionReference.position = pointingPos;
    }

    
    #endregion
}
