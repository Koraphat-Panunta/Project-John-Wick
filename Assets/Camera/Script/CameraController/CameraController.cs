using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public partial class CameraController : MonoBehaviour,IObserverPlayer,IInitializedAble
{
    [SerializeField] public CinemachineBrain cinemachineBrain;
    [SerializeField] public ThirdPersonCinemachineCamera thirdPersonCinemachineCamera;
    private List<CinemachineCamera> allCinemachine = new List<CinemachineCamera>(); 
    [SerializeField] public CinemachineCamera cinemachineCamera => player.cinemachineCamera;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player player;

    public CameraKickBack cameraKickBack;
    public CameraImpulseShake cameraImpluse;

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
    public void Initialized()
    {
        //allCinemachine.Add(thirdPersonCinemachineCamera.cinemachineCamera);
        curSide = player.curShoulderSide;

        player.AddObserver(this);

        this.isOnPlayerThirdPersonController = true;

        cameraKickBack = new CameraKickBack(this);

        cameraImpluse = new CameraImpulseShake(this);

        cameraManagerNode = new CameraManagerNode(this);
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
    [SerializeField] private float cameraKickImpulseMultiple;
    public float cameraKickUpMultiple;

    [Range(0, 5)]
    [SerializeField] private float gunFuCameraKickMultiply;
 
    public void OnNotify<T>(Player player, T node)
    {
        if (node is SubjectPlayer.NotifyEvent notifyEvent)
        {
            if (notifyEvent == SubjectPlayer.NotifyEvent.SwapShoulder)
            {
                curSide = player.curShoulderSide;
            }
            if (notifyEvent == SubjectPlayer.NotifyEvent.Firing)
            {
                cameraKickBack.Performed(player._currentWeapon);
                cameraImpluse.Performed(new Vector3(
                    (player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilCameraController) * cameraKickImpulseMultiple * (Random.Range(1, 10) > 5 ? 1 : -1)
                    , (player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilCameraController) * cameraKickImpulseMultiple
                    ,0)
                    );
            }

            if (notifyEvent == SubjectPlayer.NotifyEvent.GetShoot)
            {
                cameraImpluse.Performed(-0.05f);
            }
        }
        else
        if (node is INode)
        switch (node)
        {
           case IGunFuExecuteNodeLeaf gunFuExecute_NodeLeaf:
                    {
                        if ((gunFuExecute_NodeLeaf as PlayerStateNodeLeaf).curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                        {
                            this.isPerformGunFu = true;
                            this.curGunFuNode = gunFuExecute_NodeLeaf;
                        }
                        else if ((gunFuExecute_NodeLeaf as PlayerStateNodeLeaf).curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                        {
                            this.isPerformGunFu = false;
                            if (this.curGunFuNode == gunFuExecute_NodeLeaf)
                                this.curGunFuNode = null;
                        }
                        break;
                    }
           case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Enter)
                    {
                        this.isPerformGunFu = true;
                        this.curGunFuNode = gunFuHitNodeLeaf;
                    }
                    else if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Exit)
                    {
                        this.isPerformGunFu = false;
                        if(this.curGunFuNode == gunFuHitNodeLeaf)
                            this.curGunFuNode = null;
                    }
                        

                    if(gunFuHitNodeLeaf._stateName == "Hit3" && gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                        cameraImpluse.Performed(new Vector3(0.25f, 0, 0) * this.gunFuCameraKickMultiply);
                    else if(gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
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
                        this.isSprint = true;
                    else if(playerSprintNode.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                        this.isSprint = false;
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
                        this.isCrouching = true;
                    else if ((node as PlayerStateNodeLeaf).curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                        this.isCrouching = false;
                    break;
                }
            case AimDownSightWeaponManuverNodeLeaf adsNodeLeaf:
                {
                   if(adsNodeLeaf.curPhase == AimDownSightWeaponManuverNodeLeaf.AimDownSightPhase.Enter)
                        isAiming = true;
                   else if(adsNodeLeaf.curPhase == AimDownSightWeaponManuverNodeLeaf.AimDownSightPhase.Exit)
                        isAiming = false;
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
