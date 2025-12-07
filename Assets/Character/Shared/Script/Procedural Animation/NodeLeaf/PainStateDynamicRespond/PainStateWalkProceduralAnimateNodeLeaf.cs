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

    protected TwoBoneIKConstraint leftLeg => proceduralAnimateNodeManager.leftLeg;
    public Vector3 oldLeftFootPos;
    public Vector3 newLeftFootPos;
    public Vector3 relativeNewLeftFootPos;
    protected float lerpLeftLeg;

    protected TwoBoneIKConstraint rightLeg => proceduralAnimateNodeManager.rightLeg;
    public Vector3 oldRightFootPos;
    public Vector3 newRightFootPos;
    public Vector3 relativeNewRightFootPos;
    protected float lerpRightLeg;

    protected Transform hipTransform => proceduralAnimateNodeManager.centre;
    protected float hipLegsSpace => proceduralAnimateNodeManager.hipLegSpace;

    protected float stepDistance => proceduralAnimateNodeManager.StepDistacne * Mathf.Clamp(curVelocity.magnitude, 0.1f, 1.2f);
    protected float stepHeight => proceduralAnimateNodeManager.StepHeight;
    protected float stepSpeed
    {
        get
        {
            float stepSpeed = proceduralAnimateNodeManager.StepVelocity * Mathf.Clamp(curVelocity.magnitude, .5f, 1f);
            proceduralAnimateNodeManager.enemyProceduralAnimateNodeManagerDebug = "stepSpeed = " + stepSpeed + "\n";
            return stepSpeed;
        }
    }

    private float maxOffset = 1.5f;
    protected Vector3 footplacementOffsetDistance => Vector3.ClampMagnitude(proceduralAnimateNodeManager.FootstepPlacementOffsetDistance * curVelocity, maxOffset);

    protected Vector3 curVelocity => enemy._movementCompoent.curMoveVelocity_World;

    private float transitionVelocity = 3;

    protected Enemy enemy => proceduralAnimateNodeManager.enemy;
    protected EnemyConstrainAnimationNodeManager proceduralAnimateNodeManager;

    public enum Phase
    {
        TransitionIn,
        Stay,
        TransitionOut
    }
    public Phase curPhase;

    private float transitionInElapesTime;
    private const float transitionInDuration = 1;
    public PainStateWalkProceduralAnimateNodeLeaf(EnemyConstrainAnimationNodeManager enemyProceduralAnimateNodeManager, Func<bool> preCondition) : base(preCondition)
    {
        this.proceduralAnimateNodeManager = enemyProceduralAnimateNodeManager;
        stepAbleLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Default");
    }

    public override void Enter()
    {
        curTurn = Turn.left;

        Ray rayLeftLeg = new Ray(hipTransform.position - (hipTransform.right * hipLegsSpace), Vector3.down);

        if (Physics.Raycast(rayLeftLeg, out RaycastHit hitInfoLeft, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoLeft.point + (hipTransform.forward * 0.2f);
            relativeNewLeftFootPos = pos - hipTransform.position;
            leftLeg.data.target.position = pos;
            newLeftFootPos = pos;
            oldLeftFootPos = pos;
        }
        Ray rayRightLeg = new Ray(hipTransform.position + (hipTransform.right * hipLegsSpace), Vector3.down);
        if (Physics.Raycast(rayRightLeg, out RaycastHit hitInfoRight, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoRight.point + (hipTransform.forward * -0.2f);
            relativeNewRightFootPos = pos - hipTransform.position ;
            rightLeg.data.target.position = pos;
            newRightFootPos = pos;
            oldRightFootPos = pos;
        }

        transitionInElapesTime = 0;
        curPhase = Phase.TransitionIn;
        base.Enter();
    }

    public override void Exit()
    {
        curPhase = Phase.TransitionOut;
        proceduralAnimateNodeManager.StartCoroutine(TransitionOut());
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


        if (curPhase == Phase.TransitionIn)
        {
            weight = transitionInElapesTime / transitionInDuration;
            transitionInElapesTime += Time.deltaTime * this.transitionVelocity;

            leftLeg.weight = weight;
            rightLeg.weight = weight;

            if (transitionInElapesTime >= transitionInDuration)
                curPhase = Phase.Stay;
        }
        else if (curPhase == Phase.Stay)
        {

        }

        base.UpdateNode();
    }

    private void RayCastStepCheck()
    {
        Ray rayLeftLeg = new Ray(hipTransform.position + (hipTransform.right * -hipLegsSpace) - hipTransform.forward * 0.2f, Vector3.down);
        Ray rayRightLeg = new Ray(hipTransform.position + (hipTransform.right * hipLegsSpace) - hipTransform.forward * 0.2f, Vector3.down);

        if (curTurn == Turn.left && Physics.Raycast(rayLeftLeg, out RaycastHit hitInfoLeft, 10, stepAbleLayer))
        {
            if (Vector3.Distance(newLeftFootPos, hitInfoLeft.point) > stepDistance && lerpRightLeg >= 1 && lerpLeftLeg >= 1 )
            {
                lerpLeftLeg = 0;
                relativeNewLeftFootPos = (hitInfoLeft.point + footplacementOffsetDistance) - hipTransform.position;
                newLeftFootPos = hipTransform.position + relativeNewLeftFootPos;
            }
        }
        else if (curTurn == Turn.right && Physics.Raycast(rayRightLeg, out RaycastHit hitInfoRight, 10, stepAbleLayer))
        {
            if (Vector3.Distance(newRightFootPos, hitInfoRight.point) > stepDistance && lerpLeftLeg >= 1 && lerpRightLeg >= 1)
            {
                lerpRightLeg = 0;
                relativeNewRightFootPos = (hitInfoRight.point + footplacementOffsetDistance) - hipTransform.position;
                newRightFootPos = hipTransform.position + relativeNewRightFootPos;

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
            oldRightFootPos = newRightFootPos;
            rightLeg.data.target.position = oldRightFootPos;
        }
       
    }
    private IEnumerator TransitionOut()
    {
        while (weight > 0)
        {
            if (curPhase != Phase.TransitionOut)
                break;
            weight -= Time.deltaTime * transitionVelocity;
            leftLeg.weight = weight;
            rightLeg.weight = weight;
            yield return null;
        }
    }
}
