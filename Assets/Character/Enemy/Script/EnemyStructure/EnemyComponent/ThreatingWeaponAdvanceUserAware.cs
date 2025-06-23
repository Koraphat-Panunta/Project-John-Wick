using System;
using UnityEngine;

public class ThreatingWeaponAdvanceUserAware 
{

    private Transform transform;

    private Vector3 position => transform.position;
    private float raduisAware = 3;
    private LayerMask targetAware;

    public ThreatingWeaponAdvanceUserAware(FindingTarget findingTarget,IHeardingAble heardingAble,Transform transform,LayerMask targetAware) 
    {
        this.transform = transform;

        heardingAble.NotifyGotHearing += HeardingTarget;
        findingTarget.OnSpottingTarget += SpottingTarget;
    }
    public ThreatingWeaponAdvanceUserAware(FindingTarget findingTarget, IHeardingAble heardingAble, Transform transform,float raduisAware,LayerMask targetAware) 
        : this (findingTarget,heardingAble,transform,targetAware)
    {
        this.raduisAware = raduisAware;
    }

    public Action<GameObject> OnWeaponPointingTowardAware;
    public Action<INoiseMakingAble> OnWeaponShootingTowardAware;
    private void SpottingTarget(GameObject target)
    {
        if(target.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser targetWeaponUser) == false)
            return;

        if(target.layer != targetAware)
            return;

        if((targetWeaponUser._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
        {

            Vector3 pointingLine = targetWeaponUser._pointingPos - targetWeaponUser._userWeapon.transform.position;
            Vector3 startPointerPos = targetWeaponUser._userWeapon.transform.position;

            if (IsLineOfSightCloseEnough(pointingLine, this.position, startPointerPos))
            {
                OnWeaponPointingTowardAware.Invoke(target);
            }
        }
        

    }
   
    private bool IsLineOfSightCloseEnough(Vector3 bulletLine, Vector3 referencePos, Vector3 startPos)
    {
        

        Vector3 startPosToRefPoint = referencePos - startPos;
        float t = Vector3.Dot(startPosToRefPoint, bulletLine) / Vector3.Dot(bulletLine, bulletLine);
        if (t > 1 || t < 0)
            return false;

        Vector3 closestPoint = startPos + t * bulletLine;
        Debug.DrawLine(closestPoint, referencePos, Color.yellow);
        Debug.DrawLine(startPos, startPos + bulletLine, Color.red);

        if (Vector3.Distance(closestPoint, referencePos) < raduisAware)
            return true;

        return false;
    }
    private void HeardingTarget(INoiseMakingAble noiseMakingAble)
    {
        if(noiseMakingAble is Bullet bullet == false)
            return;

        IWeaponAdvanceUser weaponAdvanceUser = bullet.weapon.userWeapon;

        if (weaponAdvanceUser._userWeapon.gameObject.layer != targetAware)
            return;

        Vector3 shootingLine = weaponAdvanceUser._shootingPos - weaponAdvanceUser._userWeapon.transform.position;
        Vector3 startPointerPos = weaponAdvanceUser._userWeapon.transform.position;

        if (IsLineOfSightCloseEnough(shootingLine, this.position, startPointerPos))
        {
            OnWeaponShootingTowardAware.Invoke(noiseMakingAble);
        }

    }
    public void SetRaduisAware(float r)=>this.raduisAware = r;
}
