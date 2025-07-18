using UnityEngine;
using System.Threading.Tasks;
public class TimeControlBehavior 
{
    public static async void TriggerTimeStop(float duration)
    {
        Time.timeScale = 0;
        await Task.Delay((int)(1000*duration));
        Time.timeScale = 1;
    }
    public static async void TriggerTimeStop(float durationStop, float durationReset, AnimationCurve animationCurve = null)
    {
        float originalFixedDeltaTime = Time.fixedDeltaTime;

        // Freeze time
        Time.timeScale = 0f;

        // Wait in real time while time is stopped
        await Task.Delay((int)(durationStop * 1000));

        float elapsed = 0f;

        while (elapsed < durationReset)
        {
            elapsed += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / durationReset);

            // Evaluate with curve if provided, else use linear
            float timeScale = animationCurve != null
                ? animationCurve.Evaluate(normalizedTime)
                : normalizedTime;

            Time.timeScale = timeScale;

            await Task.Yield();
        }

        // Restore time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
