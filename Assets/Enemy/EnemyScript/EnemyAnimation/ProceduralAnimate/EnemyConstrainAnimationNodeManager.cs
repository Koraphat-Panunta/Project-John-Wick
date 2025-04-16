using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyConstrainAnimationNodeManager : AnimationConstrainManager
{
    public override INodeLeaf curNodeLeaf { get; set; }
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

    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(() => true);

        Debug.Log("enemy.enemyStateManagerNode.curNodeLeaf = " + enemy.enemyStateManagerNode.curNodeLeaf);

        enemyPainStateProceduralAnimateNodeLeaf = new EnemyPainStateProceduralAnimateNodeLeaf(this,
            () =>
            {
                return enemy.enemyStateManagerNode.curNodeLeaf is EnemyPainStateNodeLeaf && enemy._posture <= enemy._postureLight;
            }
            );
        restProceduralAnimateNodeLeaf = new RestAnimationConstrainNodeLeaf(() => true);

        startNodeSelector.AddtoChildNode(enemyPainStateProceduralAnimateNodeLeaf);
        startNodeSelector.AddtoChildNode(restProceduralAnimateNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);
    }
}
