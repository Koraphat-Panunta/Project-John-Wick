using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PainStateWalkProceduralAnimateNodeLeaf : AnimationConstrainNodeLeaf
{
    LayerMask stepAbleLayer;

    private enum Turn
    {
        left,
        right
    }

    private Turn curTurn;

    private float weight;

    protected ProceduralLegsWalkConstrainSCRP proceduralLegsWalkConstrainSCRP;

    protected LegsConstrainManager legsConstrainManager;

    public Vector3 oldLeftFootPos;
    public Vector3 newLeftFootPos;
    public Vector3 relativeNewLeftFootPos;
    protected float lerpLeftLeg;

    public Vector3 oldRightFootPos;
    public Vector3 newRightFootPos;
    public Vector3 relativeNewRightFootPos;
    protected float lerpRightLeg;

    public Vector3 anchorHipPos;

    protected Transform hipTransform;
    protected float hipLegsSpace = .26f;

    protected float stepDistance;
    protected float stepHeight;
    protected float stepSpeed;

    private float maxOffset = 1.5f;
    protected Vector3 footplacementOffsetDistance => Vector3.ClampMagnitude(proceduralAnimateNodeManager.FootstepPlacementOffsetDistance * curVelocity, maxOffset);

    protected Vector3 curVelocity => enemy._movementCompoent.curMoveVelocity_World;




    public PainStateWalkProceduralAnimateNodeLeaf(
        LegsConstrainManager legsConstrainManager
        , Transform hipTransform
        , ProceduralLegsWalkConstrainSCRP proceduralLegsWalkConstrainSCRP
        , Func<bool> preCondition
        ) : base(preCondition)
    {
        this.legsConstrainManager = legsConstrainManager;
        this.hipTransform = hipTransform;

        this.proceduralLegsWalkConstrainSCRP = proceduralLegsWalkConstrainSCRP;

        this.stepAbleLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Default");
    }

    public override void Enter()
    {
        curTurn = Turn.left;

        Ray rayLeftLeg = new Ray(hipTransform.position - (hipTransform.right * hipLegsSpace), Vector3.down);

        if (Physics.Raycast(rayLeftLeg, out RaycastHit hitInfoLeft, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoLeft.point + (hipTransform.forward * 0.2f);
            relativeNewLeftFootPos = pos - hipTransform.position;
            this.legsConstrainManager.SetLeftLeg_Target_Foot(pos);
            newLeftFootPos = pos;
            oldLeftFootPos = pos;
        }
        Ray rayRightLeg = new Ray(hipTransform.position + (hipTransform.right * hipLegsSpace), Vector3.down);
        if (Physics.Raycast(rayRightLeg, out RaycastHit hitInfoRight, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoRight.point + (hipTransform.forward * -0.2f);
            relativeNewRightFootPos = pos - hipTransform.position ;
            this.legsConstrainManager.SetRightLeg_Target_Foot(pos);
            newRightFootPos = pos;
            oldRightFootPos = pos;
        }

        Ray rayHip = new Ray(this.hipTransform.position,Vector3.down); 
        if(Physics.Raycast(rayHip, out RaycastHit hitInfoHip, 10, stepAbleLayer))
        {
            this.anchorHipPos = hitInfoHip.point;
        }
        else
            this.anchorHipPos = this.hipTransform.position;


        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {

        RayCastStepCheck();
        UpdateLerpingStep();


      

        base.UpdateNode();
    }

    private void RayCastStepCheck()
    {
        Ray rayHip = new Ray(this.hipTransform.position,Vector3.down);

        if ( this.lerpRightLeg >= 1 && this.lerpLeftLeg >= 1
            && Physics.Raycast(rayHip, out RaycastHit hitInfo, 10, this.stepAbleLayer)
            && Vector3.Distance(this.anchorHipPos, hitInfo.point) > this.stepDistance )
        {
            if(this.curTurn == Turn.left)
            {
                this.lerpLeftLeg = 0;
                this.relativeNewLeftFootPos = (hitInfo.point + this.footplacementOffsetDistance) - this.hipTransform.position;
                this.newLeftFootPos = this.hipTransform.position + this.relativeNewLeftFootPos;
                this.anchorHipPos = this.newLeftFootPos;
            }
            else
            {
                this.lerpRightLeg = 0;
                this.relativeNewRightFootPos = (hitInfo.point + this.footplacementOffsetDistance) - this.hipTransform.position;
                this.newRightFootPos = this.hipTransform.position + this.relativeNewRightFootPos;
                this.anchorHipPos = this.newRightFootPos;
            }
           
        }

       
    }
    private void UpdateLerpingStep()
    {
        if (lerpLeftLeg < 1)
        {
            Vector3 posL = Vector3.Lerp(oldLeftFootPos, newLeftFootPos, lerpLeftLeg);
            posL.y += Mathf.Sin(lerpLeftLeg * Mathf.PI) * stepHeight;
            lerpLeftLeg += Time.deltaTime * stepSpeed;

            leftLeg.data.target.position = posL;
            newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;

            if(lerpLeftLeg >= 1)
            {
                newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;
                oldLeftFootPos = newLeftFootPos;
                leftLeg.data.target.position = oldLeftFootPos;
                curTurn = Turn.right;
            }
        }
        else
        {
            newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;
            oldLeftFootPos = newLeftFootPos;
            leftLeg.data.target.position = oldLeftFootPos;
        }
        

        if (lerpRightLeg < 1)
        {
            Vector3 posR = Vector3.Lerp(oldRightFootPos, newRightFootPos, lerpRightLeg);
            posR.y += Mathf.Sin(lerpRightLeg * Mathf.PI) * stepHeight;
            lerpRightLeg += Time.deltaTime * stepSpeed;

            rightLeg.data.target.position = posR;
            newRightFootPos = hipTransform.position + relativeNewRightFootPos;

            if(lerpRightLeg >= 1)
            {
                newRightFootPos = hipTransform.position + relativeNewRightFootPos;
                oldRightFootPos = newRightFootPos;
                rightLeg.data.target.position = oldRightFootPos;
                curTurn = Turn.left;
            }
        }
        else
        {
            newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;
            oldRightFootPos = newRightFootPos;
            rightLeg.data.target.position = oldRightFootPos;
        }


       
    }
    
}
