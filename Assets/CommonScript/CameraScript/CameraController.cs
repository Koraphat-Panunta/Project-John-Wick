using Cinemachine;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public CinemachineCameraOffset cameraOffset;
    [SerializeField] public Player Player;
    private CamerOverShoulder cameraOverShoulder;
    public CameraKickBack cameraKickBack;
    public CameraZoom cameraZoom;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
        Player.AddObserver(this);
        cameraOverShoulder = new CamerOverShoulder(this);
        cameraKickBack = new CameraKickBack(this);
        cameraZoom = new CameraZoom(this);
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            cameraOverShoulder.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.playerWeaponCommand.CurrentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            cameraZoom.ZoomIn(player.playerWeaponCommand.CurrentWeapon);
            Debug.Log("ZoomIn");
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            cameraZoom.ZoomOut(player.playerWeaponCommand.CurrentWeapon);
        }
    }
}
