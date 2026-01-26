using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class PlayerConstrainAnimationManager : AnimationConstrainNodeManager
{
    public BodyLookConstrain standSplineLookConstrain;
    public LeaningRotation leaningRotation;
    public RightHandConstrainLookAtManager RightHandConstrainLookAtManager;
    public HandArmIKConstraintManager leftHandConstraintManager;
    public HandArmIKConstraintManager rightHandIKConstriantManager;
    public HeadLookConstraintManager headLookConstraintManager;
    [SerializeField] private Transform leftHandTransformRef;
    [SerializeField] private Transform leftHandBoneTransform;

    public HeadLookConstrainScriptableObject headLookConstrainScriptableObject;

    public AimBodyConstrainScriptableObject quickSwitchAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standPistolAim_CAR_SplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;
    public AimBodyConstrainScriptableObject standRifleAim_CAR_SplineLookConstrainScriptableObject;

    public LeaningRotaionScriptableObject quickSwitchlLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject pistolLeaning_CAR_ConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaning_CAR_ConstrainScriptableObject;

    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_rifle;
    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_pistol;

    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_pistol;
    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_rifle;

    public RightHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP;

    public RightHandIK_ConstraintSCRP rightHand_Target_AimDownSight_PrimaryWeapon_SCRP;

    public RightHandIK_ConstraintSCRP rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP;

    public RightHandIK_ConstraintSCRP rightHand_Target_AimDownSight_SecondaryWeapon_SCRP;

    public WeaponGripLeftHandScriptableObject ar15_WeaponGripLeftHandScrp;


    [SerializeField] private Rig rig;
    [SerializeField] private RigBuilder rigBuilder;

    [SerializeField] private string curState;

    public Player player;

    public PlayerAnimationManager playerAnimationManager;
    private bool isCAR => playerAnimationManager.isIn_C_A_R_aim;

    public NodeComponentManager playeBodyConstriantAnimationNodeComponentManager;
    public NodeComponentManager rightHandConstraintAnimationNodeComponentManager;
    public NodeComponentManager leftHandConstraintAnimationNodeComponentManager;
    public NodeComponentManager headConstraintAnimationNodeComponentManager;

    #region BodyConstraint

    //BODY LOOK CONSTRIANT
    public NodeSelector bodyLookConstrainSelector { get; private set; }
    public NodeSelector bodyWeaponManuverConstrainSelector { get; private set; }

    public RecoveryConstraintManagerWeightNodeLeaf splineLookConstraintRecoveryWeightConstraintNodeLeaf { get; set; }
    public AimDownSightBodyConstrainNodeLeaf quickSwitch_ADS_ConstrainNodeLeaf { get; private set; }

    public NodeSelector primaryADS_Constraint_NodeSelector;
    public AimDownSightBodyConstrainNodeLeaf rifle_ADS_ConstrainNodeLeaf { get; private set; }
    public AimDownSightBodyConstrainNodeLeaf rifle_CAR_ADS_ConstrainNodeLeaf { get; private set; }

    public NodeSelector secondaryADS_Constraint_NodeSelector;
    public AimDownSightBodyConstrainNodeLeaf pistol_ADS_ConstrainNodeLeaf { get; private set; }
    public AimDownSightBodyConstrainNodeLeaf pistol_ADS_CAR_ConstrainNodeLeaf { get; protected set; }

    private void InitializedSplineLook()
    {
        //1
        this.bodyLookConstrainSelector = new NodeSelector(() => true);

        this.bodyWeaponManuverConstrainSelector = new NodeSelector(
            () => this.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0 
            && this.playerStateManager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>() == false
            && this.playerStateManager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>() == false
            );

        this.splineLookConstraintRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
                    () => true
                    , standSplineLookConstrain, 1);

        //2
        this.quickSwitch_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , quickSwitchAimSplineLookConstrainScriptableObject
            , () => playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());

        this.primaryADS_Constraint_NodeSelector = new NodeSelector(
           () => player._currentWeapon is PrimaryWeapon);

        this.secondaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is SecondaryWeapon);

        //3
        this.rifle_CAR_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain, standRifleAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        this.rifle_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain, standRifleAimSplineLookConstrainScriptableObject
            , () => true);


        this.pistol_ADS_CAR_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(this.player
            , this.standSplineLookConstrain
            , standPistolAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        this.pistol_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , standPistolAimSplineLookConstrainScriptableObject
            , () => true);

        this.bodyLookConstrainSelector.AddtoChildNode(this.bodyWeaponManuverConstrainSelector);
        this.bodyLookConstrainSelector.AddtoChildNode(this.splineLookConstraintRecoveryWeightConstraintNodeLeaf);

        this.bodyWeaponManuverConstrainSelector.AddtoChildNode(this.quickSwitch_ADS_ConstrainNodeLeaf);
        this.bodyWeaponManuverConstrainSelector.AddtoChildNode(this.primaryADS_Constraint_NodeSelector);
        this.bodyWeaponManuverConstrainSelector.AddtoChildNode(this.secondaryADS_Constraint_NodeSelector);

        this.primaryADS_Constraint_NodeSelector.AddtoChildNode(this.rifle_CAR_ADS_ConstrainNodeLeaf);
        this.primaryADS_Constraint_NodeSelector.AddtoChildNode(this.rifle_ADS_ConstrainNodeLeaf);

        this.secondaryADS_Constraint_NodeSelector.AddtoChildNode(this.pistol_ADS_CAR_ConstrainNodeLeaf);
        this.secondaryADS_Constraint_NodeSelector.AddtoChildNode(this.pistol_ADS_ConstrainNodeLeaf);

        this.playeBodyConstriantAnimationNodeComponentManager.AddNode(this.bodyLookConstrainSelector);
    }

    //BODY LEAN CONSTRAINT
    public NodeSelector leanConstraintSelector { get; private set; }
    public NodeSelector leanWeaponManuverNodeSelector { get; private set; }
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
        //1
        this.leanConstraintSelector = new NodeSelector(() => true);

        //2
        this.leanWeaponManuverNodeSelector = new NodeSelector(
                     () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
                     && player._currentWeapon != null
                     && playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf == false
                     && playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf == false);

        this.leanRotationRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => true
            , leaningRotation, 1);

        //3
        this.quickSwitch_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player
           , this.quickSwitchlLeaningConstrainScriptableObject
           , this.leaningRotation
           , this.player
           , () => playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());
        this.leanPrimaryWeaponNodeSelector = new NodeSelector(() => player._currentWeapon is PrimaryWeapon);
        this.leanSecondaryWeaponNodeSelector = new NodeSelector(() => player._currentWeapon is SecondaryWeapon);

        //4
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
       
        this.leanConstraintSelector.AddtoChildNode(this.leanWeaponManuverNodeSelector);
        this.leanConstraintSelector.AddtoChildNode(this.leanRotationRecoveryWeightConstraintNodeLeaf);

        this.leanWeaponManuverNodeSelector.AddtoChildNode(this.quickSwitch_leaningRotationConstrainNodeLeaf);
        this.leanWeaponManuverNodeSelector.AddtoChildNode(this.leanPrimaryWeaponNodeSelector);
        this.leanWeaponManuverNodeSelector.AddtoChildNode(this.leanSecondaryWeaponNodeSelector);

        this.leanPrimaryWeaponNodeSelector.AddtoChildNode(this.rifle_CAR_leaningRotationConstrainNodeLeaf);
        this.leanPrimaryWeaponNodeSelector.AddtoChildNode(this.rifle_leaningRotationConstrainNodeLeaf);

        this.leanSecondaryWeaponNodeSelector.AddtoChildNode(this.pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf);
        this.leanSecondaryWeaponNodeSelector.AddtoChildNode(this.pistol_leaningRotationConstrainNodeLeaf);

        this.playeBodyConstriantAnimationNodeComponentManager.AddNode(this.leanConstraintSelector);

    }

    //WEIGHT CONSTRIANT
    public NodeSelector enableDisableConstraintWeightNodeSelector { get; set; }
    public SetRigWeightNodeLeaf enableConstraintWeight { get; set; }
    public SetRigWeightNodeLeaf disableConstraintWeight { get; set; }

    private void InitializedConstraintWeightManager()
    {
        this.enableDisableConstraintWeightNodeSelector = new NodeSelector(() => true, "enableDisableConstraintWeightNodeSelector");
        this.enableConstraintWeight = new SetRigWeightNodeLeaf(() => isConstraintEnable, rig, 4, 1);
        this.disableConstraintWeight = new SetRigWeightNodeLeaf(() => true, rig, 5, .2f, 0);

        this.enableDisableConstraintWeightNodeSelector.AddtoChildNode(this.enableConstraintWeight);
        this.enableDisableConstraintWeightNodeSelector.AddtoChildNode(this.disableConstraintWeight);

        this.playeBodyConstriantAnimationNodeComponentManager.AddNode(this.enableDisableConstraintWeightNodeSelector);
    }
    #endregion

    #region RightHandConstraint

    public NodeSelector rightHandConstriantSelector { get; private set; }

    public NodeSelector rightHandConstraintWeightSelector { get; private set; } 

    public NodeSelector rightHandAimDownSightSelector { get; private set; }
    public AimDownSightHandIKConstriantNodeLeaf rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon { get; private set;}
    public AimDownSightHandIKConstriantNodeLeaf rightHand_AimDownSight_Constraint_PrimaryWeapon { get; private set; }
    public AimDownSightHandIKConstriantNodeLeaf rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon { get; private set; }
    public AimDownSightHandIKConstriantNodeLeaf rightHand_AimDownSight_Constraint_SecondaryWeapon { get; private set; }

    public NodeSelector humanShieldConstrainSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_secondary_AnimationConstraintNodeLeaf { get; private set; }

    public NodeSelector restrictConstraintSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_pistol_AnimationConstraintNodeLeaf { get; private set; }

    public RestNodeLeaf rightHandConstraintRestNodeLeaf { get; private set; }

    public SetConstraintWeightNodeLeaf rightHandEnableWeightConstraintNodeLeaf { get; set; }
    public SetConstraintWeightNodeLeaf rightHandRecoveryWeightConstraintNodeLeaf { get; set; }

    private void InitialzedRightHandNodeManager()
    {
        //1
        this.rightHandConstriantSelector = new NodeSelector(() => true);
        this.rightHandConstraintWeightSelector = new NodeSelector(() => true);
        

        //2
        this.humanShieldConstrainSelector = new NodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf
            );

        this.restrictConstraintSelector = new NodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf);

        this.rightHandAimDownSightSelector = new NodeSelector(() => this.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0);
        this.rightHandConstraintRestNodeLeaf = new RestNodeLeaf(() => true);

        this.rightHandEnableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.rightHandConstriantSelector.curNodeLeaf != this.rightHandConstraintRestNodeLeaf
            , this.rightHandIKConstriantManager
            , 1
            , 1);
        this.rightHandRecoveryWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true       
            , this.rightHandIKConstriantManager
            ,1
            ,0);

        //3
        this.humanShield_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(this.RightHandConstrainLookAtManager, this.humanShieldRightHandConstrainLookAtScriptableObject_rifle,
           () => this.player._currentWeapon is PrimaryWeapon);
        this.humanShield_secondary_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(this.RightHandConstrainLookAtManager, this.humanShieldRightHandConstrainLookAtScriptableObject_pistol,
            () => this.player._currentWeapon is SecondaryWeapon);

        this.restrict_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(this.RightHandConstrainLookAtManager, this.restrictRightHandConstrainLookAtScriptableObject_rifle,
            () => this.player._currentWeapon is PrimaryWeapon);
        this.restrict_pistol_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(this.RightHandConstrainLookAtManager, this.restrictRightHandConstrainLookAtScriptableObject_pistol,
            () => this.player._currentWeapon is SecondaryWeapon);

        this.rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon = new AimDownSightHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            ,this.player._spine_2_Bone
            ,this.player._hipBone
            ,this.rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP
            ,this.player
            ,() => this.player._currentWeapon is PrimaryWeapon && playerAnimationManager.isIn_C_A_R_aim);

        this.rightHand_AimDownSight_Constraint_PrimaryWeapon = new AimDownSightHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.player._spine_2_Bone
            , this.player._hipBone
            , this.rightHand_Target_AimDownSight_PrimaryWeapon_SCRP
            , this.player
            , () => this.player._currentWeapon is PrimaryWeapon);

        this.rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon = new AimDownSightHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.player._spine_2_Bone
            , this.player._hipBone
            , this.rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP
            , this.player
            , () => this.player._currentWeapon is SecondaryWeapon && this.playerAnimationManager.isIn_C_A_R_aim);

        this.rightHand_AimDownSight_Constraint_SecondaryWeapon = new AimDownSightHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.player._spine_2_Bone
            , this.player._hipBone
            , this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP
            , this.player
            , () => this.player._currentWeapon is SecondaryWeapon);

        this.rightHandConstriantSelector.AddtoChildNode(this.restrictConstraintSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.humanShieldConstrainSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandAimDownSightSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandConstraintRestNodeLeaf);

        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandEnableWeightConstraintNodeLeaf);
        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandRecoveryWeightConstraintNodeLeaf);

        this.restrictConstraintSelector.AddtoChildNode(this.restrict_rifle_AnimationConstraintNodeLeaf);
        this.restrictConstraintSelector.AddtoChildNode(this.restrict_pistol_AnimationConstraintNodeLeaf);

        this.restrictConstraintSelector.AddtoChildNode(this.humanShield_rifle_AnimationConstraintNodeLeaf);
        this.restrictConstraintSelector.AddtoChildNode(this.humanShield_secondary_AnimationConstraintNodeLeaf);

        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_Constraint_PrimaryWeapon);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_Constraint_SecondaryWeapon);

        this.rightHandConstraintAnimationNodeComponentManager.AddNode(this.rightHandConstriantSelector);
        this.rightHandConstraintAnimationNodeComponentManager.AddNode(this.rightHandConstraintWeightSelector);
    }
    #endregion

    #region LeftHandConstraint
    public WeaponLeftHandGripHandConstraintNodeLeaf ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf { get; private set; }
    public RecoveryConstraintManagerWeightNodeLeaf leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf { get; set; }
    public NodeSelector leftHandConstraintNodeSelector { get; private set; }
    private void InitializedLeftHandNodeManager()
    {
        this.leftHandConstraintNodeSelector = new NodeSelector(()=> true);
        this.ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
                   () => isWeaponGripConstraitEnable && player._currentWeapon != null && player._currentWeapon is PrimaryWeapon
                   ,this.leftHandBoneTransform
                   , this.leftHandTransformRef
                   , this.leftHandConstraintManager
                   , this.ar15_WeaponGripLeftHandScrp
                   , this.player);
        this.leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => true
            , leftHandConstraintManager
            , 5);

        this.leftHandConstraintNodeSelector.AddtoChildNode(this.ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(this.leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf);

        this.leftHandConstraintAnimationNodeComponentManager.AddNode(this.leftHandConstraintNodeSelector);

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

        this.headConstraintAnimationNodeComponentManager.AddNode(this.headLookNodeSelector);
    }
    #endregion

   

    public override void Initialized()
    {
        this.playeBodyConstriantAnimationNodeComponentManager = new NodeComponentManager();
        this.rightHandConstraintAnimationNodeComponentManager = new NodeComponentManager();
        this.leftHandConstraintAnimationNodeComponentManager = new NodeComponentManager();
        this.headConstraintAnimationNodeComponentManager = new NodeComponentManager();

        this.player.crosshairController.crosshairLookPostion += this.UpdateConstrainLookReferencePos;

        this.InitializedConstraintWeightManager();
        this.InitializedSplineLook();
        this.InitializedLeanNodeManager();
        this.InitialzedRightHandNodeManager();
        this.InitializedLeftHandNodeManager();
        this.InitializedHeadLookConstriant();

    }

    protected void FixedUpdate()
    {
        this.playeBodyConstriantAnimationNodeComponentManager.FixedUpdate();
        this.rightHandConstraintAnimationNodeComponentManager.FixedUpdate();
        this.leftHandConstraintAnimationNodeComponentManager.FixedUpdate();
        this.headConstraintAnimationNodeComponentManager.FixedUpdate();


    }
    protected void Update()
    {

        this.playeBodyConstriantAnimationNodeComponentManager.Update();
        this.rightHandConstraintAnimationNodeComponentManager.Update();
        this.leftHandConstraintAnimationNodeComponentManager.Update();
        this.headConstraintAnimationNodeComponentManager.Update();
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

    private void UpdateConstrainLookReferencePos(Vector3 lookingPosition)
    {

        Vector3 poitnPos = lookingPosition;

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

        this.ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf.UpdateHandPosition();
    }

    
    #endregion
}
