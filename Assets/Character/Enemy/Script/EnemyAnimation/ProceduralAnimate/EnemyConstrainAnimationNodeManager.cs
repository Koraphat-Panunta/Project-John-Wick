using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager,IObserverEnemy
{

    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public BodyLookConstrain splineLookConstrain;
    public string curNodeName;
    [SerializeField] private AimBodyConstrainScriptableObject painStateBodyConstraintSCRP;
    [SerializeField] private AimBodyConstrainScriptableObject primaryAimSplineLookConstrainScriptableObject;
    [SerializeField] private AimBodyConstrainScriptableObject secondaryAimSplineLookConstrainScriptableObject;
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
    public NodeSelector bodyConstraintSelector;
    public PainStateProceduralBodyConstraintNodeLeaf painStateProceduralBodyConstraintNodeLeaf;

    public NodeSelector aimDownSightBodyNodeSelector;
    public AimDownSightBodyConstrainNodeLeaf primaryAnimationConstrainNodeLeaf;
    public AimDownSightBodyConstrainNodeLeaf secondaryAnimationConstrainNodeLeaf;

    public EnemyPainStateProceduralAnimateNodeLeaf enemyPainStateProceduralAnimateNodeLeaf;

    public void InitailizedNode()
    {
        this.enemyConstraintAnimationNodeManager = new NodeComponentManager();

        this.bodyConstraintSelector = new NodeSelector(()=> isBodyConstriantEnable);

        this.painStateProceduralBodyConstraintNodeLeaf = new PainStateProceduralBodyConstraintNodeLeaf(
            this.enemy._root
            ,this.splineLookConstrain
            ,this.painStateBodyConstraintSCRP
            , ()=> enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
            );

        aimDownSightBodyNodeSelector = new NodeSelector(()=> enemy._currentWeapon != null && enemy._weaponManuverManager.aimingWeight > 0);
        primaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            ,splineLookConstrain
            ,primaryAimSplineLookConstrainScriptableObject
            ,()=> enemy._currentWeapon is PrimaryWeapon);
        secondaryAnimationConstrainNodeLeaf = new AimDownSightBodyConstrainNodeLeaf(
            enemy
            , splineLookConstrain
            , secondaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is SecondaryWeapon);

        enemyPainStateProceduralAnimateNodeLeaf = new EnemyPainStateProceduralAnimateNodeLeaf(this,
            () =>
            {
                return (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>();
            }
            );



        enemyConstraintAnimationNodeManager.AddNode(bodyConstraintSelector);
        enemyConstraintAnimationNodeManager.AddNode(enemyPainStateProceduralAnimateNodeLeaf);

        bodyConstraintSelector.AddtoChildNode(painStateProceduralBodyConstraintNodeLeaf);
        bodyConstraintSelector.AddtoChildNode(aimDownSightBodyNodeSelector);

        aimDownSightBodyNodeSelector.AddtoChildNode(primaryAnimationConstrainNodeLeaf);
        aimDownSightBodyNodeSelector.AddtoChildNode(secondaryAnimationConstrainNodeLeaf);

    }

    public override void Initialized()
    {
        this.enemy.AddObserver(this);
        this.InitailizedNode();
    }

    protected void Update()
    {
        this.enemyConstraintAnimationNodeManager.Update();
    }
    protected void FixedUpdate()
    {
        this.enemyConstraintAnimationNodeManager.FixedUpdate();
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

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(painStateProceduralBodyConstraintNodeLeaf.painPointPosition, .15f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(painStateProceduralBodyConstraintNodeLeaf.pullBackPainPointPosition, .15f);

    }

    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyBodyBulletDamageAbleBehavior.BulletHitDetail bulletHitDetail)
        {
            this.painStateProceduralBodyConstraintNodeLeaf.SetPainPointPosition
                (bulletHitDetail.hitPos
                ,bulletHitDetail.hitDir
                ,1
                );
        }
    }
}
