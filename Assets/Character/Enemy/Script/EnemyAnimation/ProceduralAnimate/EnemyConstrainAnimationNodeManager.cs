using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;

public partial class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager, IObserverEnemy
{
    public Enemy enemy;
    public string curNodeName;

    public NodeComponentManager enemyBodyConstraintAnimationNodeManager;

    #region BodyConstraintNode
    public NodeSelector bodyConstraintSelector;
    public PainStateProceduralBodyConstraintNodeLeaf painStateProceduralBodyConstraintNodeLeaf;

    public AimDownSightBodyRotationConstraintNodeLeaf bodyLookConstraintNodeLeaf;
    public RecoveryConstraintManagerWeightNodeLeaf splineLookConstraintRecoveryWeightConstraintNodeLeaf;

    private void InitializedBodyConstrainNode()
    {
        this.enemyBodyConstraintAnimationNodeManager = new NodeComponentManager();

        this.bodyConstraintSelector = new NodeSelector(() => true);

        this.painStateProceduralBodyConstraintNodeLeaf = new PainStateProceduralBodyConstraintNodeLeaf(
            this.enemy.transform
            , this.bodyRotateConstraintManager
            , this.painBodyRespondCurve
            , this.painStateBodyConstraintSCRP
            , () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            );

        this.bodyLookConstraintNodeLeaf = new AimDownSightBodyRotationConstraintNodeLeaf(
            this.enemy.humanoidBone.hips
            , this.enemy.humanoidBone.hips
            , this.enemy.pointingTransform
            , this.enemy
            , this.bodyRotateConstraintManager
            , this.primaryAimBodyRotationConstrainSCRP
            , () => this.enemy._currentWeapon != null
                 && this.enemy._weaponManuverManager.aimingWeight > 0
                 && (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()
                 && (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>() == false
            );

        this.splineLookConstraintRecoveryWeightConstraintNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            () => true
            , this.bodyRotateConstraintManager
            , 10);

        this.bodyConstraintSelector.AddtoChildNode(this.painStateProceduralBodyConstraintNodeLeaf);
        this.bodyConstraintSelector.AddtoChildNode(this.bodyLookConstraintNodeLeaf);
        this.bodyConstraintSelector.AddtoChildNode(this.splineLookConstraintRecoveryWeightConstraintNodeLeaf);

        this.enemyBodyConstraintAnimationNodeManager.AddNode(this.bodyConstraintSelector);
    }

    #endregion

    #region RightArmConstrainNodeLeaf
    public NodeComponentManager rightArmNodeComponentManager;

    public NodeSelector rightArmConstraintSelector;
    public ArmPrceduralPainStateConstraintNodeLeaf rightArmPainStateProceduralConstraintNodeLeaf;
    public WeaponUserAimAtHandIKConstriantNodeLeaf rightHandWeaponAimAtIKCinstrainNodeLeaf;
    public RestNodeLeaf restRightArmConstrainNodeLeaf;

    public NodeSelector rightArmWeightConstrainSelector;
    public SetConstraintWeightNodeLeaf enableRightArmWeightConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf disableRightArmWeightConstrainNodeLeaf;

    private void InitializedRightArmConstrainNode()
    {
        this.rightArmNodeComponentManager = new NodeComponentManager();

        //1
        this.rightArmConstraintSelector = new NodeSelector(
            () => this.isRightArmConstraintEnable
            );
        this.rightArmWeightConstrainSelector = new NodeSelector(
            () => true
            );

        //2
        this.rightArmPainStateProceduralConstraintNodeLeaf = new ArmPrceduralPainStateConstraintNodeLeaf
            (this.rightHandIKConstraint
            , this.enemy.humanoidBone._spine_1_Bone
            , () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            //|| this.enemy.stateManagerNode.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>()
            , this.armAnchorSwingOffsetPosition
            , this.armBalancePointOffset
            , new Vector3(0, 90, 0)
            );

        this.restRightArmConstrainNodeLeaf = new RestNodeLeaf(() => true);

        this.rightHandWeaponAimAtIKCinstrainNodeLeaf = new WeaponUserAimAtHandIKConstriantNodeLeaf(
            this.rightHandIKConstraint
            , this.enemy.pointingTransform
            , this.enemy.humanoidBone._rightArmBone
            , this.enemy.humanoidBone._spine_2_Bone
            , this.enemy.humanoidBone._spine_2_Bone
            , this.enemy.transform
            , this.enemy
            , this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP
            , this.pistolHandRecoilData
            , this.secondaryWeaponBlockData
            , () => this.enemy._currentWeapon != null
               && this.enemy._weaponManuverManager.aimingWeight > 0 
               && (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()
               && (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>() == false
            );

        this.enableRightArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (()=> this.rightArmConstraintSelector.curNodeLeaf != this.restRightArmConstrainNodeLeaf && this.isRightArmConstraintEnable
            ,this.rightHandIKConstraint
            ,1,1);

        this.disableRightArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => true
            , this.rightHandIKConstraint
            , 1, 0);

        //1
        this.rightArmConstraintSelector.AddtoChildNode(this.rightArmPainStateProceduralConstraintNodeLeaf);
        this.rightArmConstraintSelector.AddtoChildNode(this.rightHandWeaponAimAtIKCinstrainNodeLeaf);
        this.rightArmConstraintSelector.AddtoChildNode(this.restRightArmConstrainNodeLeaf);

        this.rightArmWeightConstrainSelector.AddtoChildNode(this.enableRightArmWeightConstrainNodeLeaf);
        this.rightArmWeightConstrainSelector.AddtoChildNode(this.disableRightArmWeightConstrainNodeLeaf);

        this.rightArmNodeComponentManager.AddNode(this.rightArmConstraintSelector);
        this.rightArmNodeComponentManager.AddNode(this.rightArmWeightConstrainSelector);
    }


    #endregion

    #region LeftArmConstrainNodeLeaf

    public NodeComponentManager leftArmNodeComponentManager;

    public NodeSelector leftArmConstraintSelector;
    public ArmPrceduralPainStateConstraintNodeLeaf leftArmPainStateProceduralConstraintNodeLeaf;
    public WeaponLeftHandGripHandConstraintNodeLeaf primaryWeaponGripLeftHandTwoBoneIKNodeLeaf;
    public WeaponLeftHandGripHandConstraintNodeLeaf secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf;
    public RestNodeLeaf restLeftArmConstrainNodeLeaf;

    public NodeSelector leftArmWeightConstrainSelector;
    public SetConstraintWeightNodeLeaf enableLeftArmWeightConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf disableLeftArmWeightConstrainNodeLeaf;

    private void InitializedLeftArmConstrainNode()
    {
        this.leftArmNodeComponentManager = new NodeComponentManager();

        //1
        this.leftArmConstraintSelector = new NodeSelector(
            () => this.isLeftArmConstraintEnable
            );
        this.leftArmWeightConstrainSelector = new NodeSelector(
            () => true
            );

        //2
        this.leftArmPainStateProceduralConstraintNodeLeaf = new ArmPrceduralPainStateConstraintNodeLeaf
           (this.leftHandIKConstraint
           , this.enemy.humanoidBone._spine_1_Bone
           , () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           //|| this.enemy.stateManagerNode.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>()
           , this.armAnchorSwingOffsetPosition
           , this.armBalancePointOffset
           , new Vector3(0, -90, 0)
           );

        this.primaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
            () => this.isWeaponGripConstraintEnable && enemy._currentWeapon is PrimaryWeapon,
            this.rightHandIKConstraint.GetTargetHandTransform(),
            this.leftHandIKConstraint.GetTargetHandTransform(),
            this.leftHandIKConstraint,
            this.primaryWeaponGripLeftHandScrp,
            this.enemy);

        this.secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf = new WeaponLeftHandGripHandConstraintNodeLeaf(
            () => this.isWeaponGripConstraintEnable && enemy._currentWeapon is SecondaryWeapon,
            this.rightHandIKConstraint.GetTargetHandTransform(),
            this.leftHandIKConstraint.GetTargetHandTransform(),
            this.leftHandIKConstraint,
            this.secondaryWeaponGripLeftHandScrp,
            this.enemy);

        this.restLeftArmConstrainNodeLeaf = new RestNodeLeaf(() => true);

        this.enableLeftArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => this.leftArmConstraintSelector.curNodeLeaf != this.restLeftArmConstrainNodeLeaf && this.isLeftArmConstraintEnable
            , this.leftHandIKConstraint
            , 1, 1);

        this.disableLeftArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => true
            , this.leftHandIKConstraint
            , 1, 0);

        //1
        this.leftArmConstraintSelector.AddtoChildNode(this.leftArmPainStateProceduralConstraintNodeLeaf);
        this.leftArmConstraintSelector.AddtoChildNode(this.primaryWeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftArmConstraintSelector.AddtoChildNode(this.secondaryWeaponGripLeftHandTwoBoneIKNodeLeaf);
        this.leftArmConstraintSelector.AddtoChildNode(this.restLeftArmConstrainNodeLeaf);

        this.leftArmWeightConstrainSelector.AddtoChildNode(this.enableLeftArmWeightConstrainNodeLeaf);
        this.leftArmWeightConstrainSelector.AddtoChildNode(this.disableLeftArmWeightConstrainNodeLeaf);

        this.leftArmNodeComponentManager.AddNode(this.leftArmConstraintSelector);
        this.leftArmNodeComponentManager.AddNode(this.leftArmWeightConstrainSelector);
    }

    #endregion

    #region LegsConstrain

    public NodeComponentManager legsNodeComponentManager;

    public NodeSelector legsConstrainSelector;
    public NodeSelector legsWeightConstrainSelector;

    public PainStateWalkProceduralAnimateNodeLeaf painStateWalkProceduralAnimateNodeLeaf;
    public RestNodeLeaf restLegsConstrainNodeLeaf;

    public SetConstraintWeightNodeLeaf enableLegsWeightConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf disableLegsWeightConstrainNodeLeaf;

    private void InitializedLegsConstrainNode()
    {
        this.legsNodeComponentManager = new NodeComponentManager();

        //1
        this.legsConstrainSelector = new NodeSelector(()=>true);
        this.legsWeightConstrainSelector = new NodeSelector(() => true);

        //2
        this.painStateWalkProceduralAnimateNodeLeaf = new PainStateWalkProceduralAnimateNodeLeaf(
            this.legsConstrainManager
            ,this.enemy.transform
            ,this.proceduralLegsPainStateWalkConstrainSCRP
            , () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           || this.enemy.stateManagerNode.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>());

        this.restLegsConstrainNodeLeaf = new RestNodeLeaf(() => true);

        this.enableLegsWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.legsConstrainSelector.curNodeLeaf != this.restLegsConstrainNodeLeaf
            , this.legsConstrainManager
            , 1, 1);
        this.disableLegsWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true
            , this.legsConstrainManager
            , 1, 0);


        this.legsConstrainSelector.AddtoChildNode(this.painStateWalkProceduralAnimateNodeLeaf);
        this.legsConstrainSelector.AddtoChildNode(this.restLegsConstrainNodeLeaf);

        this.legsWeightConstrainSelector.AddtoChildNode(this.enableLegsWeightConstrainNodeLeaf);
        this.legsWeightConstrainSelector.AddtoChildNode(this.disableLegsWeightConstrainNodeLeaf);

        this.legsNodeComponentManager.AddNode(this.legsConstrainSelector);
        this.legsNodeComponentManager.AddNode(this.legsWeightConstrainSelector);
    }

    #endregion


    #region HeadConstrainNode
    public NodeComponentManager headNodeComponentManager;

    public NodeSelector headLookNodeSelector;
    public HeadLookConstrainAnimationNodeLeaf headLookAtWeaponConstraintNodeLeaf;
    public HeadLookConstrainAnimationNodeLeaf headLookPointingPosConstrainNodeLeaf;
    public RestNodeLeaf headConstraintRestNodeLeaf;

    public NodeSelector headWeightConstraintSelector;
    public SetConstraintWeightNodeLeaf headEnableConstraintWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf headLookRecoveryConstraintManagerWeightNodeLeaf;

    private void InitializedHeadConstrainNode()
    {
        this.headNodeComponentManager = new NodeComponentManager();

        this.headLookNodeSelector = new NodeSelector(() => true);
        this.headWeightConstraintSelector = new NodeSelector(() => true);

        this.headLookAtWeaponConstraintNodeLeaf = new HeadLookConstrainAnimationNodeLeaf(
            this.headRotateConstraintManager,
            this.enemy._mainHandSocket.transform,
            () => (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>());

        this.headLookPointingPosConstrainNodeLeaf = new HeadLookConstrainAnimationNodeLeaf(
            this.headRotateConstraintManager,
            this.enemy.pointingTransform,
            () => this.enemy._currentWeapon != null
               && (this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                   || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                   || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                   || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>())
               && (this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>() == false);

        this.headConstraintRestNodeLeaf = new RestNodeLeaf(() => true);

        this.headEnableConstraintWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => this.headLookNodeSelector.curNodeLeaf != this.headConstraintRestNodeLeaf,
            this.headRotateConstraintManager,
            5, 1);

        this.headLookRecoveryConstraintManagerWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            () => true,
            this.headRotateConstraintManager,
            5, 0);

        this.headLookNodeSelector.AddtoChildNode(this.headLookAtWeaponConstraintNodeLeaf);
        this.headLookNodeSelector.AddtoChildNode(this.headLookPointingPosConstrainNodeLeaf);
        this.headLookNodeSelector.AddtoChildNode(this.headConstraintRestNodeLeaf);

        this.headWeightConstraintSelector.AddtoChildNode(this.headEnableConstraintWeightNodeLeaf);
        this.headWeightConstraintSelector.AddtoChildNode(this.headLookRecoveryConstraintManagerWeightNodeLeaf);

        this.headNodeComponentManager.AddNode(this.headLookNodeSelector);
        this.headNodeComponentManager.AddNode(this.headWeightConstraintSelector);
    }
    #endregion

    public void InitailizedNode()
    {
        this.enemyBodyConstraintAnimationNodeManager = new NodeComponentManager();

        this.InitializedBodyConstrainNode();
        this.InitializedRightArmConstrainNode();
        this.InitializedLeftArmConstrainNode();
        this.InitializedLegsConstrainNode();
        this.InitializedHeadConstrainNode();
    }

   
    public override void Initialized()
    {
        this.enemy.AddObserver(this);
        this.InitailizedNode();
    }

    protected void Update()
    {
        this.enemyBodyConstraintAnimationNodeManager.Update();
        this.rightArmNodeComponentManager.Update();
        this.leftArmNodeComponentManager.Update();
        this.legsNodeComponentManager.Update();
        this.headNodeComponentManager.Update();
    }
    protected void FixedUpdate()
    {
        this.enemyBodyConstraintAnimationNodeManager.FixedUpdate();
        this.rightArmNodeComponentManager.FixedUpdate();
        this.leftArmNodeComponentManager.FixedUpdate();
        this.legsNodeComponentManager.FixedUpdate();
        this.headNodeComponentManager.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;

        //Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        //Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);

        #region DrawFootPlacementPosition

        //Gizmos.color = Color.blue * .5f;
        //Gizmos.DrawSphere(this.painStateWalkProceduralAnimateNodeLeaf.oldRightFootPos,.1f);
        //Gizmos.color = Color.red * .5f;
        //Gizmos.DrawSphere(this.painStateWalkProceduralAnimateNodeLeaf.newRightFootPos, .1f);

        #endregion

        try
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.leftArmPainStateProceduralConstraintNodeLeaf.balancePoint, .05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.leftArmPainStateProceduralConstraintNodeLeaf.painLookAtPos, .05f);

            

        }
        catch { }

    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if((this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
        {
            this.leftHandIKConstraint.SetWeight(0);
            this.rightHandIKConstraint.SetWeight(0);
            
        }

        this.Body_Look_ConstrainCondition(enemy, node);
        this.RightHand_ConstrainCondition(enemy, node);

        if (node is FiringNode)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.TriggeRecoilWeight();
            this.bodyLookConstraintNodeLeaf.TriggerRecoil();
        }

        if (this.enemy._currentWeapon is AutomaticShotgunModel)
            this.bodyLookConstraintNodeLeaf.SetRecoilScriptableObject(this.shotGun_BodyRecoil_SCRP);
        else
            this.bodyLookConstraintNodeLeaf.SetRecoilScriptableObject(null);

        if(node is GotGunFuHitNodeLeaf)
        {
            this.leftArmPainStateProceduralConstraintNodeLeaf.TriggerReset();
            this.rightArmPainStateProceduralConstraintNodeLeaf.TriggerReset();
        }

        if(node  is CharacterHitedEventDetail hitedEventDetail)
        {
            Vector3 hitPos = hitedEventDetail.hitPos;
            this.painStateProceduralBodyConstraintNodeLeaf.SetPainProperties
                (hitPos
                , hitedEventDetail.hitDir
                , enemy.getPosturePainPhase == Enemy.EnemyPosturePainStatePhase.Flinch ? .5f : 1f
                );

            Vector3 root = this.leftArmPainStateProceduralConstraintNodeLeaf.rootIKHandRef.transform.position;
            Vector3 rootToHitDir = (hitPos - this.leftArmPainStateProceduralConstraintNodeLeaf.rootIKHandRef.transform.position).normalized;

            if (hitedEventDetail.hitedPart is ArmLeftBodyPart)
            {

                this.leftArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(hitedEventDetail.hitDir + Vector3.up, 2);
            }
            if (hitedEventDetail.hitedPart is ArmRightBodyPart)
            {
               
                this.rightArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(hitedEventDetail.hitDir + Vector3.up, 2);
            }

            this.leftArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(hitedEventDetail.hitDir , hitedEventDetail.hitforce);
            this.rightArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(hitedEventDetail.hitDir , hitedEventDetail.hitforce);



        }
    }

    private void RightHand_ConstrainCondition<T>(Enemy enemy, T node)
    {
        if (this.enemy._currentWeapon == null) return;

        switch (this.enemy._currentWeapon)
        {
            case AutomaticShotgunModel:
                SetRightHandRecoilData(this.shotgunHandRecoilData);
                SetRightHandRecoilBlockData(this.primaryWeaponBlockData);
                break;
            case PrimaryWeapon:
                SetRightHandRecoilData(this.rifileHandRecoilData);
                SetRightHandRecoilBlockData(this.primaryWeaponBlockData);
                break;
            case SecondaryWeapon:
                SetRightHandRecoilData(this.pistolHandRecoilData);
                SetRightHandRecoilBlockData(this.secondaryWeaponBlockData);
                break;
        }

        if (this.enemy._currentWeapon is PrimaryWeapon)
            SetRightHandSCRP(this.rightHand_Target_AimDownSight_PrimaryWeapon_SCRP);
        else if (this.enemy._currentWeapon is SecondaryWeapon)
            SetRightHandSCRP(this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP);
    }

    private void SetRightHandSCRP(TwoBoneIK_ConstraintSCRP scrp)
    {
        if (this.rightHandWeaponAimAtIKCinstrainNodeLeaf.handIK_ConstraintSCRP != scrp)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandIKConstraintSCRP(scrp);
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }
    }

    private void SetRightHandRecoilData(WeaponHandRecoilSCRP data) =>
        this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandRecoilData(data);

    private void SetRightHandRecoilBlockData(WeaponHandBlockSCRP data) =>
        this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandBlockData(data);

    private void Body_Look_ConstrainCondition<T>(Enemy enemy, T node)
    {
        if (node is AimDownSightWeaponManuverNodeLeaf && this.enemy._currentWeapon != null)
        {
            if (this.enemy._currentWeapon is PrimaryWeapon)
                SetBodyRotationSCRP(this.primaryAimBodyRotationConstrainSCRP);
            else if (this.enemy._currentWeapon is SecondaryWeapon)
                SetBodyRotationSCRP(this.secondaryAimBodyRotationConstrainSCRP);
        }
    }

    private void SetBodyRotationSCRP(BodyRotationConstrainScriptableObject scrp)
    {
        if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject != scrp)
        {
            this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(scrp);
            this.bodyLookConstraintNodeLeaf.SetWeight(0);
        }
    }
}
