using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager,IObserverEnemy
{

    public Enemy enemy;

    public BodyLookConstrainManager bodyLookConstrainManager;
    public HandArmIKConstraintManager leftHandIKConstraint;
    public HandArmIKConstraintManager rightHandIKConstraint;
    public LegsConstrainManager legsConstrainManager;

    public string curNodeName;
    [SerializeField] private AimBodyConstrainScriptableObject painStateBodyConstraintSCRP;
    [SerializeField] private AimBodyConstrainScriptableObject primaryAimSplineLookConstrainScriptableObject;
    [SerializeField] private AimBodyConstrainScriptableObject secondaryAimSplineLookConstrainScriptableObject;

    [SerializeField] private TransformOffsetSCRP armAnchorSwingOffsetPosition;
    [SerializeField] private TransformOffsetSCRP armBalancePointOffset;

    [SerializeField] private ProceduralLegsWalkConstrainSCRP proceduralLegsPainStateWalkConstrainSCRP;

    [SerializeField] private Rig rig;

    [SerializeField] private AnimationCurve painBodyRespondCurve;

    public NodeComponentManager enemyBodyConstraintAnimationNodeManager;

    #region BodyConstraintNode
    public NodeSelector bodyConstraintSelector;
    public PainStateProceduralBodyConstraintNodeLeaf painStateProceduralBodyConstraintNodeLeaf;

    public NodeSelector aimDownSightBodyNodeSelector;
    public AimDownSightBodyConstrainNodeLeaf primaryAnimationConstrainNodeLeaf;
    public AimDownSightBodyConstrainNodeLeaf secondaryAnimationConstrainNodeLeaf;

    public RestNodeLeaf restBodyConstrainNodeLeaf;

    public NodeSelector bodyWeightConstranSelector;
    public SetConstraintWeightNodeLeaf enableBodyConstrainWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf disableBodyConstrainWeightNodeLeaf;

    private void InitializedBodyConstrainNode()
    {
        this.enemyBodyConstraintAnimationNodeManager = new NodeComponentManager();

        //1
        this.bodyConstraintSelector = new NodeSelector(() => true);
        this.bodyWeightConstranSelector = new NodeSelector(() => true);

        //2
        this.painStateProceduralBodyConstraintNodeLeaf = new PainStateProceduralBodyConstraintNodeLeaf(
           this.enemy.transform
           , this.bodyLookConstrainManager
           , this.painBodyRespondCurve
           , this.painStateBodyConstraintSCRP
           , () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           );

        this.aimDownSightBodyNodeSelector = new NodeSelector(
            () => enemy._currentWeapon != null && enemy._weaponManuverManager.aimingWeight > 0
            );

        this.restBodyConstrainNodeLeaf = new RestNodeLeaf
            (() => true);

        this.enableBodyConstrainWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            ()=> this.bodyConstraintSelector.curNodeLeaf != this.restBodyConstrainNodeLeaf
            ,this.bodyLookConstrainManager
            ,1,1);

        this.disableBodyConstrainWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            ()=> true
            ,this.bodyLookConstrainManager
            ,1,0);

        //3

        this.primaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.enemy._hipBone
            , this.enemy._hipBone
            , this.enemy.pointingTransform
            , this.enemy
            , bodyLookConstrainManager
            , primaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is PrimaryWeapon
            );

        this.secondaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            this.enemy._hipBone
            , this.enemy._hipBone
            , this.enemy.pointingTransform
            , this.enemy
            , bodyLookConstrainManager
            , secondaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is SecondaryWeapon
            );
        //

        //1
        this.bodyConstraintSelector.AddtoChildNode(this.painStateProceduralBodyConstraintNodeLeaf);
        this.bodyConstraintSelector.AddtoChildNode(this.aimDownSightBodyNodeSelector);
        this.bodyConstraintSelector.AddtoChildNode(this.restBodyConstrainNodeLeaf);

        this.bodyWeightConstranSelector.AddtoChildNode(this.enableBodyConstrainWeightNodeLeaf);
        this.bodyWeightConstranSelector.AddtoChildNode(this.disableBodyConstrainWeightNodeLeaf);

        //2
        this.aimDownSightBodyNodeSelector.AddtoChildNode(this.primaryAnimationConstrainNodeLeaf);
        this.aimDownSightBodyNodeSelector.AddtoChildNode(this.secondaryAnimationConstrainNodeLeaf);

        //

        this.enemyBodyConstraintAnimationNodeManager.AddNode(this.bodyConstraintSelector);
        this.enemyBodyConstraintAnimationNodeManager.AddNode(this.bodyWeightConstranSelector);
    }

    #endregion

    #region RightArmConstrainNodeLeaf
    public NodeComponentManager rightArmNodeComponentManager;

    public NodeSelector rightArmConstraintSelector;
    public ArmPrceduralPainStateConstraintNodeLeaf rightArmPainStateProceduralConstraintNodeLeaf;
    public RestNodeLeaf restRightArmConstrainNodeLeaf;

    public NodeSelector rightArmWeightConstrainSelector;
    public SetConstraintWeightNodeLeaf enableRightArmWeightConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf disableRightArmWeightConstrainNodeLeaf;

    private void InitializedRightArmConstrainNode()
    {
        this.rightArmNodeComponentManager = new NodeComponentManager();

        //1
        this.rightArmConstraintSelector = new NodeSelector(
            () => true
            );
        this.rightArmWeightConstrainSelector = new NodeSelector(
            () => true
            );

        //2
        this.rightArmPainStateProceduralConstraintNodeLeaf = new ArmPrceduralPainStateConstraintNodeLeaf
            (this.rightHandIKConstraint
            , this.enemy._spine_1_Bone
            , () => this.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            , this.armAnchorSwingOffsetPosition
            , this.armBalancePointOffset
            , new Vector3(0, 90, 0)
            );

        this.restRightArmConstrainNodeLeaf = new RestNodeLeaf(() => true);

        this.enableRightArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (()=> this.rightArmConstraintSelector.curNodeLeaf != this.restRightArmConstrainNodeLeaf
            ,this.rightHandIKConstraint
            ,1,1);

        this.disableRightArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => true
            , this.rightHandIKConstraint
            , 1, 0);

        //1
        this.rightArmConstraintSelector.AddtoChildNode(this.rightArmPainStateProceduralConstraintNodeLeaf);
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
    public RestNodeLeaf restLeftArmConstrainNodeLeaf;

    public NodeSelector leftArmWeightConstrainSelector;
    public SetConstraintWeightNodeLeaf enableLeftArmWeightConstrainNodeLeaf;
    public SetConstraintWeightNodeLeaf disableLeftArmWeightConstrainNodeLeaf;

    private void InitializedLeftArmConstrainNode()
    {
        this.leftArmNodeComponentManager = new NodeComponentManager();

        //1
        this.leftArmConstraintSelector = new NodeSelector(
            () => true
            );
        this.leftArmWeightConstrainSelector = new NodeSelector(
            () => true
            );

        //2
        this.leftArmPainStateProceduralConstraintNodeLeaf = new ArmPrceduralPainStateConstraintNodeLeaf
           (this.leftHandIKConstraint
           , this.enemy._spine_1_Bone
           , () => this.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           , this.armAnchorSwingOffsetPosition
           , this.armBalancePointOffset
           , new Vector3(0, -90, 0)
           );

        this.restLeftArmConstrainNodeLeaf = new RestNodeLeaf(() => true);

        this.enableLeftArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => this.leftArmConstraintSelector.curNodeLeaf != this.restLeftArmConstrainNodeLeaf
            , this.leftHandIKConstraint
            , 1, 1);

        this.disableLeftArmWeightConstrainNodeLeaf = new SetConstraintWeightNodeLeaf
            (() => true
            , this.leftHandIKConstraint
            , 1, 0);

        //1
        this.leftArmConstraintSelector.AddtoChildNode(this.leftArmPainStateProceduralConstraintNodeLeaf);
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
            ,this.enemy._hipBone
            ,this.proceduralLegsPainStateWalkConstrainSCRP
            ,()=> (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>());

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


    public void InitailizedNode()
    {
        this.enemyBodyConstraintAnimationNodeManager = new NodeComponentManager();

        this.InitializedBodyConstrainNode();
        this.InitializedRightArmConstrainNode();
        this.InitializedLeftArmConstrainNode();
        this.InitializedLegsConstrainNode();



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
    }
    protected void FixedUpdate()
    {
        this.enemyBodyConstraintAnimationNodeManager.FixedUpdate();
        this.rightArmNodeComponentManager.FixedUpdate();
        this.leftArmNodeComponentManager.FixedUpdate();
        this.legsNodeComponentManager.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;

        //Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        //Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);

        #region DrawFootPlacementPosition
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(painStateWalkProceduralAnimateNodeLeaf.oldLeftFootPos, 0.15f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(painStateWalkProceduralAnimateNodeLeaf.newLeftFootPos, 0.15f);

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(painStateWalkProceduralAnimateNodeLeaf.oldRightFootPos, 0.15f);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(painStateWalkProceduralAnimateNodeLeaf.newRightFootPos, 0.15f);
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

    public void Notify<T>(Enemy enemy, T node)
    {
        if (node is EnemyBodyBulletDamageAbleBehavior.CharacterHitedEventDetail bulletHitDetail)
        {
            Vector3 hitPos = bulletHitDetail.hitPos;
            this.painStateProceduralBodyConstraintNodeLeaf.SetPainProperties
                (hitPos
                , bulletHitDetail.hitDir
                , enemy.getPosturePainPhase == Enemy.EnemyPosturePainStatePhase.Flinch ? .5f : 1f
                );

            Vector3 root = this.leftArmPainStateProceduralConstraintNodeLeaf.rootIKHandRef.transform.position;
            Vector3 rootToHitDir = (hitPos - this.leftArmPainStateProceduralConstraintNodeLeaf.rootIKHandRef.transform.position).normalized;

           if(bulletHitDetail.hitedPart is ArmLeftBodyPart)
            {
                this.leftArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(bulletHitDetail.hitDir + Vector3.up, 2);
            }
            if (bulletHitDetail.hitedPart is ArmRightBodyPart)
            {
                this.rightArmPainStateProceduralConstraintNodeLeaf.TriggerForcePush(bulletHitDetail.hitDir + Vector3.up, 2);
            }

        }
    }
}
