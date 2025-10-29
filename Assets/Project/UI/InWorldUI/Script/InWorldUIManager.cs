using UnityEngine;
public class InWorldUIManager : MonoBehaviour,IInitializedAble
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private InWorldUI executeInWorldUI;
    [SerializeField] private InWorldUI interactableInWorldUI;
    [SerializeField] private InWorldUI doorInteractableInWorldUI;
    [SerializeField] private EnemyHPInWorldUI enemyHPInWorldUI;
    [SerializeField] private Player player;

  


    public void Initialized()
    {
        this.inWorldUINodeComponentManager = new NodeComponentManager();
        this.InitailizedNode();
    }
   
    void Update()
    {
        this.inWorldUINodeComponentManager.Update();
    }
    private void FixedUpdate()
    {
        this.inWorldUINodeComponentManager.FixedUpdate();
    }


    public NodeComponentManager inWorldUINodeComponentManager;
 
    public EnemyStaggerStatusInWorldUIManageNodeLeaf enemyStatusInWorldUIManageNodeLeaf;
    public InteractablePointUIManagerNodeLeaf interactablePointUIManagerNodeLeaf;
    public InteractablePointUIManagerNodeLeaf doorInteractablePointUIManagerNodeLeaf;
    public EnemyHP_Bar_InWorldUINodeLeaf enemyHP_Bar_InWorldUINodeLeaf;
    public void InitailizedNode()
    {


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

        inWorldUINodeComponentManager.AddNode(enemyHP_Bar_InWorldUINodeLeaf);
        inWorldUINodeComponentManager.AddNode(enemyStatusInWorldUIManageNodeLeaf);
        inWorldUINodeComponentManager.AddNode(interactablePointUIManagerNodeLeaf);
        inWorldUINodeComponentManager.AddNode(doorInteractablePointUIManagerNodeLeaf);


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
