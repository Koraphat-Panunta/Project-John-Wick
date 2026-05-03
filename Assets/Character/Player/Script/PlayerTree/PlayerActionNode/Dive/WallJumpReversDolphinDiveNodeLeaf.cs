using System;
using UnityEngine;

public class WallJumpReversDolphinDiveNodeLeaf : PlayerDolphinDiveStateNodeLeaf
{

    public float anticipateTime = .25f;

    public override float jumpOutTime => 0;

    protected override float stallMinimumTime => .2f;

    protected override float jumpVerticalVelocuty => base.jumpVerticalVelocuty * 1.2f;

    protected Vector3 enterPos;
    protected Vector3 wallPos;
    protected Vector3 wallNormal;
    protected Vector3 toWallDir;
    public enum WallJumpPhase
    {
        Anticipate,
        Jump
    }

    public WallJumpPhase phase;

    private LayerMask obstacleLayer = LayerMask.GetMask("Default");

    public float jumpRotateDuration => .2f;
    public float jumpRotateTimer;

    public WallJumpReversDolphinDiveNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        this.playerMovement.CancleMomentum();
        this.playerMovement.isOnUpdateEnable = false;
        this.CalculateJumpOutDir();
        this.phase = WallJumpPhase.Anticipate;
        this.playerMovement.characterController.PushForceUp(1,.05f);
        this.jumpRotateTimer = 0;
        base.Enter();
    }
    public override void Exit()
    {
        this.playerMovement.isOnUpdateEnable = true;
        base.Exit();
    }
    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;

        this.enterPos = this.player.transform.position;

        Vector3 castPos = this.player.transform.position + (Vector3.up * 1);
        Vector3 castDir = (this.playerMovement.curMoveVelocity_World.normalized + this.player.inputMoveDir_World.normalized).normalized;

        if (Physics.Raycast(castPos, castDir, out RaycastHit hit, 2, obstacleLayer, QueryTriggerInteraction.Ignore))
        {

            this.wallPos = hit.point + (hit.normal * .25f);
            this.wallNormal = hit.normal;
            this.toWallDir = (this.wallPos - this.enterPos).normalized;

            if (Vector3.Dot(new Vector3(this.toWallDir.x,0,this.toWallDir.z).normalized * -1,new Vector3(this.wallNormal.x,0,this.wallNormal.z).normalized) < .92f)
                return false;

            Debug.DrawLine(castPos, wallPos, Color.red, 5);
            return true;
        }

        return false;
    }

    public override void FixedUpdateNode()
    {
        this.timer += Time.fixedDeltaTime;

        if (this.phase == WallJumpPhase.Anticipate)
        {
            float t = Mathf.Clamp01(this.timer / this.anticipateTime);

            this.playerMovement.SetPosition(Vector3.Lerp(this.enterPos, this.wallPos, t));
            this.playerMovement.SetRotateToDirWorldSlerp(this.jumpDir * -1, t);
            Debug.DrawLine(this.wallPos, this.player.transform.position, Color.blue, 5);


            if (this.timer > this.anticipateTime)
            {
                this.playerMovement.SetProneDir(this.jumpDir);
                this.playerMovement.isOnUpdateEnable = true;
                this.phase = WallJumpPhase.Jump;
                this.player.NotifyObserver(this.player, this);
            }
        }
        else if (this.phase == WallJumpPhase.Jump)
        {


            this.UpdateJumpOut();
            this.UpdateStall();

            if (this.stallTimeCountDown <= 0)
            {
                if (this.jumpRotateTimer < this.jumpRotateDuration)
                    this.jumpRotateTimer = Mathf.Clamp(this.jumpRotateTimer + Time.fixedDeltaTime, 0, this.jumpRotateDuration);

                this.playerMovement.SetRotation(Quaternion.LookRotation(Vector3.Lerp(this.jumpDir * -1, this.player.cinemachineCamera.targetDir, this.jumpRotateTimer / this.jumpRotateDuration)));
            }


        }

        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {


    }
    protected override void CalculateJumpOutDir()
    {

        this.jumpDir = Vector3.Reflect(this.toWallDir, this.wallNormal);
            this.jumpDir = new Vector3(this.jumpDir.x,0,this.jumpDir.z).normalized;


    }
}
