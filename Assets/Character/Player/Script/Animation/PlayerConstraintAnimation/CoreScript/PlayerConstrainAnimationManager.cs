using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class PlayerConstrainAnimationManager : AnimationConstrainNodeManager
{
    

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
    public AimDownSightBodyConstrainNodeLeaf bodyProneADS_Constraint_NodeLeaf { get; private set; }
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
            () =>
            this.player._currentWeapon != null 
            && this.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
            && this.playerStateManager.TryGetCurNodeLeaf<IGunFuNode>() == false
            //&& this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>() == false
            //&& this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>() == false
            );

        this.splineLookConstraintRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
                    () => true
                    , standSplineLookConstrain, 10);

        //2
        this.bodyProneADS_Constraint_NodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player.transform
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain
            ,this.body_ADS_Prone_Constrain_SCRP 
            , () => isProne);

        this.quickSwitch_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player._hipBone
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain
            , quickSwitchAimSplineLookConstrainScriptableObject
            , () => playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());

        this.primaryADS_Constraint_NodeSelector = new NodeSelector(
           () => player._currentWeapon is PrimaryWeapon);

        this.secondaryADS_Constraint_NodeSelector = new NodeSelector(
            () => player._currentWeapon is SecondaryWeapon);

        //3
        this.rifle_CAR_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player._hipBone
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain, standRifleAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        this.rifle_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player._hipBone
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain, standRifleAimSplineLookConstrainScriptableObject
            , () => true);


        this.pistol_ADS_CAR_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player._hipBone
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain
            , standPistolAim_CAR_SplineLookConstrainScriptableObject
            , () => isCAR);
        this.pistol_ADS_ConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.player._hipBone
            , this.player._hipBone
            , this.aimConstrainPositionReference
            , this.player
            , this.standSplineLookConstrain
            , standPistolAimSplineLookConstrainScriptableObject
            , () => true);

        this.bodyLookConstrainSelector.AddtoChildNode(this.bodyWeaponManuverConstrainSelector);
        this.bodyLookConstrainSelector.AddtoChildNode(this.splineLookConstraintRecoveryWeightConstraintNodeLeaf);

        this.bodyWeaponManuverConstrainSelector.AddtoChildNode(this.bodyProneADS_Constraint_NodeLeaf);
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
                     && playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf == false 
                     && this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>() == false 
                     && this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>() == false
            );

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
    public NodeSelector rightHand_AimDownSight_Prone_Selector { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_ProneUp_Primary_Constraint_NodeLeaf;
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_ProneUp_Secondary_Constraint_NodeLeaf;
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_ProneDown_Primary_Constraint_NodeLeaf;
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_ProneDown_Secondary_Constraint_NodeLeaf;
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_QuickSwitch_Constraint_NodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon_NodeLeaf { get; private set;}
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_Constraint_PrimaryWeapon_NodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon_NodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_AimDownSight_Constraint_SecondaryWeapon_NodeLeaf { get; private set; }

    public NodeSelector humanShieldConstrainSelector { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_ADS_humanShield_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_ADS_humanShield_secondary_AnimationConstraintNodeLeaf { get; private set; }

    public NodeSelector restrictConstraintSelector { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_ADS_restrict_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_ADS_restrict_pistol_AnimationConstraintNodeLeaf { get; private set; }

    public NodeSelector rightHandLowReadyConstrainNodeSelector { get; private set; }
    public ArmIKConstriantRefTransformNodeLeaf lowReady_Prone_ConstrainNodeLeaf { get; private set; }   

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
            () => playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf
            && humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay
            );

        this.restrictConstraintSelector = new NodeSelector(
            () => playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrain_GunFu_NodeLeaf
            && restrain_GunFu_NodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay
            );

        this.rightHandLowReadyConstrainNodeSelector = new NodeSelector
            (
            () => this.player._currentWeapon != null
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false
            && 
            (
            this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>(out PlayerDolphinDiveStateNodeLeaf dolphinDiveStateNodeLeaf)
            && dolphinDiveStateNodeLeaf.isPassingJump
            || this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
            )
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
            && this.isWeaponSwitching == false
            );

        this.rightHandAimDownSightSelector = new NodeSelector(
            () =>this.player._currentWeapon != null && this.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0  
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
            && this.isWeaponSwitching == false
            );
        this.rightHandConstraintRestNodeLeaf = new RestNodeLeaf(() => true);

        this.rightHandEnableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.rightHandConstriantSelector.curNodeLeaf != this.rightHandConstraintRestNodeLeaf
            , this.rightHandIKConstriantManager
            , 10
            , 1);
        this.rightHandRecoveryWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true       
            , this.rightHandIKConstriantManager
            ,10
            ,0);

        //3
        this.rightHand_ADS_humanShield_rifle_AnimationConstraintNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            ,this.aimConstrainPositionReference
            ,this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            ,this.player._hipBone
            ,this.player
            ,this.rightHand_AimDownSight_HumanShield_Primary_SCRP
           ,() => this.player._currentWeapon is PrimaryWeapon);

        this.rightHand_ADS_humanShield_secondary_AnimationConstraintNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_AimDownSight_HumanShield_Secondary_SCRP
           , () => this.player._currentWeapon is SecondaryWeapon);

        this.rightHand_ADS_restrict_rifle_AnimationConstraintNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_AimDownSight_Restrain_Primary_SCRP
           , () => this.player._currentWeapon is PrimaryWeapon);

        this.rightHand_ADS_restrict_pistol_AnimationConstraintNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_AimDownSight_Restrain_Secondary_SCRP
           , () => this.player._currentWeapon is SecondaryWeapon);

        this.lowReady_Prone_ConstrainNodeLeaf = new ArmIKConstriantRefTransformNodeLeaf
            (()=> true
            ,this.rightHandIKConstriantManager
            ,this.player._spine_2_Bone
            ,this.player._spine_2_Bone
            ,this.lowReadyProne_LeftHand_IK_ConstrainSCRP
            );

        this.rightHand_AimDownSight_Prone_Selector = new NodeSelector(
            ()=> this.isProne);

        this.rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone            
            , this.player._hipBone
            , this.player
            , this.rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP
            ,() => this.player._currentWeapon is PrimaryWeapon && playerAnimationManager.isIn_C_A_R_aim);

        this.rightHand_AimDownSight_Constraint_PrimaryWeapon_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_Target_AimDownSight_PrimaryWeapon_SCRP
            , () => this.player._currentWeapon is PrimaryWeapon);

        this.rightHand_AimDownSight_QuickSwitch_Constraint_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_AimDownSight_QuickSwitch_SCRP            
            , () => this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>());

        this.rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP
            , () => this.player._currentWeapon is SecondaryWeapon && this.playerAnimationManager.isIn_C_A_R_aim);

        this.rightHand_AimDownSight_Constraint_SecondaryWeapon_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player._rightArmBone
            , this.player._hipBone
            , this.player
            , this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP
            , () => this.player._currentWeapon is SecondaryWeapon);

        //4
        this.rightHand_AimDownSight_ProneUp_Primary_Constraint_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
           this.rightHandIKConstriantManager
           , this.aimConstrainPositionReference
           , this.player._rightArmBone
           , this.player._headBone
           , this.player._rightArmBone
           , this.player._spine_2_Bone
           , this.player
           , this.rightHand_AimDownSight_ProneUp_PrimaryWeapon_SCRP
           , () => this.player._currentWeapon is PrimaryWeapon 
           && this.playerAnimationManager.angleLookHorizontal > 45 && this.playerAnimationManager.angleLookHorizontal < 315
           );

        this.rightHand_AimDownSight_ProneUp_Secondary_Constraint_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._headBone
            , this.player._rightArmBone
            , this.player._spine_2_Bone
            , this.player
            , this.rightHand_AimDownSight_ProneUp_SecondaryWeapon_SCRP
            , () => this.player._currentWeapon is SecondaryWeapon 
            && this.playerAnimationManager.angleLookHorizontal > 45 && this.playerAnimationManager.angleLookHorizontal < 315
            );

        this.rightHand_AimDownSight_ProneDown_Primary_Constraint_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
          this.rightHandIKConstriantManager
          , this.aimConstrainPositionReference
          , this.player._rightArmBone
          , this.player._headBone
          , this.player._rightArmBone
          , this.player._headBone
          , this.player
          , this.rightHand_AimDownSight_ProneDown_PrimaryWeapon_SCRP
          , () => this.player._currentWeapon is PrimaryWeapon
          );

        this.rightHand_AimDownSight_ProneDown_Secondary_Constraint_NodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player._rightArmBone
            , this.player._headBone
            , this.player._rightArmBone
            , this.player._headBone
            , this.player
            , this.rightHand_AimDownSight_ProneDown_SecondaryWeapon_SCRP
            , () => this.player._currentWeapon is SecondaryWeapon
            );

        this.rightHandConstriantSelector.AddtoChildNode(this.restrictConstraintSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.humanShieldConstrainSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandLowReadyConstrainNodeSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandAimDownSightSelector);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandConstraintRestNodeLeaf);

        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandEnableWeightConstraintNodeLeaf);
        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandRecoveryWeightConstraintNodeLeaf);

        this.restrictConstraintSelector.AddtoChildNode(this.rightHand_ADS_restrict_rifle_AnimationConstraintNodeLeaf);
        this.restrictConstraintSelector.AddtoChildNode(this.rightHand_ADS_restrict_pistol_AnimationConstraintNodeLeaf);

        this.humanShieldConstrainSelector.AddtoChildNode(this.rightHand_ADS_humanShield_rifle_AnimationConstraintNodeLeaf);
        this.humanShieldConstrainSelector.AddtoChildNode(this.rightHand_ADS_humanShield_secondary_AnimationConstraintNodeLeaf);

        this.rightHandLowReadyConstrainNodeSelector.AddtoChildNode(this.lowReady_Prone_ConstrainNodeLeaf);

        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_Prone_Selector);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon_NodeLeaf);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_Constraint_PrimaryWeapon_NodeLeaf);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_QuickSwitch_Constraint_NodeLeaf);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon_NodeLeaf);
        this.rightHandAimDownSightSelector.AddtoChildNode(this.rightHand_AimDownSight_Constraint_SecondaryWeapon_NodeLeaf);

        this.rightHand_AimDownSight_Prone_Selector.AddtoChildNode(this.rightHand_AimDownSight_ProneUp_Primary_Constraint_NodeLeaf);
        this.rightHand_AimDownSight_Prone_Selector.AddtoChildNode(this.rightHand_AimDownSight_ProneUp_Secondary_Constraint_NodeLeaf);
        this.rightHand_AimDownSight_Prone_Selector.AddtoChildNode(this.rightHand_AimDownSight_ProneDown_Primary_Constraint_NodeLeaf);
        this.rightHand_AimDownSight_Prone_Selector.AddtoChildNode(this.rightHand_AimDownSight_ProneDown_Secondary_Constraint_NodeLeaf);

        this.rightHandConstraintAnimationNodeComponentManager.AddNode(this.rightHandConstriantSelector);
        this.rightHandConstraintAnimationNodeComponentManager.AddNode(this.rightHandConstraintWeightSelector);
    }
    #endregion

    #region LeftHandConstraint

    public NodeSelector leftHandConstraintNodeSelector { get; private set; }
    public NodeSelector leftHandWeightConstraintSelector { get; private set; }

    public ArmIKConstriantRefTransformNodeLeaf leftHandQuickSwitchIKNodeLeaf { get; protected set; }
    public WeaponLeftHandGripHandConstraintNodeLeaf primaryWeaponGripLeftHandTwoBoneIKNodeLeaf { get; private set; }
    public WeaponLeftHandGripHandConstraintNodeLeaf secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf { get; private set; }
    public RestNodeLeaf leftHandConstriantRestNodeLeaf { get; private set; }

    public SetConstraintWeightNodeLeaf leftHandEnableWeightConstraintNodeLeaf { get; set; }
    public SetConstraintWeightNodeLeaf leftHandDisableWeightConstraintNodeLeaf { get; set; }

    private void InitializedLeftHandNodeManager()
    {
        //1
        this.leftHandConstraintNodeSelector = new NodeSelector(()=> true);
        this.leftHandWeightConstraintSelector = new NodeSelector(()=> true);

        //2
        this.leftHandQuickSwitchIKNodeLeaf = new ArmIKConstriantRefTransformNodeLeaf(
            () => this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>()
            , this.leftHandConstraintManager
            , this.player._spine_2_Bone
            , this.player._spine_2_Bone
            , this.leftHandIK_QuickSwitch_SCRP);

        this.primaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
                   () => (this.isWeaponGripConstraitEnable || this.isEnableIK)
                   && this.player._currentWeapon != null
                   && this.player._currentWeapon is PrimaryWeapon
                   , this.rightHandIKConstriantManager.GetTargetHandTransform()
                   , this.leftHandConstraintManager
                   , this.primaryWeaponGripLeftHandScrp
                   , this.player);
        this.secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
                   () => (this.isWeaponGripConstraitEnable || this.isEnableIK)
                   && player._currentWeapon != null
                   && player._currentWeapon is SecondaryWeapon
                   , this.rightHandIKConstriantManager.GetTargetHandTransform()
                   , this.leftHandConstraintManager
                   , this.secondaryWeaponGripLeftHandScrp
                   , this.player);
        this.leftHandConstriantRestNodeLeaf = new RestNodeLeaf(() => true);

        this.leftHandEnableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.leftHandConstraintNodeSelector.curNodeLeaf != leftHandConstriantRestNodeLeaf
            , this.leftHandConstraintManager
            ,5
            ,1);
        this.leftHandDisableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true
            , leftHandConstraintManager
            ,5
            ,0);


        this.leftHandConstraintNodeSelector.AddtoChildNode(this.leftHandQuickSwitchIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(this.primaryWeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(this.secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(this.leftHandConstriantRestNodeLeaf);

        this.leftHandWeightConstraintSelector.AddtoChildNode(this.leftHandEnableWeightConstraintNodeLeaf);
        this.leftHandWeightConstraintSelector.AddtoChildNode(this.leftHandDisableWeightConstraintNodeLeaf);

        this.leftHandConstraintAnimationNodeComponentManager.AddNode(this.leftHandConstraintNodeSelector);
        this.leftHandConstraintAnimationNodeComponentManager.AddNode(this.leftHandWeightConstraintSelector);

    }
    #endregion

    #region HeadLookConstraint
    public NodeSelector headLookNodeSelector { get; set; }
    public HeadLookConstrainAnimationNodeLeaf headLookAtWeaponConstraintNodeLeaf { get; set; }
    public HeadLookConstrainAnimationNodeLeaf headLookPointingPosConstrainNodeLeaf { get; set; }
    public RestNodeLeaf headConstraintRestNodeLeaf { get; set; }

    public NodeSelector headWeightConstraintSelector { get; set; }
    public SetConstraintWeightNodeLeaf headEnableConstraintWeightNodeLeaf { get; set; }
    public SetConstraintWeightNodeLeaf headLookRecoveryConstraintManagerWeightNodeLeaf { get; set; }
   

    
    private void InitializedHeadLookConstriant()
    {
        //1
        this.headLookNodeSelector = new NodeSelector(()=>true);
        this.headWeightConstraintSelector = new NodeSelector(()=>true);

        //2
        this.headLookAtWeaponConstraintNodeLeaf = new HeadLookConstrainAnimationNodeLeaf
            (this.headLookConstraintManager
            ,this.player._mainHandSocket.transform
            ,()=> this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>());
        this.headLookPointingPosConstrainNodeLeaf = new HeadLookConstrainAnimationNodeLeaf
            (
            this.headLookConstraintManager
            ,this.aimConstrainPositionReference
            ,()=> 
            (
            this.playerStateManager.GetCurNodeLeaf() is GunFuExecute_Single_NodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is GunFuHitNodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerDodgeRollStateNodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerBrounceOffGotAttackGunFuNodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerGetUpStateNodeLeaf
            ) == false
            &&
            (this.playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf
            && (humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter || humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Exit)
            ) == false
            &&
            (this.playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf
            && (restrainGunFuStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter || restrainGunFuStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
            ) == false
            );
        this.headConstraintRestNodeLeaf = new RestNodeLeaf(() => true);

        this.headEnableConstraintWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.headLookNodeSelector.curNodeLeaf != this.headConstraintRestNodeLeaf
            , this.headLookConstraintManager
            , 5
            , 1);
        this.headLookRecoveryConstraintManagerWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            ()=> true
            ,this.headLookConstraintManager
            ,5
            ,0);

        this.headLookNodeSelector.AddtoChildNode(this.headLookAtWeaponConstraintNodeLeaf);
        this.headLookNodeSelector.AddtoChildNode(this.headLookPointingPosConstrainNodeLeaf);
        this.headLookNodeSelector.AddtoChildNode(this.headConstraintRestNodeLeaf);

        this.headWeightConstraintSelector.AddtoChildNode(this.headEnableConstraintWeightNodeLeaf);
        this.headWeightConstraintSelector.AddtoChildNode(this.headLookRecoveryConstraintManagerWeightNodeLeaf);

        this.headConstraintAnimationNodeComponentManager.AddNode(this.headLookNodeSelector);
        this.headConstraintAnimationNodeComponentManager.AddNode(this.headWeightConstraintSelector);
    }
    #endregion

    public override void Initialized()
    {
        this.player.AddObserver(this);

        this.playeBodyConstriantAnimationNodeComponentManager = new NodeComponentManager();
        this.rightHandConstraintAnimationNodeComponentManager = new NodeComponentManager();
        this.leftHandConstraintAnimationNodeComponentManager = new NodeComponentManager();
        this.headConstraintAnimationNodeComponentManager = new NodeComponentManager();

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
        this.UpdateConstrainLookReferencePos();

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
    private float maxHorizontalRotateDegrees = 75;
    private float maxVerticalRotateDegrees = 75;
    private Vector3 pointingPos;
    [SerializeField] Transform aimConstrainPositionReference;
    [SerializeField] Transform beginPos;

    private void UpdateConstrainLookReferencePos()
    {
        Ray ray = new Ray(this.player.cinemachineCamera.transform.position, this.player.cinemachineCamera.transform.forward);
        Vector3 hitpos;
        hitpos = ray.GetPoint(5);
       
        this.aimConstrainPositionReference.transform.position = Vector3.Lerp(this.aimConstrainPositionReference.position, hitpos, Time.deltaTime * 100);
    }

    
    #endregion
}
