using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager,IObserverEnemy
{

    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public BodyLookConstrain bodyLookConstrain;
    public HandArmIKConstraintManager leftHandIKConstraint;
    public HandArmIKConstraintManager rightHandIKConstraint;

    public string curNodeName;
    [SerializeField] private AimBodyConstrainScriptableObject painStateBodyConstraintSCRP;
    [SerializeField] private AimBodyConstrainScriptableObject primaryAimSplineLookConstrainScriptableObject;
    [SerializeField] private AimBodyConstrainScriptableObject secondaryAimSplineLookConstrainScriptableObject;

    [SerializeField] private TransformOffsetSCRP holdPainPointTransformSCRP;
    #region PainStateWalk
    [SerializeField, TextArea] public string enemyProceduralAnimateNodeManagerDebug;

    [Range(0, 10)]
    public float hipLegSpace;

    [Range(0, 10)]
    public float StepHeight;

    [Range(0, 10)]
    public float StepDistacne;

    [Range(0, 100)]
    public float StepVelocity;

    [Range(0, 10)]
    public float FootstepPlacementOffsetDistance;
    #endregion

    [SerializeField] private Rig rig;

    [SerializeField] private AnimationCurve painBodyRespondCurve;
    [SerializeField] private AnimationCurve painArmRespondCurve;
    public NodeComponentManager enemyConstraintAnimationNodeManager;
    public NodeComponentManager enemyConstraintWeightNodeComponentManager;

    #region BodyConstraintNode
    public NodeSelector bodyConstraintSelector;
    public PainStateProceduralBodyConstraintNodeLeaf painStateProceduralBodyConstraintNodeLeaf;
    public NodeSelector aimDownSightBodyNodeSelector;
    public AimDownSightBodyConstrainNodeLeaf primaryAnimationConstrainNodeLeaf;
    public AimDownSightBodyConstrainNodeLeaf secondaryAnimationConstrainNodeLeaf;
    #endregion

    #region ArmConstraintNodeLeaf
    public NodeSelector leftArmConstraintSelector;
    public ArmHoldPainPointConstraintNodeLeaf armHoldPainPointConstraintNodeLeaf;
    public ArmFlickPainStateConstraintNodeLeaf leftArmFlickPainStateConstraintNodeLeaf;

    public NodeSelector rightArmConstraintSelector;
    #endregion



    public PainStateWalkProceduralAnimateNodeLeaf painStateWalkProceduralAnimateNodeLeaf;

    public void InitailizedNode()
    {
        this.enemyConstraintAnimationNodeManager = new NodeComponentManager();

        #region BodyConstraint

        this.bodyConstraintSelector = new NodeSelector(() => isBodyConstriantEnable);

        this.painStateProceduralBodyConstraintNodeLeaf = new PainStateProceduralBodyConstraintNodeLeaf(
           this.enemy.transform
           , this.bodyLookConstrain
           , this.painBodyRespondCurve
           , this.painStateBodyConstraintSCRP
           , () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           );

        this.aimDownSightBodyNodeSelector = new NodeSelector(
            () => enemy._currentWeapon != null && enemy._weaponManuverManager.aimingWeight > 0
            );

        this.primaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            , bodyLookConstrain
            , primaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is PrimaryWeapon
            );

        this.secondaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            , bodyLookConstrain
            , secondaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is SecondaryWeapon
            );

        #endregion

        #region ArmsConstraint

        this.leftArmConstraintSelector = new NodeSelector(
            () => isLeftArmConstraintEnable
            );

        this.armHoldPainPointConstraintNodeLeaf = new ArmHoldPainPointConstraintNodeLeaf(
            this.leftHandIKConstraint
            , this.enemy._spine_1_Bone
            , () => this.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            ,this.holdPainPointTransformSCRP);
        this.leftArmFlickPainStateConstraintNodeLeaf = new ArmFlickPainStateConstraintNodeLeaf
            (this.leftHandIKConstraint
            , this.enemy._spine_1_Bone
            , () => this.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            , this.holdPainPointTransformSCRP
            , this.painArmRespondCurve
            , new Vector3(0,-90,0)
            );

        this.rightArmConstraintSelector = new NodeSelector(
            () => isRightArmConstraintEnable
            );

        #endregion

        #region LegsConstraint

        painStateWalkProceduralAnimateNodeLeaf = new PainStateWalkProceduralAnimateNodeLeaf(this,
            () =>
            {
                return (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>();
            }
            );

        #endregion


        enemyConstraintAnimationNodeManager.AddNode(bodyConstraintSelector);
        enemyConstraintAnimationNodeManager.AddNode(leftArmConstraintSelector);
        //enemyConstraintAnimationNodeManager.AddNode(rightArmConstraintSelector);
        enemyConstraintAnimationNodeManager.AddNode(painStateWalkProceduralAnimateNodeLeaf);

        bodyConstraintSelector.AddtoChildNode(painStateProceduralBodyConstraintNodeLeaf);
        bodyConstraintSelector.AddtoChildNode(aimDownSightBodyNodeSelector);

        leftArmConstraintSelector.AddtoChildNode(leftArmFlickPainStateConstraintNodeLeaf);

        aimDownSightBodyNodeSelector.AddtoChildNode(primaryAnimationConstrainNodeLeaf);
        aimDownSightBodyNodeSelector.AddtoChildNode(secondaryAnimationConstrainNodeLeaf);

    }

    public NodeSelector leftArmConstraintWeightSelector;
    public SetConstraintWeightNodeLeaf enableLeftArmConstraintWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf disableLeftArmConstraintWeightNodeLeaf;

    public NodeSelector rightArmConstraintWeightSelector;
    public SetConstraintWeightNodeLeaf enableRightArmConstraintWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf disableRightArmConstraintWeightNodeLeaf;

    public RecoveryConstraintManagerWeightNodeLeaf recoveryBodyConstraintManagerWeightNodeLeaf;
    public void InitializedConstraintWeightNode()
    {
        this.enemyConstraintWeightNodeComponentManager = new NodeComponentManager();

        this.leftArmConstraintWeightSelector = new NodeSelector(()=>true);
        this.enableLeftArmConstraintWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            ()=> this.isLeftArmConstraintEnable
            ,this.leftHandIKConstraint
            ,5
            ,1);
        this.disableLeftArmConstraintWeightNodeLeaf = new SetConstraintWeightNodeLeaf(
            ()=> true
            ,this.leftHandIKConstraint
            ,5
            ,0);

        this.recoveryBodyConstraintManagerWeightNodeLeaf = new RecoveryConstraintManagerWeightNodeLeaf(
            ()=> isBodyConstriantEnable == false
            ,this.bodyLookConstrain
            ,1);

        this.enemyConstraintWeightNodeComponentManager.AddNode(this.leftArmConstraintWeightSelector);

        this.leftArmConstraintWeightSelector.AddtoChildNode(this.enableLeftArmConstraintWeightNodeLeaf);
        this.leftArmConstraintWeightSelector.AddtoChildNode(this.disableLeftArmConstraintWeightNodeLeaf);

        this.enemyConstraintWeightNodeComponentManager.AddNode(this.recoveryBodyConstraintManagerWeightNodeLeaf);
    }

    public override void Initialized()
    {
        this.enemy.AddObserver(this);
        this.InitailizedNode();
        this.InitializedConstraintWeightNode();
    }

    protected void Update()
    {
        this.enemyConstraintAnimationNodeManager.Update();
        this.enemyConstraintWeightNodeComponentManager.Update();
    }
    protected void FixedUpdate()
    {
        this.enemyConstraintAnimationNodeManager.FixedUpdate();
        this.enemyConstraintWeightNodeComponentManager.FixedUpdate();
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
            //Gizmos.color = Color.red;
            //Gizmos.DrawSphere(armHoldPainPointConstraintNodeLeaf.painPoint, .05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.leftArmFlickPainStateConstraintNodeLeaf.pullPoint, .05f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(this.leftArmFlickPainStateConstraintNodeLeaf.middlePullPoint, 0.05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.leftArmFlickPainStateConstraintNodeLeaf.balancePointComponent.balancePointLookAt, 0.05f);
        }
        catch { }

    }

    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyBodyBulletDamageAbleBehavior.CharacterHitedEventDetail bulletHitDetail)
        {
            Vector3 hitPos = bulletHitDetail.hitPos;
            this.painStateProceduralBodyConstraintNodeLeaf.SetPainProperties
                (hitPos
                , bulletHitDetail.hitDir
                ,enemy.getPosturePainPhase == Enemy.EnemyPosturePainStatePhase.Flinch?.5f:1f
                );

            this.armHoldPainPointConstraintNodeLeaf.SetPainPoint(hitPos);
            this.leftArmFlickPainStateConstraintNodeLeaf.SetFlickProperties(hitPos,bulletHitDetail.hitDir);
        }
    }
}
