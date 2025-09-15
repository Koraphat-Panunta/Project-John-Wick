using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player))]
public class DynamicCharacterControllerPlayerUpdate : MonoBehaviour,IObserverPlayer,IInitializedAble
{
    private CharacterController characterController;

    [SerializeField] private float StandCenterY;
    [SerializeField] private float StandHeight;

    [SerializeField] private float CrouchCenterY;
    [SerializeField] private float CrouchHeight;

    [SerializeField] private float parkourCenterY;
    [SerializeField] private float parkourHeight;
    private Player player;
    private INodeManager playerStateNodeManager => player.playerStateNodeManager;
    public void Initialized()
    {

    }
   
    public void OnNotify<T>(Player player, T node)
    {
        if (playerStateNodeManager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>())
        {
            characterController.center = new Vector3(characterController.center.x, CrouchCenterY, characterController.center.z);
            characterController.height = CrouchHeight;
        }
        else if (playerStateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>()
            || playerStateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>())
        {
            characterController.center = new Vector3(characterController.center.x, CrouchCenterY, characterController.center.z);
            characterController.height = CrouchHeight;
        }
        else if ((playerStateNodeManager.TryGetCurNodeLeaf<IParkourNodeLeaf>()))
        {
            characterController.center = new Vector3(characterController.center.x, parkourCenterY, characterController.center.z);
            characterController.height = parkourHeight;
        }
        else
        {
            characterController.center = new Vector3(characterController.center.x, StandCenterY, characterController.center.z);
            characterController.height = StandHeight;
        }
       
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GetComponent<Player>();
        this.player.AddObserver(this);
        characterController = GetComponent<CharacterController>();

        StandCenterY = characterController.center.y;
        StandHeight = characterController.height;
    }

   
}
