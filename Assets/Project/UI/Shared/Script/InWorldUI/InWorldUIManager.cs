using UnityEngine;

public class InWorldUIManager : MonoBehaviour,INodeManager
{
    [SerializeField] private ExecuteInWorldUI executeInWorldUI;
    public ObjectPooling<ExecuteInWorldUI> executeUI_Pool;

    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    INodeLeaf INodeManager.curNodeLeaf { get => _curNodeLeaf; set => _curNodeLeaf = value; }
    private INodeLeaf _curNodeLeaf;

    private void Awake()
    {
        this.executeUI_Pool = new ObjectPooling<ExecuteInWorldUI>(executeInWorldUI,10,5,Vector3.zero);

        this.InitailizedNode();
    }
    void Start()
    {
        
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
   

    public void InitailizedNode()
    {
        throw new System.NotImplementedException();
    }
}
