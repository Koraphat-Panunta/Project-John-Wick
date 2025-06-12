
using UnityEngine;
using UnityEngine.UI;

public class SetAlphaColorUI
{
    public void SetColorAlpha<T>(T ui,float n) where T : Graphic
    {
        Color color = ui.color;
        color.a = Mathf.Clamp01(n);
        ui.color = color;
    }
}
