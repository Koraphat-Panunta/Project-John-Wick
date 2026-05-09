using UnityEngine;

public static class RangeWeaponBehavior 
{
    public static void ShootByPassRateOfFire(RangeWeapon rangeWeapon)
    {
        if(rangeWeapon.chamber.isReadyShoot == false)
        {
            BulletCapacity bulletCapacity = rangeWeapon.TryGetBulletCapacity(out BulletCapacity Capacity) ? Capacity : null;

            rangeWeapon.chamber.UnLoad();
            if (bulletCapacity != null
                && bulletCapacity.GetBulletOut(out Bullet bullet)
                )
            {
                //Debug.Log("Auto load Chamber "+this.RangeWeapon);
                rangeWeapon.chamber.Load(bullet);
                //Debug.Log(this.RangeWeapon + "isReadyShoot == "+this.chamber.isReadyShoot);
            }
        }

        rangeWeapon.PullTrigger();


    }
}
