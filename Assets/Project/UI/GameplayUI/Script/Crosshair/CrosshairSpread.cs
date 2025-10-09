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
            if(_crosshairController.player._currentWeapon != null)
                return _crosshairController.player._currentWeapon.min_Precision;
            else
                return MIN_SPREAD;
        } 
    }

    public float maxWeaponPercision
    {
        get
        {
            if (_crosshairController.player._currentWeapon != null)
                return _crosshairController.player._currentWeapon.max_Precision;
            else
                return MAX_SPREAD;
        }
    }


    public float spread_rate { get; protected set; }
    Vector2 crosshairKickUpRate;
    public float sperad_rateDestination { get; protected set; }
    float spread_rateOppress 
    { get 
        {
            if(_crosshairController.player._currentWeapon != null)
                return _crosshairController.player._currentWeapon.Accuracy;
            else
                return SPREAD_RATEOPPRESS_DEFAULT;
        } 
    }
    

    private const float SPREAD_RATEOPPRESS_DEFAULT = 40;

   
    public CrosshairSpread(CrosshairController crosshairController)
    {
        this._crosshairController = crosshairController;
        this.spread_rate = 0;
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
    public void CrosshairKickUp(float recoilKickUp)
    {
        crosshairKickUpRate = new Vector2(
            crosshairKickUpRate.x + (_crosshairController.crosshairSideKickMultiple * recoilKickUp * (Random.value > .5f?1:-1))
            , Mathf.Clamp(crosshairKickUpRate.y + (recoilKickUp*_crosshairController.crosshairUpperKickMultiple), 0, 65));

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
        crosshairKickUpRate = Vector2.MoveTowards(crosshairKickUpRate, Vector2.zero, spread_rateOppress*1.2f * Time.deltaTime);

        _crosshairController.Crosshair_lineUp.anchoredPosition = new Vector2(0, minWeaponPercision + spread_rate);
        _crosshairController.Crosshair_lineDown.anchoredPosition = new Vector2(0, -minWeaponPercision - spread_rate);
        _crosshairController.Crosshair_lineLeft.anchoredPosition = new Vector2(-minWeaponPercision - spread_rate, 0);
        _crosshairController.Crosshair_lineRight.anchoredPosition = new Vector2(minWeaponPercision + spread_rate, 0);
        _crosshairController.Crosshair_CenterPosition.anchoredPosition = new Vector2(crosshairKickUpRate.x, crosshairKickUpRate.y);
      

    }

    private float isRecoveryFocus = 0f;
    private float recoverFocusRate = 6;
    private float focusSpreadMaxRate = 0.25f;
    public void TriggerFocusSpanRate()
    {
        this.focusSpreadRate = this.focusSpreadMaxRate;
        isRecoveryFocus = 1f;
    }
}
