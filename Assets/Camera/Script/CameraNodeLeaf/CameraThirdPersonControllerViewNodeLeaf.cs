using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraThirdPersonControllerViewNodeLeaf : CameraNodeLeaf
{
    protected CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject;
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    protected CinemachineCamera cinemachineFreeLook => base.cameraController.cinemachineCamera.cinemachineCamera;
    protected ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * TimeControlManager.ReadWorldTimeFactor * cameraController.standardCameraSensivity;
    protected Vector3 curOffset;
    protected float normalizedTime;
    protected float curFOV;

    public Vector3 trackPos;
    public Vector3 lookPos;

    public virtual Vector3 targetOffset => this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight;
    public virtual float targetFOV => this.cameraThirdPersonControllerViewScriptableObject.fov;
    public virtual float transitionSpeed => this.cameraThirdPersonControllerViewScriptableObject.transitionInSpeed;
    public virtual float trackingCruve => this.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(this.normalizedTime);

    public CameraThirdPersonControllerViewNodeLeaf(CameraController cameraController,
        CameraThirdPersonControllerViewScriptableObject cameraThirdPersonViewScriptableObject,
        Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.cameraThirdPersonControllerViewScriptableObject = cameraThirdPersonViewScriptableObject;
    }

    public override void Enter()
    {
        this.trackPos = thirdPersonCamera.curTrackPosition;
        this.lookPos = thirdPersonCamera.curLookPosition;
        this.curOffset = cinemachineOffset;
        normalizedTime = 0;
        curFOV = cinemachineFreeLook.Lens.FieldOfView;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        TrackPosUpdate();
        LookPosUpdate();
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        NormalizedTimeUpdate();
        this.thirdPersonCamera.InputRotateCamera(this.inputLook.x, -this.inputLook.y);
        this.UpdateCameraData();
        OffsetUpdate();
        FOVUpdate();
        base.UpdateNode();
    }

    public virtual void TrackPosUpdate()
    {
        this.trackPos = Vector3.Lerp(thirdPersonCamera.curTrackPosition,
            thirdPersonCamera.targetFollowTarget.position, this.trackingCruve);
    }

    public virtual void LookPosUpdate()
    {
        this.lookPos = Vector3.Lerp(thirdPersonCamera.curLookPosition,
            thirdPersonCamera.targetLookTarget.position, this.trackingCruve);
    }

    public virtual void NormalizedTimeUpdate()
    {
        this.normalizedTime = Mathf.Clamp(
            this.normalizedTime += Time.unscaledDeltaTime * this.transitionSpeed, 0, 1);
    }

    public virtual void FOVUpdate()
    {
        this.curFOV = Mathf.Lerp(curFOV, this.targetFOV, this.trackingCruve);
       
    }

    public virtual void OffsetUpdate()
    {
        float offsetX;
        if (this.cameraController.curSide == Side.Right)
            offsetX = Mathf.Lerp(this.cinemachineOffset.x, this.targetOffset.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        else
            offsetX = Mathf.Lerp(this.cinemachineOffset.x, -this.targetOffset.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, this.targetOffset.y, this.trackingCruve);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, this.targetOffset.z, this.trackingCruve);

        this.curOffset = new Vector3(offsetX, offsetY, offsetZ);
    }

    public virtual void UpdateCameraData()
    {
       this.thirdPersonCamera.cameraOffset = this.curOffset;
        this.cinemachineFreeLook.Lens.FieldOfView = this.curFOV;

        this.thirdPersonCamera.MoveTargetFollow(this.trackPos,this.trackingCruve);
        this.thirdPersonCamera.MoveTargetLook(this.lookPos,this.trackingCruve);

        this.thirdPersonCamera.UpdateCameraPosition();
    }

    public void SetCameraThirdPersonControllerViewSCRP(CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject)
    {
        this.cameraThirdPersonControllerViewScriptableObject = cameraThirdPersonControllerViewScriptableObject;
    }

    public CameraThirdPersonControllerViewScriptableObject ViewSCRP => cameraThirdPersonControllerViewScriptableObject;
    public Vector3 TrackPosition => trackPos;
    public Vector3 LookPosition  => lookPos;
}
