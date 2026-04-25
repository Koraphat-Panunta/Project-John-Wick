using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraThirdPersonControllerViewNodeLeaf : CameraNodeLeaf
{
    protected CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject;
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => base.cameraController.cinemachineCamera.cinemachineCamera;
    protected ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * TimeControlManager.ReadWorldTimeFactor * cameraController.standardCameraSensivity ;
    protected Vector3 enteringOffset;
    protected float normalizedTime;
    protected float enteringFOV;

    protected Vector3 trackPos;
    protected Vector3 lookPos;

    protected virtual Vector3 targetOffset => this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight;
    protected virtual float targetFOV => this.cameraThirdPersonControllerViewScriptableObject.fov;

    protected virtual float transitionSpeed => this.cameraThirdPersonControllerViewScriptableObject.transitionInSpeed;
    protected virtual float trackingCruve => this.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(this.normalizedTime);

    public CameraThirdPersonControllerViewNodeLeaf(CameraController cameraController
        ,CameraThirdPersonControllerViewScriptableObject cameraThirdPersonViewScriptableObject
        , Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.cameraThirdPersonControllerViewScriptableObject = cameraThirdPersonViewScriptableObject;
    }
   
    public override void Enter()
    {
        this.trackPos = thirdPersonCamera.curTrackPosition;
        this.lookPos = thirdPersonCamera.curLookPosition;
        enteringOffset = cinemachineOffset;
        normalizedTime = 0;
        enteringFOV = cinemachineFreeLook.Lens.FieldOfView;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        this.trackPos = Vector3.Lerp(thirdPersonCamera.curTrackPosition, thirdPersonCamera.targetFollowTarget.position, this.trackingCruve );
        this.lookPos = Vector3.Lerp(thirdPersonCamera.curLookPosition, thirdPersonCamera.targetLookTarget.position, this.trackingCruve);
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {

        float offsetX;
        normalizedTime = Mathf.Clamp(
            normalizedTime += Time.unscaledDeltaTime * this.transitionSpeed
            ,0
            ,1
            );

        thirdPersonCamera.InputRotateCamera(inputLook.x, -inputLook.y);
        this.UpdateCameraPosition();

        if (this.cameraController.curSide == Side.Right)
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                this.targetOffset.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                -this.targetOffset.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(enteringFOV, this.targetFOV, this.trackingCruve);
        this.enteringFOV = this.cinemachineFreeLook.Lens.FieldOfView;

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, this.targetOffset.y, this.trackingCruve);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, this.targetOffset.z, this.trackingCruve);

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
        base.UpdateNode();
    }

    public virtual void UpdateCameraPosition()
    {
        thirdPersonCamera.UpdateCameraPosition(this.trackPos,this.lookPos);
        
    }

    public void SetCameraThirdPersonControllerViewSCRP(CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject)
    {
        this.cameraThirdPersonControllerViewScriptableObject = cameraThirdPersonControllerViewScriptableObject;
    }
}
