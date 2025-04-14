using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyPainStateProceduralAnimateNodeLeaf : AnimationConstrainNodeLeaf
{
    LayerMask stepAbleLayer;

    private float weight;

    protected TwoBoneIKConstraint leftLeg => proceduralAnimateNodeManager.leftLeg;
    protected Vector3 oldLeftFootPos;
    protected Vector3 newLeftFootPos;
    protected float lerpLeftLeg;

    protected TwoBoneIKConstraint rightLeg => proceduralAnimateNodeManager.rightLeg;
    protected Vector3 oldRightFootPos;
    protected Vector3 newRightFootPos;
    protected float lerpRightLeg;

    protected Transform hipTransform => proceduralAnimateNodeManager.centre;
    protected float hipLegsSpace => proceduralAnimateNodeManager.hipLegSpace;

    protected float stepDistance => proceduralAnimateNodeManager.StepDistacne * Mathf.Clamp(curVelocity.magnitude, 0.1f, 1.2f);
    protected float stepHeight => proceduralAnimateNodeManager.StepHeight;
    protected float stepSpeed
    {
        get
        {
            float stepSpeed = proceduralAnimateNodeManager.StepVelocity * Mathf.Clamp(curVelocity.magnitude, 0.1f, 1f);
            proceduralAnimateNodeManager.enemyProceduralAnimateNodeManagerDebug = "stepSpeed = " + stepSpeed + "\n";
            return stepSpeed;
        }
    }

    private float maxOffset = 1.5f;
    protected Vector3 footplacementOffsetDistance => Vector3.ClampMagnitude(proceduralAnimateNodeManager.FootstepPlacementOffsetDistance * curVelocity, maxOffset);

    protected Vector3 curVelocity => enemy.enemyMovement.curMoveVelocity_World;

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
    public EnemyPainStateProceduralAnimateNodeLeaf(EnemyConstrainAnimationNodeManager enemyProceduralAnimateNodeManager, Func<bool> preCondition) : base(preCondition)
    {
        this.proceduralAnimateNodeManager = enemyProceduralAnimateNodeManager;
        stepAbleLayer = LayerMask.GetMask("Ground");
    }

    public override void Enter()
    {
        Ray rayLeftLeg = new Ray(hipTransform.position - (hipTransform.right * hipLegsSpace), Vector3.down);
        if (Physics.Raycast(rayLeftLeg, out RaycastHit hitInfoLeft, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoLeft.point + hipTransform.forward * 0.2f;
            leftLeg.data.target.position = pos;
            newLeftFootPos = pos;
            oldLeftFootPos = pos;
        }

        Ray rayRightLeg = new Ray(hipTransform.position + (hipTransform.right * hipLegsSpace), Vector3.down);
        if (Physics.Raycast(rayRightLeg, out RaycastHit hitInfoRight, 10, stepAbleLayer))
        {
            Vector3 pos = hitInfoRight.point + hipTransform.forward * -0.2f;
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
        if (Physics.Raycast(rayLeftLeg, out RaycastHit hitInfoLeft, 10, stepAbleLayer))
        {
            if (Vector3.Distance(newLeftFootPos, hitInfoLeft.point) > stepDistance && lerpRightLeg >= 1)
            {
                if (lerpLeftLeg < 1)
                {
                    oldLeftFootPos = newLeftFootPos;
                }
                lerpLeftLeg = 0;
                newLeftFootPos = hitInfoLeft.point + footplacementOffsetDistance;
            }
        }

        Ray rayRightLeg = new Ray(hipTransform.position + (hipTransform.right * hipLegsSpace) - hipTransform.forward * 0.2f, Vector3.down);
        if (Physics.Raycast(rayRightLeg, out RaycastHit hitInfoRight, 10, stepAbleLayer))
        {
            if (Vector3.Distance(newRightFootPos, hitInfoRight.point) > stepDistance && lerpLeftLeg >= 1)
            {
                if (lerpRightLeg < 1)
                {
                    oldRightFootPos = newRightFootPos;
                }
                lerpRightLeg = 0;

                newRightFootPos = hitInfoRight.point + footplacementOffsetDistance;

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
