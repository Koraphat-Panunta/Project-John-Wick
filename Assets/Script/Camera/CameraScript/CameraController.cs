using Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer,IObserverPlayerSpawner
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] public CinemachineCameraOffset cameraOffset;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player Player;

    [SerializeField] private PlayerSpawner playerSpawner;

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

    private void Awake()
    {
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        playerSpawner.AddObserverPlayerSpawner(this);
    }
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

        cameraKickBack = new CameraKickBack(this);

        cameraHandShake = new CameraHandShake(this);

        cameraManagerNode = new CameraManagerNode(this);
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if(Player != null)
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

    public void GetNotify(Player player)
    {
        Player = player;
        curStance = Player.playerStance;
        curSide = Player.curShoulderSide;

        this.CinemachineFreeLook = player.cinemachineFreeLook;
        this.cameraOffset = player.cinemachineFreeLook.GetComponent<CinemachineCameraOffset>();

        Player.AddObserver(this);
    }
}
