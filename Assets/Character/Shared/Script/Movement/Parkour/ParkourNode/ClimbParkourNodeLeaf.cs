using System.Collections.Generic;
using System;
using UnityEngine;

public class ClimbParkourNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    MovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private MovementCompoent movementCompoent;
    private ParkourScriptableObject climbParkourScriptableObject;
    public string nameState { get => climbParkourScriptableObject.stateName; }
    public AnimationClip clip { get => climbParkourScriptableObject.clip; }
    private Transform parkourAble => player.transform;

    private Vector3 enterPos;
    private Vector3 ct1;
    private Vector3 exit;

    private List<Vector3> cts = new List<Vector3>();

    public float timer { get; protected set; }
    public float parkourTimeNormalized { get => climbParkourScriptableObject.curve.Evaluate((timer / clip.length)); }
    private LayerMask obstacleLayer = LayerMask.GetMask("Default");

    private Vector3 obstacleSurfaceDir;
    private float rotateToWardSurfaceDir = 0.2f;
    public ClimbParkourNodeLeaf(Player player, Func<bool> preCondition,MovementCompoent movementCompoent, ParkourScriptableObject climbParkourScriptableObject) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        this.climbParkourScriptableObject = climbParkourScriptableObject;
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
        this.movementCompoent.isOnUpdateEnable = false;
        this.enterPos = player.transform.position;
        BezierurveBehavior.DrawBezierCurve(enterPos, cts, exit, 5);
        base.Enter();
    }
    public override void Exit()
    {
        this.movementCompoent.isOnUpdateEnable = true;
        cts.Clear();
        base.Exit();
    }
    public override void FixedUpdateNode()
    {
        timer += Time.fixedDeltaTime;
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {

        movementCompoent.SetPosition(BezierurveBehavior.GetPointOnBezierCurve(enterPos, cts, exit, parkourTimeNormalized));
        this.MovementRotateToWardSurface();

        base.UpdateNode();
    }
    private void MovementRotateToWardSurface()
    {
        float t = Mathf.Clamp(parkourTimeNormalized / rotateToWardSurfaceDir, 0, rotateToWardSurfaceDir);

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
        Debug.DrawLine(parkourAble.position,parkourAble.position + (parkourAble.forward * climbParkourScriptableObject.detectDistance) ,Color.red,2);
        if(EdgeObstacleDetection.GetEdgeObstaclePos(
            IParkourNodeLeaf.sphereRaduis
            ,10
            ,obstacleSurfaceDir
            , parkourAble.position + (Vector3.up * climbParkourScriptableObject.minHieght)
            , castUpDes
            ,IParkourNodeLeaf.sphereDistanceDifferenc
            ,true
            ,out Vector3 edgePos1
            )
            )
        {
            if (Vector3.Distance(edgePos1, new Vector3(edgePos1.x, parkourAble.position.y, edgePos1.z)) < climbParkourScriptableObject.minHieght)
                return false;

            ct1 = edgePos1
                + (obstacleSurfaceDir * climbParkourScriptableObject.forWardControlPoint_1_offset)
                + (parkourAble.transform.up * climbParkourScriptableObject.upWardControlPoint_1_offset);

            exit = edgePos1
                + (obstacleSurfaceDir * climbParkourScriptableObject.forwardExitPoint_offset)
                + (parkourAble.up * climbParkourScriptableObject.upWardExitPoint_offset);

            cts.Add(ct1);



            return true;
        }
        else
            return false;

       
    }
}
