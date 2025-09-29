using System.Collections;
using UnityEngine;

public class TimeControlBehavior 
{
    private MonoBehaviour coroutineCaller;
    private const float leastTimeScale = 0.2f;
    // Keep track of the currently running coroutine
    private Coroutine runningCoroutine;
    private const float fixedDeltaTime = 0.02f;

    public TimeControlBehavior(MonoBehaviour coroutineCaller)
    {
        this.coroutineCaller = coroutineCaller;
    }

    // --- Simple time stop ---
    public void TriggerCancelTimeControlBehavior()
    {
        // Stop previous if still running
        if (runningCoroutine != null)
        {
            coroutineCaller.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        Time.timeScale = 1f;

    }

  

    // --- Time stop with gradual reset ---
    public void TriggerTimeStop(float durationStop, float durationReset, AnimationCurve animationCurve = null)
    {
        // Stop previous if still running
        if (runningCoroutine != null)
        {
            coroutineCaller.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        runningCoroutine = coroutineCaller.StartCoroutine(TimeStopResetRoutine(durationStop, durationReset, animationCurve));
    }

    private IEnumerator TimeStopResetRoutine(float durationStop, float durationReset, AnimationCurve animationCurve)
    {

        // Stop time
        Time.timeScale = leastTimeScale;

        // Hold time stopped
        if(durationStop > 0)
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
            //Time.fixedDeltaTime = Time.timeScale * .02f;
            yield return null; // wait 1 frame
        }

        // Restore full time
        Time.timeScale = 1f;

        runningCoroutine = null; // clear when done
    }

    public void TriggerTimeStop(float timeScaleSlowMotion,float durationSlow, float durationReset, AnimationCurve animationCurve = null)
    {
        // Stop previous if still running
        if (runningCoroutine != null)
        {
            coroutineCaller.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        runningCoroutine = coroutineCaller.StartCoroutine(TimeStopResetRoutine(timeScaleSlowMotion,durationSlow, durationReset, animationCurve));
    }
    private IEnumerator TimeStopResetRoutine(float timeScaleSlowMotion,float durationStop, float durationReset, AnimationCurve animationCurve)
    {

        // Stop time
        Time.timeScale = Mathf.Clamp(timeScaleSlowMotion,leastTimeScale,1);

        // Hold time stopped
        if (durationStop > 0)
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

        runningCoroutine = null; // clear when done
    }

    public void TriggerBulletTime(float slowTimeScale) 
    {
        Time.timeScale = Mathf.Clamp(slowTimeScale, leastTimeScale, 1);

    }

    public void TriggerBulletTime(float duration, AnimationCurve animationCurve)
    {
        // Stop previous if still running
        if (runningCoroutine != null)
        {
            coroutineCaller.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        runningCoroutine = coroutineCaller.StartCoroutine(TriggerBulletTimeCoroutine(duration, animationCurve));
    }
    private IEnumerator TriggerBulletTimeCoroutine(float duration, AnimationCurve animationCurve)
    {

       

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float normalized = Mathf.Clamp01(elapsed / duration);

            float timeScale = (animationCurve != null)
                ? animationCurve.Evaluate(normalized)
                : normalized;

            Time.timeScale = timeScale;

            yield return null; // wait 1 frame
        }

        // Restore full time
        Time.timeScale = 1f;

        runningCoroutine = null; // clear when done
    }

}
