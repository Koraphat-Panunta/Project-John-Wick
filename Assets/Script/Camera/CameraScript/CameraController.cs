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

    public CameraKickBack cameraKickBack;
    public CameraHandShake cameraHandShake;

    public ScrpCameraViewAttribute cameraViewAttribute;

    public bool isZooming = false;
    public float zoomingWeight;
    public float cameraSwitchSholderVelocity = 3.5f;

    public CameraManagerNode cameraManagerNode;

    public enum Side
    {
        left,
        right
    }
    
    public Player.ShoulderSide curSide;
    public Player.PlayerStance curStance;

    void Start()
    {
        Player = FindAnyObjectByType<Player>();
        curStance = Player.playerStance;
        curSide = Player.curShoulderSide;

        Cursor.lockState = CursorLockMode.Locked;
        Player.AddObserver(this);

        cameraKickBack = new CameraKickBack(this);

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
            curSide = player.curShoulderSide;
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            isZooming = true;
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            isZooming = false;
        }
        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            cameraHandShake.Performed();
        }

        if (playerAction == SubjectPlayer.PlayerAction.Sprint)
        { curStance = player.playerStance; }

        if (playerAction == SubjectPlayer.PlayerAction.StandIdle
            || playerAction == SubjectPlayer.PlayerAction.StandMove)
        { curStance = player.playerStance; }

        if (playerAction == SubjectPlayer.PlayerAction.CrouchIdle
            || playerAction == SubjectPlayer.PlayerAction.CrouchMove)
        { curStance = player.playerStance; }
    }

    public void OnNotify(Player player)
    {
        
    }
}
