using Cinemachine;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public CinemachineCameraOffset cameraOffset;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player Player;
    private CamerOverShoulder cameraOverShoulder;
    public CameraKickBack cameraKickBack;
    public CameraZoom cameraZoom;
    public CameraHandShake cameraHandShake;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
        Player.AddObserver(this);
        cameraOverShoulder = new CamerOverShoulder(this);
        cameraKickBack = new CameraKickBack(this);
        cameraZoom = new CameraZoom(this);
        cameraHandShake = new CameraHandShake(this);
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            cameraOverShoulder.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.curentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            cameraZoom.ZoomIn(player.curentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            cameraZoom.ZoomOut(player.curentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            cameraHandShake.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
            
        }
    }
}
