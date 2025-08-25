using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class VaultingNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    MovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private MovementCompoent movementCompoent;
    private VaultingParkourScriptableObject vaultingParkourScriptableObject { get; set; }
    public string nameState { get => vaultingParkourScriptableObject.stateName; }
    public AnimationClip clip { get => vaultingParkourScriptableObject.clip; }

    private Vector3 enterPos;
    private Vector3 exitPos;

    private List<Vector3> cts = new List<Vector3>();

    public float timer { get; protected set; }
    public float parkourTimeNormalized { get => vaultingParkourScriptableObject.curve.Evaluate((timer / clip.length)); }
    private LayerMask obstacleLayer = LayerMask.GetMask("Default");
    Transform parkourAble => player.transform;

    private Vector3 enterVelocity;

    public VaultingNodeLeaf(Player player, Func<bool> preCondition,MovementCompoent movementCompoent,VaultingParkourScriptableObject vaultingParkourScriptableObject) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        this.vaultingParkourScriptableObject = vaultingParkourScriptableObject;
    }

    public override bool Precondition()
    {
        if (base.Precondition() == false)
            return false;


        if ((Physics.Raycast(parkourAble.position, parkourAble.forward, out RaycastHit hit, this.vaultingParkourScriptableObject.detectDistance, obstacleLayer)
            && Vector3.Dot(hit.normal * -1, parkourAble.forward.normalized) > 0.72f && Vector3.Dot(hit.normal * -1, movementCompoent.moveInputVelocity_World.normalized) > 0.72f) == false)
            return false;


        if (CheckEdge())
        {
            return true;
        }
        return false;

    }
    public override bool IsComplete()
    {
        if (parkourTimeNormalized >= 1)
            return true;
        return false;
    }
    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        if (player.isDead)
            return true;
        return false;
    }
    public override void Enter()
    {
        timer = 0;
        enterVelocity = this.movementCompoent.curMoveVelocity_World;
        this.movementCompoent.CancleMomentum();
        this.movementCompoent.isOnUpdateEnable = false;
        this.enterPos = player.transform.position;
        BezierurveBehavior.DrawBezierCurve(enterPos, cts, exitPos, 5);
        base.Enter();
    }
    public override void Exit()
    {
        this.movementCompoent.isOnUpdateEnable = true;
        if (this.movementCompoent is IMotionImplusePushAble motionImplusePush)
            motionImplusePush.AddForcePush(enterVelocity, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
        cts.Clear();
        base.Exit();
    }
    public override void UpdateNode()
    {
        timer += Time.deltaTime;
        movementCompoent.SetPosition(BezierurveBehavior.GetPointOnBezierCurve(enterPos, cts, exitPos, parkourTimeNormalized));


        base.UpdateNode();
    }
    private bool CheckEdge()
    {
        cts.Clear();
        Vector3 castUpDes = parkourAble.position + (Vector3.up * vaultingParkourScriptableObject.hieght);
        Debug.DrawLine(parkourAble.position, parkourAble.position + (parkourAble.forward * vaultingParkourScriptableObject.detectDistance), Color.red, 2);
        if (EdgeObstacleDetection.GetEdgeObstaclePos(
            IParkourNodeLeaf.sphereRaduis
            , 10
            , parkourAble.forward
            , parkourAble.position
            , castUpDes
            , IParkourNodeLeaf.sphereDistanceDifferenc
            , out Vector3 edgePos1
            )
            )
        {


            Vector3 startPos = parkourAble.position;
            Vector3 ct1
                = edgePos1
                + parkourAble.forward * vaultingParkourScriptableObject.forWardControlPoint_1_offset
                + parkourAble.up * vaultingParkourScriptableObject.upWardControlPoint_1_offset;

            cts.Add(ct1);
        }
        else
            return false;

        Vector3 startCastDown = edgePos1 + (Vector3.up * 1);
        Vector3 destinationCaswDown = startCastDown + parkourAble.forward * vaultingParkourScriptableObject.vaultingLenght;

        if(Physics.Raycast(startCastDown,parkourAble.forward,vaultingParkourScriptableObject.vaultingLenght,obstacleLayer))
            return false;

        if (EdgeObstacleDetection.GetEdgeObstaclePos(
            IParkourNodeLeaf.sphereRaduis
            , 10
            , Vector3.down
            , startCastDown
            , destinationCaswDown
            , IParkourNodeLeaf.sphereDistanceDifferenc
            , out Vector3 edgePos2
            , out List<Vector3> sphereLine))
        {
            Vector3 ct2
                   = edgePos2
                   + parkourAble.forward * vaultingParkourScriptableObject.forwardControlPoint_2_offset
                   + parkourAble.up * vaultingParkourScriptableObject.upWardControlPoint_2_offset;

            cts.Add(ct2);

            exitPos
                = edgePos2
                + parkourAble.forward * vaultingParkourScriptableObject.forwardExitPoint_offset
                + parkourAble.up * vaultingParkourScriptableObject.upWardExitPoint_offset;

          return true;
        }
        else
            return false;


    }


}
