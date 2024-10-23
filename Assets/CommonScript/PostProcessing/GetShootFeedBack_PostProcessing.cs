using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GetShootFeedBack_PostProcessing 
{
    private DynamicPostProcessing postProcessing;
    private Coroutine coroutine;
    private Vignette vignette;
    private float weight = 1.0f;
    private float recoveryRate = 2.6f;
    public GetShootFeedBack_PostProcessing(DynamicPostProcessing dynamicPostProcessing)
    {
        postProcessing = dynamicPostProcessing;
        vignette = postProcessing.vignette;
    }
   public void TriggerFeedBack()
    {
        if(coroutine != null)
        {
            postProcessing.StopCoroutine(coroutine);
        }
        coroutine = postProcessing.StartCoroutine(PerformedFeedBack());
    }
    IEnumerator PerformedFeedBack()
    {
        vignette.intensity.value = weight;
        while (vignette.intensity.value > 0)
        {
            vignette.intensity.value -= recoveryRate*Time.deltaTime;
            yield return null;
        }
    }
}
