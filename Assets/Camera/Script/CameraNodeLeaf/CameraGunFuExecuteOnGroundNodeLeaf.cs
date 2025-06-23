using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Threading.Tasks;

public class CameraGunFuExecuteOnGroundNodeLeaf : CameraNodeLeaf,IObserverPlayer 
{
    private Vector3 cameraOffset => cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;

    private CameraExecuteScriptableObject cameraExecuteScriptableObject;
    private float transitionEnterSpeed => cameraExecuteScriptableObject.transitionEnterDuration;
    private float transitionExitSpeed => cameraExecuteScriptableObject.transitionExitDuration;

    private float t;

    private Vector3 targetFollow;
    private Vector3 targetLookAt;

    private enum Phase
    {
        Enter,
        Exit
    }
    private Phase curPhase;
    public CameraGunFuExecuteOnGroundNodeLeaf(CameraController cameraController,CameraExecuteScriptableObject cameraExecuteScriptableObject, Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.cameraExecuteScriptableObject = cameraExecuteScriptableObject;
    }
    public override void Enter()
    {
        t = 0f;
        cameraController.player.AddObserver(this);
        curPhase = Phase.Enter;
        base.Enter();
    }

    public override void Exit()
    {
        cameraController.player.RemoveObserver(this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        switch (curPhase)
        {
            case Phase.Enter:
                {
                    t += Time.deltaTime * (1/transitionEnterSpeed);
                    t = Mathf.Clamp01(t);

                    targetFollow = Vector3.Lerp(thirdPersonCamera.targetFollowTarget.position, GetCentreAnchor(), t);
                    targetLookAt = targetFollow;

                    thirdPersonCamera.InputRotateCamera(inputLook.x, -inputLook.y);
                    thirdPersonCamera.UpdateCameraPosition(targetFollow, targetLookAt);

                    UpdateOffsetEnter();
                }
                break;
            case Phase.Exit:
                {
                    t -= Time.deltaTime * (1 / transitionExitSpeed);
                    t = Mathf.Clamp01(t);

                    targetFollow = Vector3.Lerp(thirdPersonCamera.targetFollowTarget.position, targetFollow, t);
                    targetLookAt = targetFollow;

                    thirdPersonCamera.InputRotateCamera(inputLook.x, -inputLook.y);
                    thirdPersonCamera.UpdateCameraPosition(targetFollow, targetLookAt);

                    RecoverOffsetExit();
                }
                break;

        }

        base.UpdateNode();
    }
    private Vector3 GetCentreAnchor()
    {

        Vector3 playerAnchorPos = cameraController.player.transform.position
            + cameraController.player.transform.up * cameraExecuteScriptableObject.executorAnchorOffset.y
            + cameraController.player.transform.right * cameraExecuteScriptableObject.executorAnchorOffset.x
            + cameraController.player.transform.forward * cameraExecuteScriptableObject.executorAnchorOffset.z;

        Vector3 opponentAnchorPos = cameraController.player.executedAbleGunFu._gunFuAttackedAble.position 
            + cameraController.player.executedAbleGunFu._gunFuAttackedAble.up * cameraExecuteScriptableObject.opponentExecutedAnchorOffset.y
            + cameraController.player.executedAbleGunFu._gunFuAttackedAble.right * cameraExecuteScriptableObject.opponentExecutedAnchorOffset.x
            + cameraController.player.executedAbleGunFu._gunFuAttackedAble.forward * cameraExecuteScriptableObject.opponentExecutedAnchorOffset.z;

        Vector3 centreAnchor = Vector3.Lerp(playerAnchorPos
            , opponentAnchorPos
            , cameraExecuteScriptableObject.weight);

        return centreAnchor + new Vector3(0, cameraExecuteScriptableObject.executorAnchorOffset.y, 0);
    }
    private void UpdateOffsetEnter()
    {
        float cameraOffsetZ = Mathf.Lerp(this.cameraOffset.z,cameraExecuteScriptableObject.cameraOffset.z, t);
        float cameraOffsetX = Mathf.Lerp(this.cameraOffset.x, cameraExecuteScriptableObject.cameraOffset.x,t);
        float cameraOffsetY = Mathf.Lerp(this.cameraOffset.y, cameraExecuteScriptableObject.cameraOffset.y,t);


        thirdPersonCamera.cameraOffset = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
    }
    private void RecoverOffsetExit()
    {
        float cameraOffsetZ = Mathf.Lerp(this.cameraOffset.z, 0, 1-t);
        float cameraOffsetX;
        float cameraOffsetY = Mathf.Lerp(this.cameraOffset.y, cameraController.cameraTPSStandView_SCRP.viewOffsetRight.y, 1 - t);
        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            cameraOffsetX = Mathf.Lerp(this.cameraOffset.x, cameraController.cameraTPSStandView_SCRP.viewOffsetRight.x, 1 - t);
        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            cameraOffsetX = Mathf.Lerp(this.cameraOffset.x, cameraController.cameraTPSStandView_SCRP.viewOffsetRight.x, 1 - t);
        }

        thirdPersonCamera.cameraOffset = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
    }

    public override bool IsReset()
    {
        if(curPhase == Phase.Exit && t <=0)
            { return true; }
        return false;
    }
    private async void ExitPhase()
    {
        await Task.Delay(250);
        curPhase = Phase.Exit;
    }
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
       
    }

    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if(node is GunFuExecuteNodeLeaf gunFuExecuteNodeLeaf 
            && gunFuExecuteNodeLeaf.curPhase == GunFuExecuteNodeLeaf.GunFuExecutePhase.Execute)
            ExitPhase();
    }
}
