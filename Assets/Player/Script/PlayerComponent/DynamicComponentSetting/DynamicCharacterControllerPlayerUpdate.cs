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
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (player.playerStateNodeManager.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
            || player.playerStateNodeManager.curNodeLeaf is PlayerCrouch_Move_NodeLeaf)
        {
            switch (playerAction)
            {
                case SubjectPlayer.PlayerAction.LowReady:
                    {
                        characterController.center = new Vector3(characterController.center.x, CrouchCenterY, characterController.center.z);
                        characterController.height = CrouchHeight;
                    }
                    break;
                case SubjectPlayer.PlayerAction.Aim:
                    {
                        characterController.center = new Vector3(characterController.center.x, StandCenterY, characterController.center.z);
                        characterController.height = StandHeight;
                    }
                    break;
            }

        }

        if (playerAction == SubjectPlayer.PlayerAction.StandMove
            ||playerAction == SubjectPlayer.PlayerAction.StandIdle)
        {
            characterController.center = new Vector3(characterController.center.x, StandCenterY, characterController.center.z);
            characterController.height = StandHeight;
        }
    }

    public void OnNotify(Player player)
    {
        
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
