using Cinemachine;
using System.Collections.Generic;
using UnityEditor;
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

    public ScrpCameraViewAttribute cameraViewAttribute;

    public float zoomingWeight;

    public CameraManagerNode cameraManagerNode;

    public enum Side
    {
        left,
        right
    }
    public Side curSide;

    void Start()
    {
        Player = FindAnyObjectByType<Player>();

        curSide = Side.right;

        Cursor.lockState = CursorLockMode.Locked;
        Player.AddObserver(this);
        cameraOverShoulder = new CamerOverShoulder(this);
        cameraKickBack = new CameraKickBack(this);
        cameraZoom = new CameraZoom(this);
        cameraHandShake = new CameraHandShake(this);

        cameraManagerNode = new CameraManagerNode(this);
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        zoomingWeight = Player.weaponManuverManager.aimingWeight;

        cameraManagerNode.UpdateNode();
    }
    private void FixedUpdate()
    {
        cameraManagerNode.FixedUpdateNode();
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            cameraOverShoulder.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            cameraZoom.ZoomIn(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            cameraZoom.ZoomOut(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            cameraHandShake.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
            
        }
    }

    public void OnNotify(Player player)
    {
        
    }
}
