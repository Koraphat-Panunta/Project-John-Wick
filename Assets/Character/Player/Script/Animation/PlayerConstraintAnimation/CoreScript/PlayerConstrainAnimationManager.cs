
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
    public NodeComponentManager legsConstraintAnimationNodeComponentManager;
    public NodeComponentManager headConstraintAnimationNodeComponentManager;

    #region BodyConstraint

    //BODY LOOK CONSTRIANT
    public NodeSelector bodyLookConstrainSelector { get; private set; }
    public NodeSelector bodyWeaponManuverConstrainSelector { get; private set; }


    public AimDownSightBodyRotationConstraintNodeLeaf prone_BodyLookConstraintNodeLeaf { get; private set; }
    public AimDownSightBodyRotationConstraintNodeLeaf bodyLookConstraintNodeLeaf { get; private set; }
    public BodySetRotationConstraintNodeLeaf bodySetRotationConstraintNodeLeaf { get; private set; }
    public RecoveryConstraintManagerWeightNodeLeaf splineLookConstraintRecoveryWeightConstraintNodeLeaf { get; set; }


    private void InitializedSplineLook()
    {
        //1
        this.bodyLookConstrainSelector = new NodeSelector(() => true);

        

        this.prone_BodyLookConstraintNodeLeaf = new AimDownSightBodyRotationConstraintNodeLeaf(
            this.player.humanoidBone.hips
            , this.player.transform
            , this.aimConstrainPositionReference
            , this.player
            , this.bodyRotateConstraintManager
            , this.body_ADS_Prone_Constrain_SCRP
            , () => this.player._weaponManuverManager.aimingWeight > 0 
            && this.isProne);

        this.bodyLookConstraintNodeLeaf = new AimDownSightBodyRotationConstraintNodeLeaf(
            this.player.humanoidBone.hips
            , this.player.humanoidBone.hips
            , this.aimConstrainPositionReference
            , this.player
            , this.bodyRotateConstraintManager
            , standPistolAimSplineLookConstrainScriptableObject
            , () => this.player._currentWeapon != null
            && this.player._weaponManuverManager.aimingWeight > 0
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()
            && this.playerStateManager.TryGetCurNodeLeaf<I_OCM_Node>() == false
            );

        this.bodySetRotationConstraintNodeLeaf = new BodySetRotationConstraintNodeLeaf(
            this.bodyRotateConstraintManager
            , this.body_Restrain_ConstrainSCRP
            , 0.2f
            , this.player.transform
            , () => this.playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrainNode
                 && (restrainNode.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay 
                 || restrainNode.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter));

        this.splineLookConstraintRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
                    () => true
                    , bodyRotateConstraintManager, 10);

        this.bodyLookConstrainSelector.AddtoChildNode(this.bodySetRotationConstraintNodeLeaf);
        this.bodyLookConstrainSelector.AddtoChildNode(this.prone_BodyLookConstraintNodeLeaf);
        this.bodyLookConstrainSelector.AddtoChildNode(this.bodyLookConstraintNodeLeaf);
        this.bodyLookConstrainSelector.AddtoChildNode(this.splineLookConstraintRecoveryWeightConstraintNodeLeaf);

        this.playeBodyConstriantAnimationNodeComponentManager.AddNode(this.bodyLookConstrainSelector);
    }

    //BODY LEAN CONSTRAINT
    public NodeSelector leanConstraintSelector { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf leaningRotationConstrainNodeLeaf { get; private set; }
    public RestNodeLeaf leaningRestNodeLeaf { get; private set; }


    private void InitializedLeanNodeManager()
    {
        //1
        this.leanConstraintSelector = new NodeSelector(() => true);

        //2
        this.leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf
            (this.player
            , this.leaningConstrainScriptableObject
            , this.bodyRotateConstraintManager
            ,this.bodyLookConstraintNodeLeaf
            , player
            , () => player._weaponManuverManager.aimingWeight > 0         
            && player._currentWeapon != null
            && playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf == false       
            && playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf == false      
            && this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>() == false                  
            && this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>() == false         
            );

        this.leaningRestNodeLeaf = new RestNodeLeaf(() => true);



        this.leanConstraintSelector.AddtoChildNode(this.leaningRotationConstrainNodeLeaf);
        this.leanConstraintSelector.AddtoChildNode(this.leaningRestNodeLeaf);

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

    public ArmIKConstriantRefTransformNodeLeaf rightLowReady_Prone_ConstrainNodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf { get; private set; }
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHandWeaponAimAtIKCinstrainNodeLeaf {get; private set; } 
    public RestNodeLeaf rightHandConstraintRestNodeLeaf { get; private set; }

    public SetConstraintWeightNodeLeaf rightHandEnableWeightConstraintNodeLeaf { get; set; }
    public SetConstraintWeightNodeLeaf rightHandRecoveryWeightConstraintNodeLeaf { get; set; }

    private void InitialzedRightHandNodeManager()
    {
        //1
        this.rightHandConstriantSelector = new NodeSelector(() => true);
        this.rightHandConstraintWeightSelector = new NodeSelector(() => true);


        //2
        this.rightLowReady_Prone_ConstrainNodeLeaf = new ArmIKConstriantRefTransformNodeLeaf
            (
             () => this.player._currentWeapon != null 
             && ((this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>(out PlayerDolphinDiveStateNodeLeaf dolphinDiveStateNodeLeaf)
            && dolphinDiveStateNodeLeaf.isPassingJump)
            || this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>())
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false 
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
            && this.isWeaponSwitching == false
            , this.rightHandIKConstriantManager
            , this.player.humanoidBone._spine_2_Bone
            , this.player.humanoidBone._spine_2_Bone
            , this.lowReadyProne_LeftHand_IK_ConstrainSCRP
            );

        this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player.humanoidBone._spine_2_Bone
            , this.player.humanoidBone.transform
            , this.player.humanoidBone._spine_2_Bone
            , this.player.transform
            , this.player
            , this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP
            ,this.pistolHandRecoilData
            ,this.secondaryWeaponBlockData
           , () => this.player._currentWeapon != null
           && this.isProne
           && this.player._weaponManuverManager.aimingWeight > 0
           && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()
           && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
           && this.isWeaponSwitching == false
           );

        this.rightHandWeaponAimAtIKCinstrainNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstriantManager
            , this.aimConstrainPositionReference
            , this.player.humanoidBone._rightArmBone
            , this.player.humanoidBone._spine_2_Bone
            , this.player.humanoidBone._spine_2_Bone
            , this.player.transform
            , this.player
            , this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP       
            , this.pistolHandRecoilData
            , this.secondaryWeaponBlockData
           , () => this.player._currentWeapon != null 
           && this.player._weaponManuverManager.aimingWeight > 0
           && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() 
           && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
           && this.isWeaponSwitching == false
           );
        this.rightHandConstraintRestNodeLeaf = new RestNodeLeaf(()=>true);

        this.rightHandEnableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.rightHandConstriantSelector.curNodeLeaf != this.rightHandConstraintRestNodeLeaf
            , this.rightHandIKConstriantManager
            , 3
            , 1);
        this.rightHandRecoveryWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true       
            , this.rightHandIKConstriantManager
            ,10
            ,0);

      

        this.rightHandConstriantSelector.AddtoChildNode(this.rightLowReady_Prone_ConstrainNodeLeaf);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandWeaponAimAtIKCinstrainNodeLeaf);
        this.rightHandConstriantSelector.AddtoChildNode(this.rightHandConstraintRestNodeLeaf);

        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandEnableWeightConstraintNodeLeaf);
        this.rightHandConstraintWeightSelector.AddtoChildNode(this.rightHandRecoveryWeightConstraintNodeLeaf);

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
    public WeaponLeftHandGripHandConstraintNodeLeaf lowReadyWeaponLeftHandTwoBoneIKNodeLeaf { get; set; }
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
            , this.player.humanoidBone._spine_2_Bone
            , this.player.humanoidBone._spine_2_Bone
            , this.leftHandIK_QuickSwitch_SCRP);

        this.primaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
                   () => (this.isWeaponGripConstraitEnable || this.isEnableIK)
                   && this.player._currentWeapon != null
                   && this.player._currentWeapon is PrimaryWeapon
                   , this.rightHandIKConstriantManager.GetTargetHandTransform()
                   ,this.leftHandConstraintManager.GetTargetHandTransform()
                   , this.leftHandConstraintManager
                   , this.primaryWeaponGripLeftHandScrp
                   , this.player);
        this.secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
                   () => (this.isWeaponGripConstraitEnable || this.isEnableIK)
                   && player._currentWeapon != null
                   && player._currentWeapon is SecondaryWeapon
                   , this.rightHandIKConstriantManager.GetTargetHandTransform()
                   , this.leftHandConstraintManager.GetTargetHandTransform()
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
        //this.leftHandConstraintNodeSelector.AddtoChildNode(this.lowReadyWeaponLeftHandTwoBoneIKNodeLeaf);
        this.leftHandConstraintNodeSelector.AddtoChildNode(this.leftHandConstriantRestNodeLeaf);

        this.leftHandWeightConstraintSelector.AddtoChildNode(this.leftHandEnableWeightConstraintNodeLeaf);
        this.leftHandWeightConstraintSelector.AddtoChildNode(this.leftHandDisableWeightConstraintNodeLeaf);

        this.leftHandConstraintAnimationNodeComponentManager.AddNode(this.leftHandConstraintNodeSelector);
        this.leftHandConstraintAnimationNodeComponentManager.AddNode(this.leftHandWeightConstraintSelector);

    }
    #endregion

    #region LegsConstraint
    public NodeSelector legsConstrainSelector;
    public NodeSelector legConstraintWeightSelector;


    public ProneLegsConstrainNodeLeaf proneLegsConstrainNodeLeaf;
    public RestNodeLeaf legRestConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf legsEnableWeightConstraintNodeLeaf { get; set; }
    public SetConstraintWeightNodeLeaf legsDisableWeightConstraintNodeLeaf { get; set; }

    private void InitializedLegsConstrainNodeManager()
    {
        //1
        this.legsConstrainSelector = new NodeSelector(() => true);
        this.legConstraintWeightSelector = new NodeSelector(() => true);

        //2
      
        this.proneLegsConstrainNodeLeaf = new ProneLegsConstrainNodeLeaf(this.legsConstraintManager
            ,this.player.humanoidBone.hips
            ,this.player.humanoidBone.hips
            , this.diveStallLegsBlendingConstrainSCRP
            ,()=>
            this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
            );

        this.legRestConstrainNodeLeaf = new RestNodeLeaf(()=> true);

        this.legsEnableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () =>
           this.legsConstrainSelector.curNodeLeaf is RestNodeLeaf == false
            , this.legsConstraintManager
            , 1
            , 1);

        this.legsDisableWeightConstraintNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true
            , this.legsConstraintManager
            , 10
            , 0);

        this.legsConstrainSelector.AddtoChildNode(this.proneLegsConstrainNodeLeaf);
        this.legsConstrainSelector.AddtoChildNode(this.legRestConstrainNodeLeaf);

        this.legConstraintWeightSelector.AddtoChildNode(this.legsEnableWeightConstraintNodeLeaf);
        this.legConstraintWeightSelector.AddtoChildNode(this.legsDisableWeightConstraintNodeLeaf);

        this.legsConstraintAnimationNodeComponentManager.AddNode(this.legsConstrainSelector);
        this.legsConstraintAnimationNodeComponentManager.AddNode(this.legConstraintWeightSelector);
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
            this.playerStateManager.GetCurNodeLeaf() is OCM_Execute_Single_NodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is OCM_Hit_NodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerDodgeRollStateNodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerBrounceOffNodeLeaf
            || this.playerStateManager.GetCurNodeLeaf() is PlayerGetUpStateNodeLeaf
            ) == false
            &&
            (this.playerStateManager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf
            && (humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter || humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Exit)
            ) == false
            &&
            (this.playerStateManager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf
            && (restrainGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter || restrainGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
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
        this.legsConstraintAnimationNodeComponentManager = new NodeComponentManager();
        this.headConstraintAnimationNodeComponentManager = new NodeComponentManager();

        this.InitializedConstraintWeightManager();
        this.InitializedSplineLook();
        this.InitializedLeanNodeManager();
        this.InitialzedRightHandNodeManager();
        this.InitializedLeftHandNodeManager();
        this.InitializedLegsConstrainNodeManager();
        this.InitializedHeadLookConstriant();

    }

    private void Update()
    {
        this.UpdateConstrainLookReferencePos();
        this.UpdateBlackBorad();

        this.playeBodyConstriantAnimationNodeComponentManager.Update();
        this.rightHandConstraintAnimationNodeComponentManager.Update();
        this.leftHandConstraintAnimationNodeComponentManager.Update();
        this.legsConstraintAnimationNodeComponentManager.Update();
        this.headConstraintAnimationNodeComponentManager.Update();
    }
    protected void FixedUpdate()
    {

        this.playeBodyConstriantAnimationNodeComponentManager.FixedUpdate();
        this.rightHandConstraintAnimationNodeComponentManager.FixedUpdate();
        this.leftHandConstraintAnimationNodeComponentManager.FixedUpdate();
        this.legsConstraintAnimationNodeComponentManager.FixedUpdate();
        this.headConstraintAnimationNodeComponentManager.FixedUpdate();


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
    [Range(1,10)]
    [SerializeField] float maxCastDistacne;
    [Range(1, 10)]
    [SerializeField] float minCastDisTance;
    float distanceCast = 5;
    [SerializeField] private LayerMask castCollideMask;
    private void UpdateConstrainLookReferencePos()
    {
        Ray ray = new Ray(this.player.cinemachineCamera.targetPos, this.player.cinemachineCamera.targetDir);
        Vector3 hitpos;

        if(Physics.Raycast(ray,out RaycastHit hitInfo,this.maxCastDistacne, this.castCollideMask,QueryTriggerInteraction.Ignore))
        {
            float castHitDistance = Vector3.Distance(this.player.cinemachineCamera.targetPos, hitInfo.point);
            this.distanceCast = Mathf.Clamp(
                Mathf.Lerp(this.distanceCast, castHitDistance, Time.deltaTime * 10)
                , this.minCastDisTance
                , this.maxCastDistacne
                ); 
        }
        else
            this.distanceCast = Mathf.Clamp(
               Mathf.Lerp(this.distanceCast, maxCastDistacne, Time.deltaTime * 10)
               , this.minCastDisTance
               , this.maxCastDistacne
               );

        hitpos = ray.GetPoint(this.distanceCast);

        this.aimConstrainPositionReference.transform.position = Vector3.Lerp
            (
            this.aimConstrainPositionReference.position
            , Vector3.Lerp(this.aimConstrainPositionReference.position,hitpos, Time.deltaTime * 60
            )
            , Time.deltaTime * 60
            );
    }

    private void UpdateBlackBorad()
    {
        if (this.proneLegsConstrainNodeLeaf != null)
            this.proneLegsConstrainNodeLeaf.SetAngle(this.playerAnimationManager.angleLookHorizontal);

        if (this.isProne 
            )
        {
            this.rightHand_AimDownSight_Prone_PrimaryWeapon_SCRP.SetWeight(this.playerAnimationManager.angleLookHorizontal);
            this.rightHand_AimDownSight_Prone_SecondaryWeapon_SCRP.SetWeight(this.playerAnimationManager.angleLookHorizontal);
            this.body_ADS_Prone_Constrain_SCRP.SetWeight(this.playerAnimationManager.angleLookHorizontal);
        }

        if(this.rightHandConstriantSelector.curNodeLeaf is WeaponUserAimAtHandIKConstriantNodeLeaf)
        {
            this.rightHandEnableWeightConstraintNodeLeaf.SetWeight(this.player._weaponManuverManager.aimingWeight);
        }
    }


    #endregion
}
