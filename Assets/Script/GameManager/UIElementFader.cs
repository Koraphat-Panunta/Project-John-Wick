using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
public class UIElementFader 
{

    private SetAlphaColorUI setAlphaColorUI;

    private bool isFading = false;

    public UIElementFader()
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public async Task FadeApprear(Graphic graphic,float fadeDuration)
    {
        if (isFading) return;
        isFading = true;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            setAlphaColorUI.SetColorAlpha<Graphic>(graphic, t / fadeDuration);
            await Task.Yield();
        }

        setAlphaColorUI.SetColorAlpha<Graphic>(graphic, 1);
        isFading = false;
    }
    public async void FadeAppear(Graphic graphic,float fadeDuration,Action triggerEventFadeOut) 
    {
        await FadeApprear(graphic,fadeDuration);
        
        if(triggerEventFadeOut != null)
            triggerEventFadeOut.Invoke();
    }
    public async Task FadeDisappear(Graphic graphic, float fadeDuration)
    {
        if (isFading) return;
        isFading = true;

        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            setAlphaColorUI.SetColorAlpha<Graphic>(graphic, t / fadeDuration);
            await Task.Yield();
        }

        setAlphaColorUI.SetColorAlpha<Graphic>(graphic, 1);
        isFading = false;
    }
    public async void FadeDisappear(Graphic graphic,float fadeDuration,Action triggerEventFadeIn)
    {
        await FadeDisappear(graphic, fadeDuration);
        
        if(triggerEventFadeIn != null)
            triggerEventFadeIn.Invoke();
    }
    public void SetAlphaSceneFade(Graphic graphic,float t) => this.setAlphaColorUI.SetColorAlpha<Graphic>(graphic, t);
}
