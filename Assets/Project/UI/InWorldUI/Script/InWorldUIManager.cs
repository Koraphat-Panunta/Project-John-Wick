using UnityEngine;
using System.Collections.Generic;
public class InWorldUIManager : MonoBehaviour,INodeManager
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private InWorldUI executeInWorldUI;
    [SerializeField] private Player player;

    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    INodeLeaf INodeManager.curNodeLeaf { get => _curNodeLeaf; set => _curNodeLeaf = value; }
    private INodeLeaf _curNodeLeaf;

    

    private void Awake()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
        this.InitailizedNode();
    }
   
    void Update()
    {
        this.UpdateNode();
    }
    private void FixedUpdate()
    {
        this.FixedUpdateNode();
    }

    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);


    public NodeCombine inWorldUINodeCombine; 
    public EnemyStatusInWorldUIManageNodeLeaf enemyStatusInWorldUIManageNodeLeaf;
    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);
        inWorldUINodeCombine = new NodeCombine(()=> true);
        enemyStatusInWorldUIManageNodeLeaf = new EnemyStatusInWorldUIManageNodeLeaf(()=> true,mainCamera,player,executeInWorldUI);

        startNodeSelector.AddtoChildNode(inWorldUINodeCombine);
        inWorldUINodeCombine.AddCombineNode(enemyStatusInWorldUIManageNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
}
