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

    public Vector3 curTrackPos;
    public Vector3 curLookPos;

    public virtual Vector3 targetTrackPos { get => thirdPersonCamera.targetFollowTarget.position; }
    public virtual Vector3 targetLookPos { get => thirdPersonCamera.targetLookTarget.position; }

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
        this.curTrackPos = thirdPersonCamera.curTrackPosition;
        this.curLookPos = thirdPersonCamera.curLookPosition;
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
        
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {

        TrackPosUpdate();
        LookPosUpdate();
        NormalizedTimeUpdate();
        this.thirdPersonCamera.InputRotateCamera(this.inputLook.x, -this.inputLook.y);
        OffsetUpdate();
        FOVUpdate();
        this.UpdateCameraData();
        base.UpdateNode();
    }

    public virtual void TrackPosUpdate()
    {
        this.curTrackPos = Vector3.Lerp(thirdPersonCamera.curTrackPosition,
            this.targetTrackPos, this.trackingCruve);
    }

    public virtual void LookPosUpdate()
    {
        this.curLookPos = Vector3.Lerp(thirdPersonCamera.curLookPosition,
            this.targetLookPos, this.trackingCruve);
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
        float targetX = this.cameraController.curSide == Side.Right ? this.targetOffset.x : -this.targetOffset.x;
        float t = 1f - Mathf.Exp(-this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        float offsetX = Mathf.Lerp(this.cinemachineOffset.x, targetX, t);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, this.targetOffset.y, this.trackingCruve);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, this.targetOffset.z, this.trackingCruve);

        this.curOffset = new Vector3(offsetX, offsetY, offsetZ);
    }

    public virtual void UpdateCameraData()
    {
       this.thirdPersonCamera.cameraOffset = this.curOffset;
        this.cinemachineFreeLook.Lens.FieldOfView = this.curFOV;

        this.thirdPersonCamera.UpdateCameraPosition(this.curTrackPos,this.curLookPos);
    }

    public void SetCameraThirdPersonControllerViewSCRP(CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject)
    {
        this.cameraThirdPersonControllerViewScriptableObject = cameraThirdPersonControllerViewScriptableObject;
    }

    public CameraThirdPersonControllerViewScriptableObject ViewSCRP => cameraThirdPersonControllerViewScriptableObject;
    public Vector3 TrackPosition => curTrackPos;
    public Vector3 LookPosition  => curLookPos;
}
