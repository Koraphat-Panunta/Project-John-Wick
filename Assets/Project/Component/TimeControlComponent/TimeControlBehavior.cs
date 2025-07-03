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
    public static async void TriggerTimeStop(float durationStop, float durationReset)
    {
        float originalFixdeltatime = Time.fixedDeltaTime;

        Time.timeScale = 0;
        //Time.fixedDeltaTime *= Mathf.Clamp(Time.timeScale,0.01f,1);

        await Task.Delay((int)(1000*durationStop));

        float t = 0f;
        while (t < durationReset)
        {
            t += Time.unscaledDeltaTime; // use unscaledDeltaTime because timeScale is changing
            float normalized = Mathf.Clamp01(t / durationReset);
            Time.timeScale = normalized; // smoothly increase timescale from 0 to 1

            await Task.Yield(); // wait for next frame
        }

        Time.timeScale = 1;
        Time.fixedDeltaTime = originalFixdeltatime;
    }
}
