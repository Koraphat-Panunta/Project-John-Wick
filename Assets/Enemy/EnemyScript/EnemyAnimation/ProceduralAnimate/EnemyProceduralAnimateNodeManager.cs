using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyProceduralAnimateNodeManager : MonoBehaviour,INodeManager
{
    public INodeLeaf curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }

    public Transform centre;
    public TwoBoneIKConstraint leftLeg;
    public TwoBoneIKConstraint rightLeg;
    public Enemy enemy;

    public string curNodeName;

    private void Awake()
    {
        Debug.Log("EnemyProceduralAnimateNodeManager Awake");
        nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }
  

    // Update is called once per frame
    private void Update()
    {
        curNodeName = curNodeLeaf.ToString();
        UpdateNode();
    }
    private void FixedUpdate()
    {
        FixedUpdateNode();
    }

    [Range(0, 10)]
    public float hipLegSpace;

    [Range(0, 10)]
    public float StepHeight;

    [Range(0,10)]
    public float StepDistacne;

    [Range(0,100)]
    public float StepVelocity;

    [Range(0, 10)]
    public float FootstepPlacementOffsetDistance;

    EnemyPainStateProceduralAnimateNodeLeaf enemyPainStateProceduralAnimateNodeLeaf { get; set; }
    RestProceduralAnimateNodeLeaf restProceduralAnimateNodeLeaf { get; set; }

    public void InitailizedNode()
    {
        startNodeSelector = new SelectorProceduralAnimateNode(() => true);

        enemyPainStateProceduralAnimateNodeLeaf = new EnemyPainStateProceduralAnimateNodeLeaf(this, 
            () => 
            {
                return enemy.enemyStateManagerNode.curNodeLeaf is EnemyPainStateNodeLeaf && enemy._posture<= enemy._postureMedium;
            }
            );
        restProceduralAnimateNodeLeaf = new RestProceduralAnimateNodeLeaf(() => true);

        startNodeSelector.AddtoChildNode( enemyPainStateProceduralAnimateNodeLeaf);
        startNodeSelector.AddtoChildNode( restProceduralAnimateNodeLeaf );

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }

   
    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);
    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centre.position + (centre.right * hipLegSpace), 0.05f);
        Gizmos.DrawWireSphere(centre.position - (centre.right * hipLegSpace), 0.05f);
    }

}
