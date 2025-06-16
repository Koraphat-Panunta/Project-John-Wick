using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player))]
public class DynamicCharacterControllerPlayerUpdate : MonoBehaviour,IObserverPlayer
{
    private CharacterController characterController;

    [SerializeField] private float StandCenterY;
    [SerializeField] private float StandHeight;

    [SerializeField] private float CrouchCenterY;
    [SerializeField] private float CrouchHeight;
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
       

       

       
    }

    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if(node is PlayerStateNodeLeaf playerStateNodeLeaf)
            switch (playerStateNodeLeaf)
        {
            case PlayerCrouch_Idle_NodeLeaf playerCrouchIdleNodeLeaf:
            case PlayerCrouch_Move_NodeLeaf playerCrouchMoveNodeLeaf:
                {
                    if (player._weaponManuverManager.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf)
                    {
                        characterController.center = new Vector3(characterController.center.x, StandCenterY, characterController.center.z);
                        characterController.height = StandHeight;
                    }
                    else
                    {
                        characterController.center = new Vector3(characterController.center.x, CrouchCenterY, characterController.center.z);
                        characterController.height = CrouchHeight;
                    }
                    break;
                }
            case PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf: 
                {
                    characterController.center = new Vector3(characterController.center.x, CrouchCenterY, characterController.center.z);
                    characterController.height = CrouchHeight;
                    break;
                }
            case PlayerStandMoveNodeLeaf playerStandMoveNodeLeaf:
            case PlayerStandIdleNodeLeaf playerStandIdleNodeLeaf:
                {
                    characterController.center = new Vector3(characterController.center.x, StandCenterY, characterController.center.z);
                    characterController.height = StandHeight;
                    break;
                }
            
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Player>().AddObserver(this);
        characterController = GetComponent<CharacterController>();

        StandCenterY = characterController.center.y;
        StandHeight = characterController.height;
    }

   
}
