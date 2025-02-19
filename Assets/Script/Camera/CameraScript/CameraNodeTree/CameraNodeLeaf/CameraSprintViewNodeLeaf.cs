using Cinemachine;
using System;
using UnityEngine;

public class CameraSprintViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cameraOffset => cameraController.cameraOffset;
    private CinemachineFreeLook cinemachineFreeLook => cameraController.CinemachineFreeLook;
    private ScrpCameraViewAttribute sprintView => cameraController.cameraViewAttribute;

    private float speedEnterView = 1.4f;
    public CameraSprintViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
    }

    public override void UpdateNode()
    {
        ScrpCameraViewAttribute viewAttribute = this.cameraController.cameraViewAttribute;
        float offsetX;

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(this.cameraOffset.m_Offset.x,
                viewAttribute.Sprint_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cameraOffset.m_Offset.x,
                -viewAttribute.Sprint_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.m_Lens.FieldOfView, viewAttribute.Sprint_FOV, speedEnterView * Time.deltaTime);

        float offsetY = Mathf.Lerp(this.cameraOffset.m_Offset.y, viewAttribute.Sprint_Offset_Right.y, speedEnterView * Time.deltaTime);
        float offsetZ = Mathf.Lerp(this.cameraOffset.m_Offset.z, viewAttribute.Sprint_Offset_Right.z, speedEnterView * Time.deltaTime);

        this.cameraOffset.m_Offset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
