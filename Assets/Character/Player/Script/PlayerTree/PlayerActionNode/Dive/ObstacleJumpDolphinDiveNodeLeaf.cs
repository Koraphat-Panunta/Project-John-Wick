using System;
using UnityEngine;

public class ObstacleJumpDolphinDiveNodeLeaf : PlayerDolphinDiveStateNodeLeaf
{
    public float anticipateTime = .45f;

    public override float jumpOutTime => this.anticipateTime + .15f;

    protected override float stallMinimumTime => .2f;

    protected override float jumpVerticalVelocuty => base.jumpVerticalVelocuty * 1.2f;



    protected Vector3 enterPos;
    protected Vector3 obstaclePos;
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

    public ObstacleJumpDolphinDiveNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        this.CalculateJumpOutDir();
        this.playerMovement.CancleMomentum();
        this.playerMovement.isOnUpdateEnable = false;
        this.phase = WallJumpPhase.Anticipate;
        this.playerMovement.characterController.PushForceUp(1, .05f);
        this.playerMovement.characterController.enableDynamicCollider = true;
        this.jumpRotateTimer = 0;
        base.Enter();
    }
    public override void Exit()
    {
        this.playerMovement.isOnUpdateEnable = true;
        this.playerMovement.characterController.enableDynamicCollider = false;
        base.Exit();
    }
    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;

        this.enterPos = this.player.transform.position;

        Vector3 castPos = this.player.transform.position;
        Vector3 castDesPos = this.player.transform.position + (Vector3.up * 1.25f);
        Vector3 castDir = (this.playerMovement.curMoveVelocity_World + this.player.inputMoveDir_World.normalized).normalized;

        float castDistance = 3;

        if (Physics.Raycast(castPos + (Vector3.up * .5f) , castDir, out RaycastHit hit, castDistance, obstacleLayer, QueryTriggerInteraction.Ignore) == false)
            return false;
        

        //Debug.DrawRay(castPos, castDir * castDistance, Color.red, 5);

        if (EdgeObstacleDetection.GetEdgeObstaclePos(IParkourNodeLeaf.sphereRaduis
            ,castDistance + IParkourNodeLeaf.sphereDistanceDifferenc
            ,castDir
            ,castPos
            ,castDesPos
            ,IParkourNodeLeaf.sphereDistanceDifferenc
            ,true
            ,out Vector3 edgePos
            ))
        {
            this.obstaclePos = edgePos + (Vector3.up * .1f);
            return true;
        }

        return false;


    }

    protected Vector3 deltaFootPos => this.playerMovement.curPosition - this.player._leftFootBone.transform.position  ;
    public override void FixedUpdateNode()
    {
        this.timer += Time.fixedDeltaTime;

        if (this.phase == WallJumpPhase.Anticipate)
        {
            float t = Mathf.Clamp01(this.timer / this.anticipateTime);

            this.playerMovement.SetPosition(Vector3.Lerp(this.enterPos, this.obstaclePos + deltaFootPos, t));

            this.playerMovement.SetRotateToDirWorldSlerp(this.jumpDir, t);
            Debug.DrawLine(this.obstaclePos, this.player.transform.position, Color.blue, 5);


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

                this.playerMovement.SetRotation(Quaternion.LookRotation(Vector3.Lerp(this.jumpDir, this.player.cinemachineCamera.targetDir, this.jumpRotateTimer / this.jumpRotateDuration)));
            }


        }

    }
    public override void UpdateNode()
    {
       

    }
    protected override void UpdateJumpOut()
    {
        if(this.isPassingJump == false)
        {
            this.playerMovement.SetPosition(Vector3.Lerp(this.playerMovement.curPosition, this.obstaclePos + deltaFootPos,Time.fixedDeltaTime * 40));
        }
        else
        {
            this.playerMovement.characterController.enableDynamicCollider = false;
        }
       
        base.UpdateJumpOut();
    }
    protected override void CalculateJumpOutDir()
    {
        this.jumpDir = (this.playerMovement.curMoveVelocity_World + this.player.inputMoveDir_World.normalized * Mathf.Clamp(this.playerMovement.curMoveVelocity_World.magnitude,1, this.playerMovement.curMoveVelocity_World.magnitude)).normalized;
        this.jumpDir = new Vector3(this.jumpDir.x,0,this.jumpDir.z).normalized;
    }
}
