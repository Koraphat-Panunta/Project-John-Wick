using System.Collections.Generic;
using System;
using UnityEngine;

public class ClimbParkourNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    EdgeObstacleDetection IParkourNodeLeaf._edgeObstacleDetection { get => edgeObstacleDetection; set => edgeObstacleDetection = value; }
    IMovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private EdgeObstacleDetection edgeObstacleDetection;
    private IMovementCompoent movementCompoent;
    private ParkourScriptableObject climbParkourScriptableObject;
    public string nameState { get => climbParkourScriptableObject.stateName; }
    public AnimationClip clip { get => climbParkourScriptableObject.clip; }
    private Transform parkourAble => player.transform;

    private Vector3 enterPos;
    private Vector3 ct1;
    private Vector3 exit;

    private List<Vector3> cts = new List<Vector3>();

    public float timer { get; protected set; }
    public float parkourTimeNormalized { get => (timer / clip.length); }
    public ClimbParkourNodeLeaf(Player player, Func<bool> preCondition,IMovementCompoent movementCompoent, ParkourScriptableObject climbParkourScriptableObject) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        this.climbParkourScriptableObject = climbParkourScriptableObject;
        this.edgeObstacleDetection = new EdgeObstacleDetection();
    }
    public override bool Precondition()
    {
        if(base.Precondition() == false)
            return false;

        if(CheckEdge())
            return true;
        return false;

    }
    public override bool IsComplete()
    {
        if(parkourTimeNormalized >= 1)
            return true;
        return false;
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
        timer = 0;
        this.movementCompoent.CancleMomentum();
        this.movementCompoent.isEnable = false;
        this.enterPos = player.transform.position;

        base.Enter();
    }
    public override void Exit()
    {
        this.movementCompoent.isEnable = true;
        cts.Clear();
        base.Exit();
    }
    public override void UpdateNode()
    {
        timer += Time.deltaTime;
        BezierurveMove.MoveTransformOnBezierCurve(parkourAble, enterPos, cts, exit, parkourTimeNormalized);

        base.UpdateNode();
    }
    private bool CheckEdge()
    {
        Vector3 castUpDes = parkourAble.position + (Vector3.up*climbParkourScriptableObject.hieght);

        if(edgeObstacleDetection.GetEdgeObstaclePos(
            IParkourNodeLeaf.sphereRaduis
            ,parkourAble.forward
            ,parkourAble.position
            ,castUpDes
            ,IParkourNodeLeaf.sphereDistanceDifferenc
            ,out Vector3 edgePos1)
            )
        {
            ct1 = edgePos1
                + (parkourAble.transform.forward * climbParkourScriptableObject.forWardControlPoint_1_offset)
                + (parkourAble.transform.up * climbParkourScriptableObject.upWardControlPoint_1_offset);

            exit = ct1
                + (parkourAble.forward * climbParkourScriptableObject.forwardExitPoint_offset)
                + (parkourAble.up * climbParkourScriptableObject.upWardExitPoint_offset);

            cts.Add(ct1);

            return true;
        }
        else
            return false;

       
    }
}
