using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraThirdPersonControllerViewNodeLeaf : CameraNodeLeaf
{
    protected CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject;
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => base.cameraController.cinemachineCamera;
    protected ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;
    protected Vector3 enteringOffset;
    protected float normalizedTime;
    protected float enteringFOV;

    protected Vector3 trackPos;
    protected Vector3 lookPos;
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
        this.trackPos = Vector3.Lerp(thirdPersonCamera.curTrackPosition, thirdPersonCamera.targetFollowTarget.position, normalizedTime);
        this.lookPos = Vector3.Lerp(thirdPersonCamera.curLookPosition, thirdPersonCamera.targetLookTarget.position, normalizedTime);
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        float offsetX;
        normalizedTime = Mathf.Clamp(
            normalizedTime += Time.unscaledDeltaTime * cameraThirdPersonControllerViewScriptableObject.transitionInSpeed
            ,cameraThirdPersonControllerViewScriptableObject.minNormalized
            ,cameraThirdPersonControllerViewScriptableObject.maxNormalized
            );

        thirdPersonCamera.InputRotateCamera(inputLook.x, -inputLook.y);
        this.UpdateCameraPosition();

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                -this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(enteringFOV, this.cameraThirdPersonControllerViewScriptableObject.fov, this.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(normalizedTime));

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.y, this.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(normalizedTime));
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, this.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.z, this.cameraThirdPersonControllerViewScriptableObject.transitionCurve.Evaluate(normalizedTime));

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
        base.UpdateNode();
    }

    public virtual void UpdateCameraPosition()
    {
        thirdPersonCamera.UpdateCameraPosition(this.trackPos,this.lookPos);
        
    }
}
