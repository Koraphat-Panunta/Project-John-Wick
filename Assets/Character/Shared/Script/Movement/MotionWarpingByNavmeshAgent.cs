using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MotionWarpingByNavmeshAgent : IMovementMotionWarping
{
    public bool isWarping { get; set; }
    private bool isWarpingComplete;
    public Coroutine motionWarping { get; set; }
    public MovementCompoent movementComponent { get; set; }

    public NavMeshAgent agent { get; set; }
    public MotionWarpingByNavmeshAgent(MovementCompoent movementComponent, NavMeshAgent agent)
    {
        this.movementComponent = movementComponent;
        this.agent = agent;
    }
    public IEnumerator MotionWarpingCurve(
        Vector3 start,
        Vector3 cT1,
        Vector3 cT2,
        Vector3 exit,
        float duration,
        AnimationCurve animationCurve)
    {
        float elapsedTime = 0f; // Track the elapsed time of the motion

        isWarpingComplete = false;
        isWarping = true;

        while (elapsedTime < duration)
        {
            // Calculate normalized time (0 to 1)
            float t = elapsedTime / duration;

            // Apply animation curve to smooth the motion
            float smoothedT = animationCurve.Evaluate(t);

            // Compute position along the cubic B�zier curve using the smoothed parameter
            Vector3 position = CalculateBezierPoint(smoothedT, start, cT1, cT2, exit);

            // Move the selfNPCTarget to the computed position
            Vector3 delta = position - agent.transform.position;
            agent.Move(delta);

            // Increment time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the selfNPCTarget reaches the final exit position
        Vector3 finalPosition = CalculateBezierPoint(1f, start, cT1, cT2, exit);
        Vector3 finalDelta = finalPosition - agent.transform.position;
        agent.Move(finalDelta);

        motionWarping = null;
        isWarping = false;
        isWarpingComplete |= true;
    }

    // Helper method to calculate a point on a cubic B�zier curve
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1 - t)^3 * P0
        point += 3 * uu * t * p1; // 3 * (1 - t)^2 * t * P1
        point += 3 * u * tt * p2; // 3 * (1 - t) * t^2 * P2
        point += ttt * p3;        // t^3 * P3

        return point;
    }

    public void StartMotionWarpingCurve(Vector3 start,
        Vector3 cT1,
        Vector3 cT2,
        Vector3 exit,
        float duration,
        AnimationCurve animationCurve)
    {
        motionWarping = this.movementComponent.userMovement.StartCoroutine(MotionWarpingCurve(start, cT1, cT2, exit, duration, animationCurve));
    }
    public bool IsWarpingComplete() => isWarpingComplete;

    public IEnumerator MotionWarpingLinear(Vector3 start, Vector3 end, float duration, AnimationCurve animationCurve)
    {
        float elapsedTime = 0f; // Track the elapsed time of the motion

        isWarpingComplete = false;
        isWarping = true;

        // Set NavMeshAgent settings to allow manual movement
        agent.isStopped = true;  // Stop any ongoing pathfinding

        while (elapsedTime < duration)
        {
            // Calculate normalized time (0 to 1)
            float t = elapsedTime / duration;

            // Apply animation curve for smooth interpolation
            float smoothedT = animationCurve.Evaluate(t);

            // Compute the interpolated position
            Vector3 position = Vector3.Lerp(start, end, smoothedT);

            // Move the NavMeshAgent
            Vector3 delta = position - agent.transform.position;
            agent.Move(delta);

            // Increment time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the agent reaches the final position
        Vector3 finalDelta = end - agent.transform.position;
        agent.Move(finalDelta);

        // Restore NavMeshAgent behavior
        agent.isStopped = false;

        motionWarping = null;
        isWarping = false;
        isWarpingComplete = true;
    }

    public void StartMotionWarpingLinear(Vector3 start, Vector3 end, float duration, AnimationCurve animationCurve)
    {
        if (motionWarping != null)
        {
            movementComponent.userMovement.StopCoroutine(motionWarping);
        }

        motionWarping = movementComponent.userMovement.StartCoroutine(MotionWarpingLinear(start, end, duration, animationCurve));
    }

}
