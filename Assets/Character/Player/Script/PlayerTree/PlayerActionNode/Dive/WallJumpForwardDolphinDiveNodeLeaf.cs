using System;
using UnityEngine;

public class WallJumpForwardDolphinDiveNodeLeaf : PlayerDolphinDiveStateNodeLeaf
{
    public float anticipateTime = .25f;

    public override float jumpOutTime => anticipateTime + .3f;

    protected override float stallMinimumTime => .2f;

    protected override float jumpVerticalVelocuty => base.jumpVerticalVelocuty * 1.2f;

    public Vector3 alighWallDir;
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

    public bool isJumpLeft { get; protected set; }

    public WallJumpForwardDolphinDiveNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        this.playerMovement.CancleMomentum();
        this.playerMovement.isOnUpdateEnable = false;
        this.CalculateJumpOutDir();
        this.phase = WallJumpPhase.Anticipate;
        this.playerMovement.characterController.PushForceUp(1, .05f);
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

        Vector3 castPos = this.player.transform.position + (Vector3.up *1);
        Vector3 castDir = (this.playerMovement.curMoveVelocity_World.normalized + this.player.inputMoveDir_World.normalized).normalized;

        if (Physics.Raycast(castPos, castDir, out RaycastHit hit, 3, obstacleLayer, QueryTriggerInteraction.Ignore))
        {

            this.wallPos = hit.point + (hit.normal * .2f);
            this.wallNormal = hit.normal;
            this.toWallDir = (this.wallPos - this.enterPos).normalized;

            this.alighWallDir = Vector3.ProjectOnPlane(this.toWallDir, this.wallNormal);
            this.alighWallDir = new Vector3(this.alighWallDir.x, 0, this.alighWallDir.z).normalized;

            if (Vector3.Dot(this.player.transform.right, new Vector3(this.wallNormal.x, 0, this.wallNormal.z).normalized) > 0)
                this.isJumpLeft = true;
            else
                this.isJumpLeft = false;

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
            this.playerMovement.SetRotateToDirWorldSlerp(this.alighWallDir, t);
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

            if (this.isPassingJump)
            {
                if (this.jumpRotateTimer < this.jumpRotateDuration)
                    this.jumpRotateTimer = Mathf.Clamp(this.jumpRotateTimer + Time.fixedDeltaTime, 0, this.jumpRotateDuration);

                this.playerMovement.SetRotation(Quaternion.LookRotation(Vector3.Lerp(this.alighWallDir, this.player.cinemachineCamera.targetDir, this.jumpRotateTimer / this.jumpRotateDuration)));
            }


        }

    }

    protected override void UpdateJumpOut()
    {
        Vector3 deltaPos = this.playerMovement.curPosition - this.player.humanoidBone._leftFootBone.position; 

        if(this.isJumpLeft == false) 
            deltaPos = this.playerMovement.curPosition - this.player.humanoidBone._rightFootBone.position;

        if(this.isPassingJump == false)
        {
            this.playerMovement.SetPosition(Vector3.Lerp(this.playerMovement.curPosition, this.wallPos + deltaPos,Time.fixedDeltaTime * 10f));
        }
        base.UpdateJumpOut();
    }

    public override void UpdateNode()
    {
       

    }
    protected override void CalculateJumpOutDir()
    {
        this.jumpDir = Vector3.Reflect(this.toWallDir, this.wallNormal);
        this.jumpDir = new Vector3(this.jumpDir.x, 0, this.jumpDir.z).normalized;


    }
}
