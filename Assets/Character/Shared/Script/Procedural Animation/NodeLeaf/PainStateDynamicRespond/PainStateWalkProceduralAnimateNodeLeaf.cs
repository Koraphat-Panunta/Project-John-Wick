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

    protected Transform hipTransform;

    protected float hipLegsSpace = .26f;

    protected Quaternion leftFootRot
    {
        get 
        {
            Vector3 dir = this.hipTransform.forward;
            return Quaternion.LookRotation(new Vector3(dir.x,0,dir.z).normalized,this.hipTransform.up);
        }
    }

    protected Quaternion rightFootRot
    {
        get
        {
            Vector3 dir = this.hipTransform.forward;
            return Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z).normalized, this.hipTransform.up);
        }
    }

    protected Vector3 hipRootPos { 
        get
        {
            Ray rayHip = new Ray(this.hipTransform.position,Vector3.down);

            if (Physics.Raycast(rayHip, out RaycastHit hitInfo, 1.5f, this.stepAbleLayer, QueryTriggerInteraction.Ignore))
            {
                return hitInfo.point;
            }
            else
                return rayHip.GetPoint(1.5f);
        } 
    }
    protected Vector3 leftLegRootPos 
    {
        get
        {
            Ray rayLeftLeg = new Ray(this.hipTransform.position - (this.hipTransform.right * this.hipLegsSpace),Vector3.down);

            if (Physics.Raycast(rayLeftLeg, out RaycastHit hitInfo, 1.5f, this.stepAbleLayer, QueryTriggerInteraction.Ignore))
            {
                return hitInfo.point + Vector3.up * .1f;
            }
            else
                return rayLeftLeg.GetPoint(1.5f) + Vector3.up * .1f;
        }
    }
    protected Vector3 rightLegRootPos
    {
        get
        {
            Ray rayRightLeg = new Ray(this.hipTransform.position + (this.hipTransform.right * this.hipLegsSpace), Vector3.down);

            if (Physics.Raycast(rayRightLeg, out RaycastHit hitInfo, 1.5f, this.stepAbleLayer, QueryTriggerInteraction.Ignore))
            {
                return hitInfo.point + Vector3.up * .1f;
            }
            else
                return rayRightLeg.GetPoint(1.5f) + Vector3.up * .1f;
        }
    }

    public float distanceLeftLeg { get => Vector3.Distance(this.leftLegRootPos, this.newLeftFootPos); }
    public float distanceRightLeg { get => Vector3.Distance(this.rightLegRootPos, this.newRightFootPos); }

    protected float distanceBeginStep { get => this.proceduralLegsWalkConstrainSCRP.distanceBeginStep; }
    
    protected float stepHeight { get => Mathf.Clamp(
        this.proceduralLegsWalkConstrainSCRP.stepHeightFactor
        ,0
        ,this.proceduralLegsWalkConstrainSCRP.stepMaxHeight); 
    }

    protected float stepSpeed { get => Mathf.Clamp(
        Mathf.Clamp(this.curVelocity.magnitude,1,10) * this.proceduralLegsWalkConstrainSCRP.stepVelocityFactor
        ,0
        ,this.proceduralLegsWalkConstrainSCRP.stepMaxVelocity);
    }

    protected float stepDistance 
    {
        get => Mathf.Clamp(this.curVelocity.magnitude * this.proceduralLegsWalkConstrainSCRP.stepDistanceFactor, .1f, this.proceduralLegsWalkConstrainSCRP.stepMaxDistance);
    }

    protected Vector3 oldPos;
    protected Vector3 curVelocity;
    protected readonly float timeCheckVelocity = .002f;
    protected float timerCheckVelocity = 0;



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
        this.lerpLeftLeg = 1;
        this.lerpRightLeg = 1;

        this.oldPos = this.hipTransform.position;
        this.timerCheckVelocity = 0;

        curTurn = Turn.left;



        Vector3 pos = this.leftLegRootPos + (this.hipTransform.forward * 0.2f);
        this.relativeNewLeftFootPos = pos - hipTransform.position;
        this.legsConstrainManager.SetLeftLeg_Target_Foot(pos);
        newLeftFootPos = pos;
        oldLeftFootPos = pos;

        pos = this.rightLegRootPos + (hipTransform.forward * -0.2f);
        relativeNewRightFootPos = pos - hipTransform.position;
        this.legsConstrainManager.SetRightLeg_Target_Foot(pos);
        newRightFootPos = pos;
        oldRightFootPos = pos;

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
        this.UpdateCurVelocity();
        RayCastStepCheck();
        UpdateLerpingStep();

        this.UpdateHintFoot();

        base.UpdateNode();

        this.DebugDrawLine();
    }

    private void RayCastStepCheck()
    {

        if((this.distanceLeftLeg >= this.distanceBeginStep
            || this.distanceRightLeg >= this.distanceBeginStep)
            &&( this.lerpLeftLeg >= 1 && this.lerpRightLeg >= 1))
        {
            if(this.distanceLeftLeg >= this.distanceBeginStep
                && this.distanceLeftLeg > this.distanceRightLeg)
            {
                this.lerpLeftLeg = 0;
                this.relativeNewLeftFootPos = (this.leftLegRootPos + (this.curVelocity.normalized * this.stepDistance)) - this.hipTransform.position;
                this.newLeftFootPos = this.hipTransform.position + this.relativeNewLeftFootPos;
                this.oldLeftFootPos = this.legsConstrainManager.GetLeftLeg_Target_Transform().position;
            }
            else if(this.distanceRightLeg >= this.distanceBeginStep
                && this.distanceRightLeg > this.distanceLeftLeg)
            {
                this.lerpRightLeg = 0;
                this.relativeNewRightFootPos = (this.rightLegRootPos + (this.curVelocity.normalized * this.stepDistance) - this.hipTransform.position);
                this.newRightFootPos = this.hipTransform.position + this.relativeNewRightFootPos;
                this.oldRightFootPos = this.legsConstrainManager.GetRightLeg_Target_Transform().position;
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

            this.legsConstrainManager.SetLeftLeg_Target_Foot(posL,this.leftFootRot);
            newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;

            //Debug.Log("lerpLeftLeg = " + lerpLeftLeg);

            if(lerpLeftLeg >= 1)
            {
                newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;
                oldLeftFootPos = newLeftFootPos;
                this.legsConstrainManager.SetLeftLeg_Target_Foot(this.oldLeftFootPos, this.leftFootRot);
                curTurn = Turn.right;
            }
        }
        else
        {
            if (this.distanceLeftLeg >= this.stepDistance)
            {
                this.legsConstrainManager.SetLeftLeg_Target_Foot(this.hipTransform.position + this.relativeNewLeftFootPos, this.leftFootRot);
            }
            else
            {
                this.relativeNewLeftFootPos = this.legsConstrainManager.GetLeftLeg_Target_Transform().position - this.hipTransform.position;
                this.legsConstrainManager.SetLeftLeg_Target_Foot(this.newLeftFootPos, this.leftFootRot);
            }
        }
        

        if (lerpRightLeg < 1)
        {
            Vector3 posR = Vector3.Lerp(oldRightFootPos, newRightFootPos, lerpRightLeg);
            posR.y += Mathf.Sin(lerpRightLeg * Mathf.PI) * stepHeight;
            lerpRightLeg += Time.deltaTime * stepSpeed;

            this.legsConstrainManager.SetRightLeg_Target_Foot(posR, this.rightFootRot);
            newRightFootPos = hipTransform.position + relativeNewRightFootPos;

            //Debug.Log("lerpRightLeg = " + lerpRightLeg);

            if (lerpRightLeg >= 1)
            {
                newRightFootPos = hipTransform.position + relativeNewRightFootPos;
                oldRightFootPos = newRightFootPos;
                this.legsConstrainManager.SetRightLeg_Target_Foot(this.oldRightFootPos, this.rightFootRot);
                curTurn = Turn.left;
            }
        }
        else
        {
            if (this.distanceRightLeg >= this.stepDistance)
            {
                this.legsConstrainManager.SetRightLeg_Target_Foot(this.hipTransform.position + this.relativeNewRightFootPos, this.rightFootRot);
            }
            else
            {
                this.relativeNewRightFootPos = this.legsConstrainManager.GetRightLeg_Target_Transform().position - this.hipTransform.position;
                this.legsConstrainManager.SetRightLeg_Target_Foot(this.newRightFootPos, this.rightFootRot);
            }
        }



    }
    private void UpdateCurVelocity()
    {


        this.timerCheckVelocity += Time.deltaTime;
        if(this.timerCheckVelocity >= this.timeCheckVelocity)
        {
            this.timerCheckVelocity = 0;
            this.curVelocity = (this.hipTransform.position - this.oldPos)/this.timeCheckVelocity;
            this.oldPos = this.hipTransform.position;

            //Debug.Log("CurVelocity = " + this.curVelocity.magnitude);
        }
    }

  
    private void UpdateHintFoot()
    {
        Transform targetLeftLeg = this.legsConstrainManager.GetLeftLeg_Target_Transform();

        Vector3 leftHint = targetLeftLeg.position 
            + (targetLeftLeg.forward * this.proceduralLegsWalkConstrainSCRP.leftHintOffset.z)
            + (targetLeftLeg.up * this.proceduralLegsWalkConstrainSCRP.leftHintOffset.y)
            + (targetLeftLeg.right * this.proceduralLegsWalkConstrainSCRP.leftHintOffset.x);

        Transform targetRightLeg = this.legsConstrainManager.GetRightLeg_Target_Transform();

        Vector3 rightHint = targetRightLeg.position
            + (targetRightLeg.forward * this.proceduralLegsWalkConstrainSCRP.rightHintOffset.z)
            + (targetRightLeg.up * this.proceduralLegsWalkConstrainSCRP.rightHintOffset.y)
            + (targetRightLeg.right * this.proceduralLegsWalkConstrainSCRP.rightHintOffset.x);

        this.legsConstrainManager.SetLeftLeg_Hint_FootPos(leftHint);
        this.legsConstrainManager.SetRightLeg_Hint_FootPos(rightHint);
    }

    public void DebugDrawLine()
    {
        ////Draw FootsRoot
        //Debug.DrawLine(this.hipTransform.position, this.leftLegRootPos, Color.blue);
        //Debug.DrawLine(this.hipTransform.position, this.rightLegRootPos, Color.blue);

        ////Draw FootPos
        //Debug.DrawLine(this.leftLegRootPos, this.newLeftFootPos, Color.yellow);
        //Debug.DrawLine(this.rightLegRootPos, this.newRightFootPos, Color.yellow);

        //Debug.DrawLine(this.oldLeftFootPos, this.newLeftFootPos, Color.red);
        //Debug.DrawLine(this.oldRightFootPos, this.newRightFootPos, Color.blue);
    }
}
