using UnityEngine;
public class InWorldUIManager : MonoBehaviour,INodeManager,IInitializedAble
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private InWorldUI executeInWorldUI;
    [SerializeField] private InWorldUI interactableInWorldUI;
    [SerializeField] private InWorldUI doorInteractableInWorldUI;
    [SerializeField] private EnemyHPInWorldUI enemyHPInWorldUI;
    [SerializeField] private Player player;

    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    INodeLeaf INodeManager._curNodeLeaf { get => _curNodeLeaf; set => _curNodeLeaf = value; }
    private INodeLeaf _curNodeLeaf;


    public void Initialized()
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
    public EnemyStaggerStatusInWorldUIManageNodeLeaf enemyStatusInWorldUIManageNodeLeaf;
    public InteractablePointUIManagerNodeLeaf interactablePointUIManagerNodeLeaf;
    public InteractablePointUIManagerNodeLeaf doorInteractablePointUIManagerNodeLeaf;
    public EnemyHP_Bar_InWorldUINodeLeaf enemyHP_Bar_InWorldUINodeLeaf;
    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);
        inWorldUINodeCombine = new NodeCombine(()=> true);

        enemyHP_Bar_InWorldUINodeLeaf = new EnemyHP_Bar_InWorldUINodeLeaf(()=> true,mainCamera,player, enemyHPInWorldUI);
        enemyStatusInWorldUIManageNodeLeaf = new EnemyStaggerStatusInWorldUIManageNodeLeaf(
            ()=> player._currentWeapon != null 
            && player._currentWeapon.bulletStore[BulletStackType.Magazine] + player._currentWeapon.bulletStore[BulletStackType.Chamber] > 0
            , mainCamera
            ,player
            ,executeInWorldUI);
        interactablePointUIManagerNodeLeaf = new InteractablePointUIManagerNodeLeaf(
            () => true, 
            interactableInWorldUI, 
            mainCamera,
            player,
            LayerMask.GetMask("Weapon"),
            Vector3.zero
            );
        doorInteractablePointUIManagerNodeLeaf = new DoorInteractablePointUIManagerNodeLeaf(
            () => true
            , doorInteractableInWorldUI
            , mainCamera
            , player
            , LayerMask.GetMask("InteractAble")
            , Vector3.zero
            );

        startNodeSelector.AddtoChildNode(inWorldUINodeCombine);
        inWorldUINodeCombine.AddCombineNode(enemyHP_Bar_InWorldUINodeLeaf);
        inWorldUINodeCombine.AddCombineNode(enemyStatusInWorldUIManageNodeLeaf);
        inWorldUINodeCombine.AddCombineNode(interactablePointUIManagerNodeLeaf);
        inWorldUINodeCombine.AddCombineNode(doorInteractablePointUIManagerNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    private void OnValidate()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if(player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
            
    }

    
}
