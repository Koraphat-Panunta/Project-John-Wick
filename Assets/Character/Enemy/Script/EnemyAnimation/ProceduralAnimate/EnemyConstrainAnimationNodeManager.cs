using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyConstrainAnimationNodeManager : AnimationConstrainNodeManager
{
    public override INodeSelector startNodeSelector { get; set; }


    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public string curNodeName;

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

    EnemyPainStateProceduralAnimateNodeLeaf enemyPainStateProceduralAnimateNodeLeaf { get; set; }
    RestAnimationConstrainNodeLeaf restProceduralAnimateNodeLeaf { get; set; }

    [SerializeField] private Rig rig;

    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(() => true);

        enemyPainStateProceduralAnimateNodeLeaf = new EnemyPainStateProceduralAnimateNodeLeaf(this,
            () =>
            {
                return true;
                return (enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>() && enemy._posture <= enemy._postureLight;
            }
            );
        restProceduralAnimateNodeLeaf = new RestAnimationConstrainNodeLeaf(rig, () => true);

        startNodeSelector.AddtoChildNode(enemyPainStateProceduralAnimateNodeLeaf);
        startNodeSelector.AddtoChildNode(restProceduralAnimateNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.newLeftFootPos,0.15f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(enemyPainStateProceduralAnimateNodeLeaf.newRightFootPos, 0.15f);

    }
}
