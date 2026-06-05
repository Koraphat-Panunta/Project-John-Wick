using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraThirdPersonSnapLookControllerNodeLeaf : CameraNodeLeaf
{
    public ThirdPersonCinemachineCamera thirdPersonCinemachineCamera { get => this.cameraController.thirdPersonCinemachineCamera; }

    public Transform targetTrackPosition { get; protected set; }
    public Transform targetLookPosition { get; protected set; }

    public CameraSnapLookScriptableObject snapLookSCRP { get; protected set; }

    public Vector3 targetlookDir { get; protected set; }
    public Vector3 enterLookDir { get; protected set; }

    public float timer { get; protected set; }
    public float snapLookDuration { get; protected set; }

    public bool triggerTransition => _triggerTransition;

    private bool _triggerTransition;
    private bool _isTriggerReset;

    private float _enterYaw;
    private float _enterPitch;
    private float _targetYaw;
    private float _targetPitch;
    private Vector3 _enterOffset;
    private float _enterFOV;

    private CinemachineCamera _cinemachineCamera;

    public CameraThirdPersonSnapLookControllerNodeLeaf(
        CameraController cameraController,
        Transform targetTrackPosition,
        Transform targetLookPosition,
        CameraSnapLookScriptableObject snapLookSCRP,
        Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.targetTrackPosition = targetTrackPosition;
        this.targetLookPosition = targetLookPosition;
        this.snapLookSCRP = snapLookSCRP;
        this.snapLookDuration = snapLookSCRP.duration;
        _cinemachineCamera = cameraController.thirdPersonCinemachineCamera.cinemachineCamera;
    }

    public override void Enter()
    {
        timer = 0;
        _isTriggerReset = false;
        enterLookDir = thirdPersonCinemachineCamera.transform.forward;

        _enterYaw = thirdPersonCinemachineCamera.yaw;
        _enterPitch = thirdPersonCinemachineCamera.pitch;
        _enterOffset = thirdPersonCinemachineCamera.cameraOffset;
        _enterFOV = _cinemachineCamera.Lens.FieldOfView;

        if (targetlookDir != Vector3.zero)
        {
            thirdPersonCinemachineCamera.InputRotateCameraToDirection(targetlookDir);
            _targetYaw = thirdPersonCinemachineCamera.yaw;
            _targetPitch = thirdPersonCinemachineCamera.pitch;
            thirdPersonCinemachineCamera.SetYaw(_enterYaw);
            thirdPersonCinemachineCamera.SetPitch(_enterPitch);
        }

        base.Enter();
    }

    public override void UpdateNode()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / snapLookDuration);
        float curvedT = snapLookSCRP.transitionCurve.Evaluate(t);

        thirdPersonCinemachineCamera.SetYaw(Mathf.LerpAngle(_enterYaw, _targetYaw, curvedT));
        thirdPersonCinemachineCamera.SetPitch(Mathf.Lerp(_enterPitch, _targetPitch, curvedT));

        float targetOffsetX = cameraController.curSide == Side.Right
            ? snapLookSCRP.viewSCRP.viewOffsetRight.x
            : -snapLookSCRP.viewSCRP.viewOffsetRight.x;
        Vector3 targetOffset = new Vector3(targetOffsetX, snapLookSCRP.viewSCRP.viewOffsetRight.y, snapLookSCRP.viewSCRP.viewOffsetRight.z);
        thirdPersonCinemachineCamera.cameraOffset = Vector3.Lerp(_enterOffset, targetOffset, curvedT);

        _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_enterFOV, snapLookSCRP.viewSCRP.fov, curvedT);

        Vector3 trackPos = targetTrackPosition != null ? targetTrackPosition.position : thirdPersonCinemachineCamera.curTrackPosition;
        Vector3 lookPos = targetLookPosition != null ? targetLookPosition.position : thirdPersonCinemachineCamera.curLookPosition;
        thirdPersonCinemachineCamera.UpdateCameraPosition(trackPos, lookPos);

        base.UpdateNode();
    }

    public override void Exit()
    {
        _triggerTransition = false;
        base.Exit();
    }

    public override bool IsComplete()
    {
        return timer >= snapLookDuration;
    }

    public override bool IsReset()
    {
        if (_isTriggerReset) return true;
        return IsComplete();
    }

    public void SetLookDir(Vector3 lookDir) => targetlookDir = lookDir;

    public void SetDuration(float duration) => snapLookDuration = duration;

    public void SetSCRP(CameraSnapLookScriptableObject scrp)
    {
        snapLookSCRP = scrp;
        snapLookDuration = scrp.duration;
    }

    public void TriggerCameraTransition() => _triggerTransition = true;

    public void TriggerReset()
    {
        _isTriggerReset = true;
        _triggerTransition = false;
    }

}
