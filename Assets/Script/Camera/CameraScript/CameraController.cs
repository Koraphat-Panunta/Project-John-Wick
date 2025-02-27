using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer,IObserverPlayerSpawner
{
    [SerializeField] public CinemachineCamera cinemachineCamera => Player.cinemachineCamera;
    [SerializeField] public CinemachineOrbitalFollow cinemachineOrbitalFollow => Player.cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
    [SerializeField] public CinemachineCameraOffset cameraOffset => Player.cinemachineCamera.GetComponent<CinemachineCameraOffset>();
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player Player;

    [SerializeField] private PlayerSpawner playerSpawner;

    public CameraKickBack cameraKickBack;
    public CameraImpulseShake cameraImpluse;

    public ScrpCameraViewAttribute cameraViewAttribute;

    public bool isZooming = false;
    public float zoomingWeight;
    public float cameraSwitchSholderVelocity = 3.5f;

    public float gunFuCameraTimer = 0;
    public const float gunFuCameraDuration = 1;

    public CameraManagerNode cameraManagerNode;

    public string CameraNodeName;
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

        cameraKickBack = new CameraKickBack(this);

        cameraImpluse = new CameraImpulseShake(this);

        cameraManagerNode = new CameraManagerNode(this);
    }
    void Start()
    {

    }
    private void Update()
    {
        if(gunFuCameraTimer >0)
            gunFuCameraTimer -= Time.deltaTime;

        if(Player != null && Player.weaponManuverManager != null)
        zoomingWeight = Player.weaponManuverManager.aimingWeight;

        Debug.Log("CameraCurNode = " + cameraManagerNode.curNodeLeaf);

        cameraManagerNode.UpdateNode();


        //Debug.Log("CameraOffset = " + cameraOffset);
        //Debug.Log("CameraOffset value = " + cameraOffset.Offset);
    }
    private void FixedUpdate()
    {
        cameraManagerNode.FixedUpdateNode();
    }
    [SerializeField] private float cameraKickbackMultiple;
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.GunFuEnter)
        {
            gunFuCameraTimer = gunFuCameraDuration;
        }
        if(playerAction == SubjectPlayer.PlayerAction.GunFuAttack)
        {
            if(player.playerStateNodeManager.curNodeLeaf is KnockDown_GunFuNode)
                cameraImpluse.Performed(new Vector3(0.25f,0,0));
            else
            cameraImpluse.Performed(0.25f);
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            curSide = player.curShoulderSide;
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.currentWeapon);
            cameraImpluse.Performed((player.currentWeapon.RecoilKickBack - player.currentWeapon.RecoilCameraController) * cameraKickbackMultiple);
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
            cameraImpluse.Performed();
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

        Player.AddObserver(this);
    }
}
