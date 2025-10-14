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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        UpdateConstrainLookReferencePos();
        base.Update();
    }

    public override INodeSelector startNodeSelector { get; set; }
    public NodeCombine playerConstraintCombineNode { get; set; }
    public NodeSelector enableDisableConstraintWeightNodeSelector { get; set; }
    public SetWeightConstraintNodeLeaf enableConstraintWeight { get; set; }
    public SetWeightConstraintNodeLeaf disableConstraintWeight { get; set; }   
    
    public RecoveryConstraintManagerWeightNodeLeaf rightHandRecoveryWeightConstraintNodeLeaf { get; set; }
    public RecoveryConstraintManagerWeightNodeLeaf aimDownSightRecoveryWeightConstraintNodeLeaf { get; set; }
    public RecoveryConstraintManagerWeightNodeLeaf leanRotationRecoveryWeightConstraintNodeLeaf {get; set; }
    public RecoveryConstraintManagerWeightNodeLeaf leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf { get; set; }
    public RecoveryConstraintManagerWeightNodeLeaf headLookRecoveryConstraintManagerWeightNodeLeaf { get; set; }

    public NodeSelector constraintNodeStateSelector { get; set; }
    public RestAnimationConstrainNodeLeaf restAnimationConstrainNodeLeaf { get;private set; }
    public RestAnimationConstrainNodeLeaf rest_gunfu_AnimationConstrainNodeLeaf { get; private set; }

    public AimDownSightAnimationConstrainNodeLeaf quickSwitch_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf quickSwitch_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode quickSwitchADSConstrainCombineNode { get; private set; }

    public NodeSelector primaryADS_Constraint_NodeSelector;

    public AimDownSightAnimationConstrainNodeLeaf rifle_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf rifle_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode rifleADSConstrainCombineNode { get; private set; }

    public AimDownSightAnimationConstrainNodeLeaf rifle_CAR_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf rifle_CAR_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode rifleADS_CAR_ConstrainCombineNode { get; private set; }


    public NodeSelector secondaryADS_Constraint_NodeSelector;

    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf pistol_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode pistolADSConstrainCombineNode { get; private set; }

    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_CAR_ConstrainNodeLeaf { get; protected set; }
    public PlayerLeaningRotationConstrainNodeLeaf pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf { get; protected set; }
    public AnimationConstrainCombineNode pistol_ADS_CAR_ConstrainCombineNode { get; private set; }

    public AnimationConstrainNodeSelector gunFuConstraintSelector { get; private set; }
    public AnimationConstrainNodeSelector humanShieldConstrainSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_secondary_AnimationConstraintNodeLeaf { get; private set; }
    public AnimationConstrainNodeSelector restrictConstraintSelector { get; private set; }  
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_pistol_AnimationConstraintNodeLeaf { get; private set; }

    public AnimationConstrainNodeSelector aimDownSightConstrainSelector { get; private set; }

    public WeaponGripLeftHandTwoBoneIKNodeLeaf ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf { get; private set; }

    public HeadLookConstrainAnimationNodeLeaf headLookConstrainNodeLeaf { get; set; }
    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(()=>true);

        playerConstraintCombineNode = new NodeCombine(()=> true);

        enableDisableConstraintWeightNodeSelector = new NodeSelector(() => true, "enableDisableConstraintWeightNodeSelector");
        enableConstraintWeight = new SetWeightConstraintNodeLeaf(()=> isConstraintEnable,rig,4,1);
        disableConstraintWeight = new SetWeightConstraintNodeLeaf(() => true, rig, 5,.2f, 0);

        rightHandRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            ()=> 
            {
                if (playerStateManager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>())
                    return false;
                if (playerStateManager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>())
                {
                    return false;
                }

                return true;
            }
            ,this.RightHandConstrainLookAtManager,1);

        aimDownSightRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0 == false
            , standSplineLookConstrain, 1);

        leanRotationRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0 == false 
            || playerStateManager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>()
            || playerStateManager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>()
            , leaningRotation,1);
        leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            ()=> ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf.Precondition() == false
            , leftHandConstraintManager
            ,5);
        headLookRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            ()=> isHeadLookEnable == false
            ,headLookConstraintManager
            ,1);

        constraintNodeStateSelector = new NodeSelector(()=> isConstraintEnable);

        aimDownSightConstrainSelector = new AnimationConstrainNodeSelector(()=>player._currentWeapon != null && player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0);

        quickSwitchADSConstrainCombineNode = new AnimationConstrainCombineNode(()=>playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());
        quickSwitch_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(
            this.player
            , this.quickSwitchlLeaningConstrainScriptableObject
            , this.leaningRotation
            , this.player
            , () => true);
        quickSwitch_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , quickSwitchAimSplineLookConstrainScriptableObject
            , () => true);

        primaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is PrimaryWeapon);
        rifle_CAR_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            ,this.standSplineLookConstrain,standRifleAim_CAR_SplineLookConstrainScriptableObject
            ,()=> player._currentWeapon is PrimaryWeapon);
        rifle_CAR_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf
            (this.player
            , this.rifileLeaningConstrainScriptableObject
            , leaningRotation
            , player
            , () => player._currentWeapon is PrimaryWeapon);
        rifleADS_CAR_ConstrainCombineNode = new AnimationConstrainCombineNode(
            () => isCAR);

        rifle_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            ,this.standSplineLookConstrain,standRifleAimSplineLookConstrainScriptableObject
            ,()=> player._currentWeapon is PrimaryWeapon);
        rifle_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(
            this.player
            , this.rifileLeaningConstrainScriptableObject
            , leaningRotation,player
            , () => player._currentWeapon is PrimaryWeapon);
        rifleADSConstrainCombineNode = new AnimationConstrainCombineNode(
            ()=> true);

        secondaryADS_Constraint_NodeSelector = new NodeSelector(()=> player._currentWeapon is SecondaryWeapon);

        pistol_ADS_CAR_ConstrainCombineNode = new AnimationConstrainCombineNode(
            ()=> isCAR);
        pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(
            this.player
            , this.pistolLeaning_CAR_ConstrainScriptableObject
            , leaningRotation
            , player
            , () => player._currentWeapon is SecondaryWeapon);
        pistol_ADS_CAR_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player
            , this.standSplineLookConstrain
            , standPistolAim_CAR_SplineLookConstrainScriptableObject
             , () => player._currentWeapon is SecondaryWeapon);

        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.standSplineLookConstrain
            , standPistolAimSplineLookConstrainScriptableObject
             , () => player._currentWeapon is SecondaryWeapon);
        pistol_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(
            this.player
            , this.pistolLeaningConstrainScriptableObject
            , leaningRotation
            , player
            , () => player._currentWeapon is SecondaryWeapon);
        pistolADSConstrainCombineNode = new AnimationConstrainCombineNode(() => true);

        restAnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(rig,() => true);

        gunFuConstraintSelector = new AnimationConstrainNodeSelector(
            ()=> playerStateManager.TryGetCurNodeLeaf<IGunFuNode>());
        humanShieldConstrainSelector = new AnimationConstrainNodeSelector(
            () => playerStateManager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>() 
            );
        humanShield_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,humanShieldRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        humanShield_secondary_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, humanShieldRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        restrictConstraintSelector = new AnimationConstrainNodeSelector(
            () => playerStateManager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>());
        restrict_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        restrict_pistol_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        rest_gunfu_AnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(rig, () => true);

        ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponGripLeftHandTwoBoneIKNodeLeaf(
            ()=> isWeaponGripConstraitEnable && player._currentWeapon != null && player._currentWeapon is PrimaryWeapon
            , this.leftHandTransformRef
            ,this.leftHandConstraintManager
            ,this.ar15_WeaponGripLeftHandScrp
            ,this.player);

        headLookConstrainNodeLeaf = new HeadLookConstrainAnimationNodeLeaf(
            headLookConstraintManager
            ,headLookConstrainScriptableObject
            ,()=> isHeadLookEnable);

        startNodeSelector.AddtoChildNode(playerConstraintCombineNode);

        playerConstraintCombineNode.AddCombineNode(enableDisableConstraintWeightNodeSelector);
        playerConstraintCombineNode.AddCombineNode(rightHandRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(aimDownSightRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(leanRotationRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(headLookRecoveryConstraintManagerWeightNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(constraintNodeStateSelector);
        playerConstraintCombineNode.AddCombineNode(ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(headLookConstrainNodeLeaf);

        enableDisableConstraintWeightNodeSelector.AddtoChildNode(enableConstraintWeight);
        enableDisableConstraintWeightNodeSelector.AddtoChildNode(disableConstraintWeight);

        constraintNodeStateSelector.AddtoChildNode(gunFuConstraintSelector);
        constraintNodeStateSelector.AddtoChildNode(aimDownSightConstrainSelector);
        constraintNodeStateSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

        //ADS Constraint
        aimDownSightConstrainSelector.AddtoChildNode(quickSwitchADSConstrainCombineNode);
        aimDownSightConstrainSelector.AddtoChildNode(primaryADS_Constraint_NodeSelector);
        aimDownSightConstrainSelector.AddtoChildNode(secondaryADS_Constraint_NodeSelector);

        quickSwitchADSConstrainCombineNode.AddCombineNode(quickSwitch_ADS_ConstrainNodeLeaf);
        quickSwitchADSConstrainCombineNode.AddCombineNode(quickSwitch_leaningRotationConstrainNodeLeaf);

        primaryADS_Constraint_NodeSelector.AddtoChildNode(rifleADS_CAR_ConstrainCombineNode);
        primaryADS_Constraint_NodeSelector.AddtoChildNode(rifleADSConstrainCombineNode);

        rifleADS_CAR_ConstrainCombineNode.AddCombineNode(rifle_CAR_leaningRotationConstrainNodeLeaf);
        rifleADS_CAR_ConstrainCombineNode.AddCombineNode(rifle_CAR_ADS_ConstrainNodeLeaf);

        rifleADSConstrainCombineNode.AddCombineNode(rifle_leaningRotationConstrainNodeLeaf);
        rifleADSConstrainCombineNode.AddCombineNode(rifle_ADS_ConstrainNodeLeaf);

        secondaryADS_Constraint_NodeSelector.AddtoChildNode(pistol_ADS_CAR_ConstrainCombineNode);
        secondaryADS_Constraint_NodeSelector.AddtoChildNode(pistolADSConstrainCombineNode);

        pistol_ADS_CAR_ConstrainCombineNode.AddCombineNode(pistoll_ADS_CAR_leaningRotationConstrainNodeLeaf);
        pistol_ADS_CAR_ConstrainCombineNode.AddCombineNode(pistol_ADS_CAR_ConstrainNodeLeaf);

        pistolADSConstrainCombineNode.AddCombineNode(pistol_leaningRotationConstrainNodeLeaf);
        pistolADSConstrainCombineNode.AddCombineNode(pistol_ADS_ConstrainNodeLeaf);

        gunFuConstraintSelector.AddtoChildNode(restrictConstraintSelector);
        gunFuConstraintSelector.AddtoChildNode(humanShieldConstrainSelector);
        gunFuConstraintSelector.AddtoChildNode(rest_gunfu_AnimationConstrainNodeLeaf);

        restrictConstraintSelector.AddtoChildNode(restrict_rifle_AnimationConstraintNodeLeaf);
        restrictConstraintSelector.AddtoChildNode(restrict_pistol_AnimationConstraintNodeLeaf);

        humanShieldConstrainSelector.AddtoChildNode(humanShield_rifle_AnimationConstraintNodeLeaf);
        humanShieldConstrainSelector.AddtoChildNode(humanShield_secondary_AnimationConstraintNodeLeaf);

  

        nodeManagerBehavior.SearchingNewNode(this);
    }
    private void OnDrawGizmos()
    {
        if(player._currentWeapon != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(player._currentWeapon.bulletSpawnerPos.position,player._currentWeapon.bulletSpawner.transform.forward*10);

        }
    }

    #region UpdateConstranLookReference

    private Vector3 forwardDir => player.transform.forward;
    private float maxHorizontalRotateDegrees = 30;
    private float maxVerticalRotateDegrees = 60;
    private Vector3 pointingPos;
    [SerializeField] Transform aimConstrainPositionReference;
    [Range(0,10)]
    [SerializeField] float trackRate;
    [SerializeField] Transform beginPos;
    private void UpdateConstrainLookReferencePos()
    {
        Vector3 poitnPos = player._aimPosRef.position;

        Vector3 startPos = beginPos.position;

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
        pointingPos = Vector3.Lerp(pointingPos, startPos + (clampedDir.normalized * Mathf.Clamp((poitnPos - startPos).magnitude, 1, 1)), this.trackRate);
        aimConstrainPositionReference.position = pointingPos;
    }
    #endregion
}
