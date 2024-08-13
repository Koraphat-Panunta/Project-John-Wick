using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpread : ICrosshairAction
{
    private CrosshairController _crosshairController;
    float spread_rate = 0;
    private Coroutine _coroutine;
    bool isRecovery = false;
    public CrosshairSpread(CrosshairController crosshairController)
    {
        this._crosshairController = crosshairController;
       
    }
    public void Performed(Weapon weapon)
    {
        spread_rate += weapon.RecoilKickBack;
        spread_rate = Mathf.Clamp(spread_rate, 0, weapon.max_Precision - weapon.min_Precision);
        if (isRecovery == false)
        {
            _crosshairController.StartCoroutine(ShootSpread(weapon));
        }
    }

    public void Performed(PlayerStateManager playerStateManager)
    {
        
    }
    IEnumerator ShootSpread(Weapon weapon)
    {
        isRecovery = true;
        while (spread_rate > 0)
        {
            _crosshairController.Crosshair_lineUp.anchoredPosition = new Vector2(0, weapon.min_Precision + spread_rate);
            _crosshairController.Crosshair_lineDown.anchoredPosition = new Vector2(0, -weapon.min_Precision - spread_rate);
            _crosshairController.Crosshair_lineLeft.anchoredPosition = new Vector2(-weapon.min_Precision - spread_rate, 0);
            _crosshairController.Crosshair_lineRight.anchoredPosition = new Vector2(weapon.min_Precision + spread_rate, 0);
            spread_rate -= weapon.Accuracy*Time.deltaTime;
            spread_rate = Mathf.Clamp(spread_rate, 0, weapon.max_Precision - weapon.min_Precision);
            yield return null;
        }
        isRecovery = false;
    }
}
