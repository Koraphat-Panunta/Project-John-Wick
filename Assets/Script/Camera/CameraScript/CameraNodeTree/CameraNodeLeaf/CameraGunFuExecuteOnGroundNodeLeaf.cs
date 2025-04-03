using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraGunFuExecuteOnGroundNodeLeaf : CameraNodeLeaf
{
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private ThirdPersonCinemachineCamera executeThirdPersonCinemachineCamera => base.cameraController.executeThirdPersonCinemachineCameara; 
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;
    private Vector3 executeTarget => cameraController.player.executedAbleGunFu.attackedPos;

    private Vector3 targetFollow 
    { get 
        {
            return Vector3.MoveTowards(thirdPersonCamera.targetFollowTarget.position
                , executeTarget
                , Vector3.Distance(thirdPersonCamera.targetFollowTarget.position, executeTarget)/2);
        } 

    }
    private Vector3 targetLook 
    { 
        get 
        {
            return Vector3.MoveTowards(thirdPersonCamera.targetLookTarget.position
                , executeTarget
                , Vector3.Distance(thirdPersonCamera.targetLookTarget.position, executeTarget) / 2);
        } 
    }
    public CameraGunFuExecuteOnGroundNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {

    }
    public override void Enter()
    {

        cameraController.ChangeCamera(executeThirdPersonCinemachineCamera.cinemachineCamera, 0.2f);
        base.Enter();
    }

    public override void Exit()
    {
        cameraController.ChangeCamera(thirdPersonCamera.cinemachineCamera, 0.2f);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {

        //thirdPersonCamera.InputRotateCamera(inputLook.x,-inputLook.y);
        //executeThirdPersonCinemachineCamera.InputRotateCamera(inputLook.x, -inputLook.y);

        executeThirdPersonCinemachineCamera.UpdateCameraPosition(this.targetFollow, this.targetLook);

       
        base.UpdateNode();
    }
}
