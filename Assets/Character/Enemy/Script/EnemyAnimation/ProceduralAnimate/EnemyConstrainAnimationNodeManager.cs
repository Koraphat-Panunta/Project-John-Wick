using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager
{
    public override INodeSelector startNodeSelector { get; set; }
    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public SplineLookConstrain splineLookConstrain;
    public string curNodeName;
    [SerializeField] private AimSplineLookConstrainScriptableObject primaryAimSplineLookConstrainScriptableObject;
    [SerializeField] private AimSplineLookConstrainScriptableObject secondaryAimSplineLookConstrainScriptableObject;
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

    NodeCombine nodeCombineConstrainAnimationNodeLeaf;
    NodeSelector aimNodeSelector;
    AimDownSightAnimationConstrainNodeLeaf primaryAnimationConstrainNodeLeaf;
    AimDownSightAnimationConstrainNodeLeaf secondaryAnimationConstrainNodeLeaf;
    EnemyPainStateProceduralAnimateNodeLeaf enemyPainStateProceduralAnimateNodeLeaf { get; set; }
    RestAnimationConstrainNodeLeaf restProceduralAnimateNodeLeaf { get; set; }

    [SerializeField] private Rig rig;

    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(() => true);

        nodeCombineConstrainAnimationNodeLeaf = new NodeCombine(()=> true);
        aimNodeSelector = new NodeSelector(()=> enemy._currentWeapon != null && enemy._weaponManuverManager.aimingWeight > 0);
        primaryAnimationConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            enemy
            ,splineLookConstrain
            ,primaryAimSplineLookConstrainScriptableObject
            ,()=> enemy._currentWeapon is PrimaryWeapon);
        secondaryAnimationConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(
            enemy
            , splineLookConstrain
            , secondaryAimSplineLookConstrainScriptableObject
            , () => enemy._currentWeapon is SecondaryWeapon);

        enemyPainStateProceduralAnimateNodeLeaf = new EnemyPainStateProceduralAnimateNodeLeaf(this,
            () =>
            {
                return (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>() && enemy._posture <= enemy._postureLight;
            }
            );
        restProceduralAnimateNodeLeaf = new RestAnimationConstrainNodeLeaf(rig, () => true);

        startNodeSelector.AddtoChildNode(nodeCombineConstrainAnimationNodeLeaf);
        startNodeSelector.AddtoChildNode(restProceduralAnimateNodeLeaf);

        nodeCombineConstrainAnimationNodeLeaf.AddCombineNode(aimNodeSelector);
        nodeCombineConstrainAnimationNodeLeaf.AddCombineNode(enemyPainStateProceduralAnimateNodeLeaf);

        aimNodeSelector.AddtoChildNode(primaryAnimationConstrainNodeLeaf);
        aimNodeSelector.AddtoChildNode(secondaryAnimationConstrainNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);

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

    }
}
