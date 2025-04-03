using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineBrain cinemachineBrain;
    [SerializeField] public ThirdPersonCinemachineCamera thirdPersonCinemachineCamera;
    [SerializeField] public ThirdPersonCinemachineCamera executeThirdPersonCinemachineCameara;
    private List<CinemachineCamera> allCinemachine = new List<CinemachineCamera>(); 
    [SerializeField] public CinemachineCamera cinemachineCamera => player.cinemachineCamera;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player player;


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
        allCinemachine.Add(executeThirdPersonCinemachineCameara.cinemachineCamera);

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

        if(player != null && player.weaponManuverManager != null)
        zoomingWeight = player.weaponManuverManager.aimingWeight;

        cameraManagerNode.UpdateNode();

        this.CameraNodeName = cameraManagerNode.curNodeLeaf.ToString();

        //Debug.Log("CameraOffset = " + cameraOffset);
        //Debug.Log("CameraOffset value = " + cameraOffset.Offset);


    }
    private void FixedUpdate()
    {
        inputLook = "ScreenWidht = " + Screen.width + " ScreenHight = "+Screen.height;
        Debug.Log("Input = " + player.inputLookDir_Local);
        cameraManagerNode.FixedUpdateNode();
    }
    [SerializeField] private float cameraKickbackMultiple;
    public float cameraKickUpMultiple;
    public bool isWeaponDisarm => player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf;
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
            cameraKickBack.Performed(player._currentWeapon);
            cameraImpluse.Performed((player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilCameraController) * cameraKickbackMultiple);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            isZooming = true;
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady || playerAction == SubjectPlayer.PlayerAction.Resting)
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
        executeThirdPersonCinemachineCameara = player.executeCinemachineCamera.GetComponent<ThirdPersonCinemachineCamera>();
    }
}
