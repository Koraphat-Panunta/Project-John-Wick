using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager,IObserverEnemy
{

    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public BodyLookConstrain splineLookConstrain;
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

    public NodeSelector rightArmConstraintSelector;
    #endregion



    public PainStateWalkProceduralAnimateNodeLeaf enemyPainStateProceduralAnimateNodeLeaf;

    public void InitailizedNode()
    {
        this.enemyConstraintAnimationNodeManager = new NodeComponentManager();

        #region BodyConstraint

        this.bodyConstraintSelector = new NodeSelector(() => isBodyConstriantEnable);

        this.painStateProceduralBodyConstraintNodeLeaf = new PainStateProceduralBodyConstraintNodeLeaf(
           this.enemy.transform
           , this.splineLookConstrain
           , this.painStateBodyConstraintSCRP
           , () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
           );

        this.aimDownSightBodyNodeSelector = new NodeSelector(
            () => enemy._currentWeapon != null && enemy._weaponManuverManager.aimingWeight > 0
            );

        this.primaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            , splineLookConstrain
            , primaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is PrimaryWeapon
            );

        this.secondaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            , splineLookConstrain
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

        this.rightArmConstraintSelector = new NodeSelector(
            () => isRightArmConstraintEnable
            );

        #endregion

        #region LegsConstraint

        enemyPainStateProceduralAnimateNodeLeaf = new PainStateWalkProceduralAnimateNodeLeaf(this,
            () =>
            {
                return (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>();
            }
            );

        #endregion


        enemyConstraintAnimationNodeManager.AddNode(bodyConstraintSelector);
        enemyConstraintAnimationNodeManager.AddNode(leftArmConstraintSelector);
        //enemyConstraintAnimationNodeManager.AddNode(rightArmConstraintSelector);
        enemyConstraintAnimationNodeManager.AddNode(enemyPainStateProceduralAnimateNodeLeaf);

        bodyConstraintSelector.AddtoChildNode(painStateProceduralBodyConstraintNodeLeaf);
        bodyConstraintSelector.AddtoChildNode(aimDownSightBodyNodeSelector);

        leftArmConstraintSelector.AddtoChildNode(armHoldPainPointConstraintNodeLeaf);

        aimDownSightBodyNodeSelector.AddtoChildNode(primaryAnimationConstrainNodeLeaf);
        aimDownSightBodyNodeSelector.AddtoChildNode(secondaryAnimationConstrainNodeLeaf);

    }

    public NodeSelector leftArmConstraintWeightSelector;
    public SetConstraintWeightNodeLeaf enableLeftArmConstraintWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf disableLeftArmConstraintWeightNodeLeaf;

    public NodeSelector rightArmConstraintWeightSelector;
    public SetConstraintWeightNodeLeaf enableRightArmConstraintWeightNodeLeaf;
    public SetConstraintWeightNodeLeaf disableRightArmConstraintWeightNodeLeaf;
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

        this.enemyConstraintWeightNodeComponentManager.AddNode(this.leftArmConstraintWeightSelector);

        this.leftArmConstraintWeightSelector.AddtoChildNode(this.enableLeftArmConstraintWeightNodeLeaf);
        this.leftArmConstraintWeightSelector.AddtoChildNode(this.disableLeftArmConstraintWeightNodeLeaf);
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
        //Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.oldLeftFootPos, 0.15f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.newLeftFootPos, 0.15f);

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.oldRightFootPos, 0.15f);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.newRightFootPos, 0.15f);
        #endregion

        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(armHoldPainPointConstraintNodeLeaf.painPoint, .05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(painStateProceduralBodyConstraintNodeLeaf.pullBackPainPointPosition, .05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(painStateProceduralBodyConstraintNodeLeaf.painLookAtPos, .05f);
        }
        catch { }

    }

    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyBodyBulletDamageAbleBehavior.BulletHitDetail bulletHitDetail)
        {
            Vector3 hitPos = bulletHitDetail.hitPos;
            this.painStateProceduralBodyConstraintNodeLeaf.SetPainPointPosition
                (hitPos
                , bulletHitDetail.hitDir
                ,1
                );

            this.armHoldPainPointConstraintNodeLeaf.SetPainPoint(hitPos);
        }
    }
}
