using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineBrain cinemachineBrain;
    [SerializeField] public ThirdPersonCinemachineCamera thirdPersonCinemachineCamera;
    private List<CinemachineCamera> allCinemachine = new List<CinemachineCamera>(); 
    [SerializeField] public CinemachineCamera cinemachineCamera => player.cinemachineCamera;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player player;

    [SerializeField] public CameraExecuteScriptableObject cameraExecuteScriptableObject;

    public CameraKickBack cameraKickBack;
    public CameraImpulseShake cameraImpluse;

    public ScrpCameraViewAttribute cameraViewAttribute;

    public float zoomingWeight;
    public float cameraSwitchSholderVelocity = 3.5f;

    public float gunFuCameraTimer = 0;
    public const float gunFuCameraDuration = 1.25f;

    public CameraManagerNode cameraManagerNode;

    [Range(1, 10)]
    public float standardCameraSensivity;
    [Range(1, 10)]
    public float aimDownSightCameraSensivity;

    [SerializeField,TextArea]
    public string inputLook;
    public enum Side
    {
        left,
        right
    }
    
    public Player.ShoulderSide curSide;
    public Player.PlayerStance curStance;

    private void Awake()
    {
        allCinemachine.Add(thirdPersonCinemachineCamera.cinemachineCamera);

        curStance = player.playerStance;
        curSide = player.curShoulderSide;

        player.AddObserver(this);

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

        if(player != null && player._weaponManuverManager != null)
        zoomingWeight = player._weaponManuverManager.aimingWeight;

        cameraManagerNode.UpdateNode();

    }
    private void FixedUpdate()
    {
        inputLook = "ScreenWidht = " + Screen.width + " ScreenHight = "+Screen.height;
        cameraManagerNode.FixedUpdateNode();
    }
    [SerializeField] private float cameraKickbackMultiple;
    public float cameraKickUpMultiple;

    [Range(0, 5)]
    [SerializeField] private float gunFuCameraKickMultiply;
    public bool isWeaponDisarm => player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf;
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
        if(playerAction == SubjectPlayer.NotifyEvent.GunFuEnter)
        {
            gunFuCameraTimer = gunFuCameraDuration;
        }
        if(playerAction == SubjectPlayer.NotifyEvent.GunFuInteract)
        {
            if (player.playerStateNodeManager.curNodeLeaf is RestrictGunFuStateNodeLeaf restrict
                && restrict.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                gunFuCameraTimer = gunFuCameraDuration;
        }
        if(playerAction == SubjectPlayer.NotifyEvent.GunFuAttack)
        {
            if (player.playerStateNodeManager.curNodeLeaf is RestrictGunFuStateNodeLeaf)
                cameraImpluse.Performed(new Vector3(0,0.25f,0)*this.gunFuCameraKickMultiply);
            else if (player.playerStateNodeManager.curNodeLeaf is KnockDown_GunFuNode)
                cameraImpluse.Performed(new Vector3(0.25f, 0, 0) * this.gunFuCameraKickMultiply);
            else
                cameraImpluse.Performed(0.25f * this.gunFuCameraKickMultiply);
        }
        if(playerAction == SubjectPlayer.NotifyEvent.SwapShoulder)
        {
            curSide = player.curShoulderSide;
        }
        if(playerAction == SubjectPlayer.NotifyEvent.Firing)
        {
            cameraKickBack.Performed(player._currentWeapon);
            cameraImpluse.Performed((player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilCameraController) * cameraKickbackMultiple);
        }
        
        if(playerAction == SubjectPlayer.NotifyEvent.GotAttackGunFuAttack)
        {
            cameraImpluse.Performed(-0.2f);
        }

        if(playerAction == SubjectPlayer.NotifyEvent.GetShoot)
        {
            cameraImpluse.Performed(-0.05f);
        }

        if (playerAction == SubjectPlayer.NotifyEvent.Sprint)
        { curStance = player.playerStance; }

        if (playerAction == SubjectPlayer.NotifyEvent.StandIdle
            || playerAction == SubjectPlayer.NotifyEvent.StandMove)
        { curStance = player.playerStance; }

        if (playerAction == SubjectPlayer.NotifyEvent.CrouchIdle
            || playerAction == SubjectPlayer.NotifyEvent.CrouchMove)
        { curStance = player.playerStance; }
    }

    public void OnNotify(Player player)
    {
        
    }

    public void ChangeCamera(CinemachineCamera cinemachineCamera,float time)
    {
        cinemachineBrain.DefaultBlend.Time = time;

        cinemachineCamera.Priority = 10;

        if(allCinemachine.Count <= 0)
            return;

        for (int i = 0; i < allCinemachine.Count; i++) 
        {
            if (allCinemachine[i] == cinemachineCamera)
                continue;

            allCinemachine[i].Priority = 1;
        }
    }

    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
        thirdPersonCinemachineCamera = cinemachineCamera.GetComponent<ThirdPersonCinemachineCamera>();
    }
}
