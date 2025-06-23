using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerGunFuHitNodeLeaf;


public partial class CameraController : MonoBehaviour,IObserverPlayer
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

    private void Awake()
    {
        //allCinemachine.Add(thirdPersonCinemachineCamera.cinemachineCamera);
        curSide = player.curShoulderSide;

        player.AddObserver(this);

        cameraKickBack = new CameraKickBack(this);

        cameraImpluse = new CameraImpulseShake(this);

        cameraManagerNode = new CameraManagerNode(this);
    }
    void Start()
    {
        cameraManagerNode.blackBoard.Set<bool>("isOnPlayerThirdPersonController", true);
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
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
        
        if(playerAction == SubjectPlayer.NotifyEvent.SwapShoulder)
        {
            curSide = player.curShoulderSide;
        }
        if(playerAction == SubjectPlayer.NotifyEvent.Firing)
        {
            cameraKickBack.Performed(player._currentWeapon);
            cameraImpluse.Performed((player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilCameraController) * cameraKickbackMultiple);
        }

        if(playerAction == SubjectPlayer.NotifyEvent.GetShoot)
        {
            cameraImpluse.Performed(-0.05f);
        }
       
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        switch (node)
        {
           case PlayerGunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curGunFuHitPhase == PlayerGunFuHitNodeLeaf.GunFuHitPhase.Enter)
                    {
                        cameraManagerNode.blackBoard.Set<bool>("isPerformGunFu", true);
                        cameraManagerNode.blackBoard.Set<IGunFuNode>("curGunFuNode", gunFuHitNodeLeaf);
                    }

                    if (gunFuHitNodeLeaf.curGunFuHitPhase == GunFuHitPhase.Exit)
                    {
                        cameraManagerNode.blackBoard.Set<bool>("isPerformGunFu", false);
                        if(cameraManagerNode.blackBoard.Get<IGunFuNode>("curGunFuNode") == gunFuHitNodeLeaf)
                            cameraManagerNode.blackBoard.Set<IGunFuNode>("curGunFuNode", null);
                    }
                        

                    if(gunFuHitNodeLeaf is KnockDown_GunFuNode knock && knock.curGunFuHitPhase == GunFuHitPhase.Hit)
                        cameraImpluse.Performed(new Vector3(0.25f, 0, 0) * this.gunFuCameraKickMultiply);
                    else if(gunFuHitNodeLeaf.curGunFuHitPhase == GunFuHitPhase.Hit)
                        cameraImpluse.Performed(0.25f * this.gunFuCameraKickMultiply);
                    break;
                }
            case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {
                    if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                        gunFuCameraTimer = gunFuCameraDuration;
                    else if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.ExitAttack)
                        cameraImpluse.Performed(new Vector3(0, 0.25f, 0) * this.gunFuCameraKickMultiply);
                    break;
                }
            case PlayerBrounceOffGotAttackGunFuNodeLeaf playerBrounceOffGotAttackGunFuNodeLeaf: 
                {
                    cameraImpluse.Performed(-0.2f);
                    break;
                }
            case PlayerSprintNode playerSprintNode: 
                {
                    if (playerSprintNode.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                        cameraManagerNode.blackBoard.Set<bool>("isSprinting", true);
                    else if(playerSprintNode.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                        cameraManagerNode.blackBoard.Set<bool>("isSprinting", false);
                    break;
                }
            case PlayerStandIdleNodeLeaf:
            case PlayerStandMoveNodeLeaf: 
                {
                    
                    break;
                }
            case PlayerCrouch_Idle_NodeLeaf:
            case PlayerCrouch_Move_NodeLeaf: 
                {
                    if ((node as PlayerStateNodeLeaf).curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                        cameraManagerNode.blackBoard.Set<bool>("isCrouching", true);
                    else if ((node as PlayerStateNodeLeaf).curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                        cameraManagerNode.blackBoard.Set<bool>("isCrouching", false);
                    break;
                }
        }
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
        if(this.player == null)
            this.player = FindAnyObjectByType<Player>();
        thirdPersonCinemachineCamera = cinemachineCamera.GetComponent<ThirdPersonCinemachineCamera>();
    }

    
}
