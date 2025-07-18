using UnityEngine;

public partial class CameraController
{
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSStandView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSCrouchView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuWeaponDisarm_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuExecuteView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuHitView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSSprintView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraAimDownSightView_SCRP;

    [SerializeField] public CameraExecuteScriptableObject cameraExecuteScriptableObject;

    [SerializeField] public bool isAiming;
    [SerializeField] public bool isSprint;
    [SerializeField] public bool isPerformGunFu;
    [SerializeField] public bool isCrouching;
    [SerializeField] public bool isOnPlayerThirdPersonController;
    [SerializeField] public IGunFuNode curGunFuNode;
}
