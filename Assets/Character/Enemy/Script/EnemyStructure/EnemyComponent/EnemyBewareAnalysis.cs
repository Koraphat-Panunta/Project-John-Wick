using UnityEngine;

public static class EnemyBewareAnalysis 
{
    public static bool IsTargetAimingTo(IWeaponAdvanceUser target,Vector3 aimedPos,float raduisAware,float limitDistance)
    {
        if(target._currentWeapon == null)
            return false;

        if ((target._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false)
            return false;

        Vector3 aimingLine = (target._shootingPos - target._currentWeapon.bulletSpawnerPos.position).normalized * limitDistance;
        Vector3 referencePos = aimedPos;
        Vector3 startPos = target._currentWeapon.bulletSpawnerPos.position;



        return IsLineOfSightCloseEnough(aimingLine,aimedPos,startPos,raduisAware);

    }
    private static bool IsLineOfSightCloseEnough(Vector3 bulletLine, Vector3 referencePos, Vector3 startPos,float raduisAware)
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
}
