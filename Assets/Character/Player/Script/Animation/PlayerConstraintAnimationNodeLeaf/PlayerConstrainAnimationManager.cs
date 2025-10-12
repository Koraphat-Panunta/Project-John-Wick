using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class PlayerConstrainAnimationManager : AnimationConstrainNodeManager
{
    public SplineLookConstrain StandSplineLookConstrain;
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
            , StandSplineLookConstrain, 1);

        leanRotationRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0 == false
            , leaningRotation,1);
        leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            ()=> ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf.Precondition() == false
            , leftHandConstraintManager
            ,5);

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
            , this.StandSplineLookConstrain
            , quickSwitchAimSplineLookConstrainScriptableObject
            , () => true);

        primaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is PrimaryWeapon);
        rifle_CAR_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            ,this.StandSplineLookConstrain,standRifleAim_CAR_SplineLookConstrainScriptableObject
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
            ,this.StandSplineLookConstrain,standRifleAimSplineLookConstrainScriptableObject
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
            , this.StandSplineLookConstrain
            , standPistolAim_CAR_SplineLookConstrainScriptableObject
             , () => player._currentWeapon is SecondaryWeapon);

        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            this.player
            , this.StandSplineLookConstrain
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

        startNodeSelector.AddtoChildNode(playerConstraintCombineNode);

        playerConstraintCombineNode.AddCombineNode(enableDisableConstraintWeightNodeSelector);
        playerConstraintCombineNode.AddCombineNode(rightHandRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(aimDownSightRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(leanRotationRecoveryWeightConstraintNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(leftHandTwoBoneIKRecoveryConstraintManagerWeightNodeLeaf);
        playerConstraintCombineNode.AddCombineNode(constraintNodeStateSelector);
        playerConstraintCombineNode.AddCombineNode(ar15_WeaponGripLeftHandTwoBoneIKNodeLeaf);

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

}
