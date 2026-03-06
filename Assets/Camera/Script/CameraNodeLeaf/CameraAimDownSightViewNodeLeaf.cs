using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{
    


    private IWeaponAdvanceUser weaponAdvanceUser;
    private float aimingWeight => weaponAdvanceUser._weaponManuverManager.aimingWeight;

    CameraThirdPersonControllerViewScriptableObject aimDownSightViewSCRP;

    protected override float trackingCruve
        => Mathf.Lerp(base.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(normalizedTime)
            , this.aimDownSightViewSCRP.transitionCurve.Evaluate(normalizedTime)
            , this.aimingWeight);

    protected override Vector3 targetOffset => Vector3.Lerp
        (
        this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight
        ,this.aimDownSightViewSCRP.viewOffsetRight
        ,this.aimingWeight
        );

    protected override float targetFOV => Mathf.Lerp
        (
        this.cameraThirdPersonControllerViewScriptableObject.fov
        , this.aimDownSightViewSCRP.fov
        , this.aimingWeight
        );

    protected override float transitionSpeed => this.cameraThirdPersonControllerViewScriptableObject.transitionInSpeed;


    public CameraAimDownSightViewNodeLeaf(
        CameraController cameraController
        ,CameraThirdPersonControllerViewScriptableObject aimDownSightViewSCRP
        , CameraThirdPersonControllerViewScriptableObject lowReadyViewSCRP
        , IWeaponAdvanceUser weaponAdvanceUser
        , Func<bool> preCondition)
        : base(cameraController, lowReadyViewSCRP, preCondition)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.aimDownSightViewSCRP = aimDownSightViewSCRP;
    }
   

   

   
}
