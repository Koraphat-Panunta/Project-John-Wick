using System.Collections.Generic;
using System;
using UnityEngine;

public class ClimbParkourNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    MovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private MovementCompoent movementCompoent;
    private ClimbParkourScriptableObject climbParkourScriptableObject;
    public string nameState { get => climbParkourScriptableObject.stateName; }
    private Transform parkourAble => player.transform;

    private Vector3 enterPos;
    private Vector3 startClimbPos;
    private Vector3 ct1;

    private List<Vector3> cts = new List<Vector3>();

    private readonly AnimationTriggerEventPlayer _animTriggerPlayer;
    private bool _isClimbPhase;
    private readonly float _warpEventNormalized;

    private LayerMask obstacleLayer = LayerMask.GetMask("Default");

    private Vector3 obstacleSurfaceDir;
    private float rotateToWardSurfaceDir = 0.2f;

    private const string CLIMB_EVENT = "Climb";

    public ClimbParkourNodeLeaf(Player player, Func<bool> preCondition,MovementCompoent movementCompoent, ClimbParkourScriptableObject climbParkourScriptableObject) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        this.climbParkourScriptableObject = climbParkourScriptableObject;

        _animTriggerPlayer = new AnimationTriggerEventPlayer(climbParkourScriptableObject.animationTriggerEventSCRP);
        _animTriggerPlayer.SubscribeEvent(CLIMB_EVENT, OnClimbPhase);
        _warpEventNormalized = _animTriggerPlayer.GetEventNormalizedTime(CLIMB_EVENT);
    }
    public override bool Precondition()
    {
        if(base.Precondition() == false)
            return false;

        if ((Physics.Raycast(parkourAble.position, parkourAble.forward, out RaycastHit hit, this.climbParkourScriptableObject.detectDistance, obstacleLayer)
            && Vector3.Dot(hit.normal * -1, parkourAble.forward.normalized) > 0.7f && Vector3.Dot(hit.normal * -1, movementCompoent.moveInputVelocity_World.normalized) > 0.7f) == false)
            return false;

        obstacleSurfaceDir = (hit.normal*-1).normalized;

        if (CheckEdge())
        {
            return true;
        }
        return false;

    }
    public override bool IsComplete()
    {
        return _animTriggerPlayer.IsPlayFinish();
    }
    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        if(player.isDead)
            return true ;
        return false;
    }
    public override void Enter()
    {
        _isClimbPhase = false;
        this.player.enableRootMotion = false;
        this.movementCompoent.CancleMomentum();
        this.movementCompoent.isOnUpdateEnable = false;
        this.player.playerMovement.characterController.PushForceUp(1, 0.05f);
        this.enterPos = player.transform.position;
        _animTriggerPlayer.Rewind();
        base.Enter();
    }
    public override void Exit()
    {
        this.player.enableRootMotion = false;
        this.player.rootMotionScale = Vector3.one;
        this.movementCompoent.isOnUpdateEnable = true;
        cts.Clear();
        base.Exit();
    }
    public override void UpdateNode()
    {
        _animTriggerPlayer.UpdatePlay(Time.deltaTime);
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        // Climb phase: baked root motion drives position/rotation (see Character.HandleAnimatorMove).
        if (_isClimbPhase)
        {
            base.FixedUpdateNode();
            return;
        }

        // Warp phase: code-driven bezier arc from the entry point to the tuned warp destination.
        float t = _animTriggerPlayer.GetRemapNormalizedTimer(_animTriggerPlayer.enterNormalizedTime, _warpEventNormalized);
        movementCompoent.SetPosition(BezierurveBehavior.GetPointOnBezierCurve(enterPos, cts, startClimbPos, t));
        this.MovementRotateToWardSurface(t);
        base.FixedUpdateNode();
    }

    // Hand-off: snap exactly to the tuned warp destination, then let scaled root motion finish the climb.
    private void OnClimbPhase()
    {
        movementCompoent.SetPosition(startClimbPos);
        this.player.rootMotionScale = climbParkourScriptableObject.rootMotionScale;
        this.player.enableRootMotion = true;
        _isClimbPhase = true;
    }

    private void MovementRotateToWardSurface(float warpNormalized)
    {
        float t = Mathf.Clamp(warpNormalized / rotateToWardSurfaceDir, 0, rotateToWardSurfaceDir);

        Quaternion rotate = Quaternion.Lerp(
            Quaternion.LookRotation(parkourAble.forward, Vector3.up)
            , Quaternion.LookRotation(obstacleSurfaceDir, Vector3.up)
            , t);
        movementCompoent.SetRotation(rotate);
    }
    private bool CheckEdge()
    {
        cts.Clear();
        Vector3 castUpDes = parkourAble.position + (Vector3.up*climbParkourScriptableObject.hieght);
        if(EdgeObstacleDetection.GetEdgeObstaclePos(
            IParkourNodeLeaf.sphereRaduis
            ,this.climbParkourScriptableObject.detectDistance
            ,obstacleSurfaceDir
            , parkourAble.position + (Vector3.up * climbParkourScriptableObject.minHieght)
            , castUpDes
            ,IParkourNodeLeaf.sphereDistanceDifferenc
            ,true
            ,out Vector3 edgePos1
            )
            )
        {
            //Debug.DrawLine(parkourAble.position + (Vector3.up * climbParkourScriptableObject.minHieght), edgePos1, Color.red, 2f);

            if (Vector3.Distance(edgePos1, new Vector3(edgePos1.x, parkourAble.position.y, edgePos1.z)) < climbParkourScriptableObject.minHieght)
                return false;

            this.startClimbPos = edgePos1
                + (obstacleSurfaceDir * climbParkourScriptableObject.forwardExitPoint_offset)
                + (parkourAble.transform.up * climbParkourScriptableObject.upWardExitPoint_offset);

            ct1 = edgePos1
                + (obstacleSurfaceDir * climbParkourScriptableObject.forWardControlPoint_1_offset)
                + (parkourAble.transform.up * climbParkourScriptableObject.upWardControlPoint_1_offset);

            cts.Add(ct1);

            return true;
        }
        else
            return false;


    }
}
