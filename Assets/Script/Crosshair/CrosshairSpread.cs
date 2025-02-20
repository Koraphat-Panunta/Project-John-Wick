using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpread : ICrosshairAction
{
    private CrosshairController _crosshairController;
    public const float MAX_SPREAD = 1080;
    public const float MIN_SPREAD = 0;

    public bool isAiming;
   
    private float focusSpreadRate;

    public float minWeaponPercision 
    { get 
        {
            if(_crosshairController.player.currentWeapon != null)
                return _crosshairController.player.currentWeapon.min_Precision;
            else
                return MIN_SPREAD;
        } 
    }

    public float maxWeaponPercision
    {
        get
        {
            if (_crosshairController.player.currentWeapon != null)
                return _crosshairController.player.currentWeapon.max_Precision;
            else
                return MAX_SPREAD;
        }
    }


    float spread_rate = 0;
    float sperad_rateDestination;
    float spread_rateOppress 
    { get 
        {
            if(_crosshairController.player.currentWeapon != null)
                return _crosshairController.player.currentWeapon.Accuracy;
            else
                return SPREAD_RATEOPPRESS_DEFAULT;
        } 
    }
    

    private const float SPREAD_RATEOPPRESS_DEFAULT = 40;

   
    public CrosshairSpread(CrosshairController crosshairController)
    {
        this._crosshairController = crosshairController;
       
    }
    public void Performed(Weapon weapon)
    {
       float recoilKick = weapon.RecoilKickBack - weapon.RecoilController;
        spread_rate = Mathf.Clamp(spread_rate + recoilKick, minWeaponPercision, maxWeaponPercision);
    }
    public void Performed(float recoilKick)
    {
        spread_rate = Mathf.Clamp(spread_rate + recoilKick, minWeaponPercision, maxWeaponPercision);
    }


    public void CrosshairSpreadUpdate()
    {
        if(isAiming == false) 
            sperad_rateDestination = maxWeaponPercision;

        if (isAiming)
        {
            sperad_rateDestination = minWeaponPercision + (Mathf.Abs(minWeaponPercision - maxWeaponPercision) * focusSpreadRate);

            if (spread_rate <= sperad_rateDestination)
            {
                if (isRecoveryFocus >= 0)
                    isRecoveryFocus -= Time.deltaTime;

                if (isRecoveryFocus <= 0)
                    this.focusSpreadRate = Mathf.Clamp(this.focusSpreadRate - recoverFocusRate * Time.deltaTime, 0, focusSpreadMaxRate);
            }
                
        }

        spread_rate = Mathf.MoveTowards(spread_rate,sperad_rateDestination,spread_rateOppress * Time.deltaTime);

        _crosshairController.Crosshair_lineUp.anchoredPosition = new Vector2(0, minWeaponPercision + spread_rate);
        _crosshairController.Crosshair_lineDown.anchoredPosition = new Vector2(0, -minWeaponPercision - spread_rate);
        _crosshairController.Crosshair_lineLeft.anchoredPosition = new Vector2(-minWeaponPercision - spread_rate, 0);
        _crosshairController.Crosshair_lineRight.anchoredPosition = new Vector2(minWeaponPercision + spread_rate, 0);

       

        Debug.Log("Spread_rate = " + spread_rate);

    }

    private float isRecoveryFocus = 0f;
    private float recoverFocusRate = 6;
    private float focusSpreadMaxRate = 0.35f;
    public void TriggerFocusSpanRate()
    {
        this.focusSpreadRate = this.focusSpreadMaxRate;
        isRecoveryFocus = 0.5f;
    }
}
