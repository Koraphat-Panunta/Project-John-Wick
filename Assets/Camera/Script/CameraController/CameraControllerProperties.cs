using UnityEngine;

public partial class CameraController
{
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSStandView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSCrouchView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuWeaponDisarm_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuHitView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraTPSSprintView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraStandAimDownSightView_SCRP;
    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraCrouchAimDownSightView_SCRP;

    [SerializeField] public CameraThirdPersonControllerViewScriptableObject cameraExecute_Single_SCRP;

    [SerializeField] public bool isAiming;
    [SerializeField] public bool isSprint;
    [SerializeField] public bool isPerformGunFu;
    [SerializeField] public bool isCrouching;
    [SerializeField] public bool isOnPlayerThirdPersonController;
    [SerializeField] public IGunFuNode curGunFuNode;


}
