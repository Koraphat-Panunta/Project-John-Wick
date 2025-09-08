using System.Collections;
using UnityEngine;

public class TimeControlBehavior 
{
    private MonoBehaviour coroutineCaller;

    public TimeControlBehavior(MonoBehaviour coroutineCaller)
    {
        this.coroutineCaller = coroutineCaller;
    }

    // --- Simple time stop ---
    public void TriggerTimeStop(float duration)
    {
        coroutineCaller.StartCoroutine(TimeStopRoutine(duration));
    }

    private IEnumerator TimeStopRoutine(float duration)
    {
        float originalFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration); // unaffected by timeScale

        Time.timeScale = 1f;
    }

    // --- Time stop with gradual reset ---
    public void TriggerTimeStop(float durationStop, float durationReset, AnimationCurve animationCurve = null)
    {
        coroutineCaller.StartCoroutine(TimeStopResetRoutine(durationStop, durationReset, animationCurve));
    }

    private IEnumerator TimeStopResetRoutine(float durationStop, float durationReset, AnimationCurve animationCurve)
    {
        float originalFixedDeltaTime = Time.fixedDeltaTime;

        // Stop time
        Time.timeScale = 0f;

        // Hold time stopped
        yield return new WaitForSecondsRealtime(durationStop);

        float elapsed = 0f;
        while (elapsed < durationReset)
        {
            elapsed += Time.unscaledDeltaTime;
            float normalized = Mathf.Clamp01(elapsed / durationReset);

            float timeScale = (animationCurve != null)
                ? animationCurve.Evaluate(normalized)
                : normalized;

            Time.timeScale = timeScale;

            yield return null; // wait 1 frame
        }

        // Restore full time
        Time.timeScale = 1f;
    }
}
