using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public partial class CameraController : MonoBehaviour,IObserverPlayer,IInitializedAble
{
    [SerializeField] public CinemachineBrain cinemachineBrain;
    [SerializeField] public Camera cameraMain;
    [SerializeField] public ThirdPersonCinemachineCamera thirdPersonCinemachineCamera;
    private List<CinemachineCamera> allCinemachine = new List<CinemachineCamera>(); 
    [SerializeField] public CinemachineCamera cinemachineCamera => player.cinemachineCamera;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player player;

    public CameraKickBack cameraKickBack;
    public CameraImpulseShake cameraImpluse;

    [SerializeField] private GameMaster gameMaster;

    public float zoomingWeight;
    public float cameraSwitchSholderVelocity = 3.5f;

    public float gunFuCameraTimer = 0;
    public const float gunFuCameraDuration = 1.25f;

    public CameraManagerNode cameraManagerNode;

    public float standardCameraSensivity => gameMaster.dataBased.settingData.mouseSensitivivty * 0.1f;
    public float aimDownSightCameraSensivity => gameMaster.dataBased.settingData.mouseAimDownSightSensitivity * 0.1f;

    [SerializeField,TextArea]
    public string inputLook;
    public enum Side
    {
        left,
        right
    }
    
    public Player.ShoulderSide curSide;

    [Range(0, 10)]
    [SerializeField] private float shootImpluseDuration;

    [Range(0, 10)]
    [SerializeField] private float hitImpluseDuration;

    [Range(0, 10)]
    [SerializeField] private float gotHitImplauseDuration;
    [SerializeField] private Vector3 gotHitImplauseDirection;
    [SerializeField] private float gotHitImplauseIntensity;


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
    public Vector2 cameraKickPositionRange;
    public Vector2 cameraImpulseRange;

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
                cameraImpluse.impulseSource.ImpulseDefinition.ImpulseDuration = shootImpluseDuration;
                cameraImpluse.Performed(new Vector3(
                    (Mathf.Lerp(cameraImpulseRange.x,cameraImpulseRange.y,player._currentWeapon.Recoil_VisualImpulse) * (Random.Range(1, 10) > 5 ? 1 : -1))
                    , Mathf.Lerp(cameraImpulseRange.x, cameraImpulseRange.y, player._currentWeapon.Recoil_VisualImpulse)
                    ,0
                    ));
            }

            if (notifyEvent == SubjectPlayer.NotifyEvent.GetShoot)
            {
                cameraImpluse.impulseSource.ImpulseDefinition.ImpulseDuration = gotHitImplauseDuration;
                cameraImpluse.Performed(new Vector3(
                    (gotHitImplauseDirection.x * gotHitImplauseIntensity) * (Random.Range(1, 10) > 5 ? 1 : -1)
                    , (gotHitImplauseDirection.y * gotHitImplauseIntensity)
                    , (gotHitImplauseDirection.z * gotHitImplauseIntensity))
                    );
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


                        if (gunFuHitNodeLeaf._stateName == "Hit3" && gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                        {
                            cameraImpluse.impulseSource.ImpulseDefinition.ImpulseDuration = hitImpluseDuration*2.5f;
                            cameraImpluse.Performed(new Vector3(-1, 0, 1f) * this.gunFuCameraKickMultiply);
                        }
                        else if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                        {
                            cameraImpluse.impulseSource.ImpulseDefinition.ImpulseDuration = hitImpluseDuration;
                            cameraImpluse.Performed(new Vector3(0, 0, 1f) * this.gunFuCameraKickMultiply);
                        }
                    break;
                }
            case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {
                        if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                            gunFuCameraTimer = gunFuCameraDuration;
                        else if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.ExitAttack)
                        {
                            cameraImpluse.impulseSource.ImpulseDefinition.ImpulseDuration = hitImpluseDuration;
                            cameraImpluse.Performed(new Vector3(0, 0, 1f) * this.gunFuCameraKickMultiply);
                        }
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
        try
        {
            thirdPersonCinemachineCamera = cinemachineCamera.GetComponent<ThirdPersonCinemachineCamera>();
        }
        catch { }
        if (this.gameMaster == null)
            this.gameMaster = FindAnyObjectByType<GameMaster>();
    }

  
}
