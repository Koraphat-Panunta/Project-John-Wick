using UnityEngine;

public static class EnemyOffendCommandWeaponBased 
{
    public static void Rest(Enemy enemy, EnemyCommandAPI enemyCommandAPI)
    {
        switch (enemy._mainHandSocket._currentGrabbedObject)
        {
            case MeleeWeapon meleeWeapon:
                {
                    enemyCommandAPI.LowReady();
                }
                break;
            case RangeWeapon rangeWeapon:
                {
                    enemyCommandAPI.LowReady();
                }
                break;
        }
    }
    public static void Hold(Enemy enemy,EnemyCommandAPI enemyCommandAPI,Vector3 targetKnowPos)
    {
        switch (enemy._mainHandSocket._currentGrabbedObject)
        {
            case MeleeWeapon meleeWeapon:
                {
                    enemyCommandAPI.AimDownSight(targetKnowPos);
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
            case RangeWeapon rangeWeapon:
                {
                    enemyCommandAPI.AimDownSight(targetKnowPos);
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
        }
    }
    public static void Hold(Enemy enemy, EnemyCommandAPI enemyCommandAPI)
    {
        switch (enemy._mainHandSocket._currentGrabbedObject)
        {
            case MeleeWeapon meleeWeapon:
                {
                    enemyCommandAPI.AimDownSight();
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
            case RangeWeapon rangeWeapon:
                {
                    enemyCommandAPI.AimDownSight();
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
        }
    }
    public static void Engage(Enemy enemy,EnemyCommandAPI enemyCommandAPI,Vector3 targetKnow)
    {
        switch (enemy._mainHandSocket._currentGrabbedObject)
        {
            case MeleeWeapon meleeWeapon:
                {
                    enemyCommandAPI.AimDownSight(targetKnow);
                    enemyCommandAPI.NormalFiringPattern.Performing();
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
            case RangeWeapon rangeWeapon:
                {
                    enemyCommandAPI.AimDownSight(targetKnow);
                    enemyCommandAPI.AutoDetectSoftCover();
                }
                break;
        }
    }
    public static void Ambush(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Vector3 targetKnow)
    {
        switch (enemy._mainHandSocket._currentGrabbedObject)
        {
            case MeleeWeapon meleeWeapon:
                {
                    enemyCommandAPI.SprintToMeleeAttack(targetKnow, enemy._attackRange);
                }
                break;
            case RangeWeapon rangeWeapon:
                {
                   
                    enemyCommandAPI.AutoDetectSoftCover();
                    enemyCommandAPI.AimDownSight(targetKnow);
                    enemyCommandAPI.NormalFiringPattern.Performing();
                }
                break;
        }
    }
}
