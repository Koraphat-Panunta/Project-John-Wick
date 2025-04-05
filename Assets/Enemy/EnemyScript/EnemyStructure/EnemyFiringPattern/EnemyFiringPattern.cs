using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFiringPattern 
{
    public bool isShootAble = true;
    public abstract bool isReadyToShoot { get; set; }
    protected Enemy enemy;
    protected EnemyCommandAPI enemyController;
    protected Weapon curWeapon => enemy._currentWeapon;

    public EnemyFiringPattern(EnemyCommandAPI enemyController)
    {
        this.enemy = enemyController._enemy;
        this.enemyController = enemyController;

    }

    public abstract void Performing();

    protected virtual void Shoot()
    {
        if (isShootAble == false)
            return;

        if (DetectObstacle(1))
            return;
        enemyController.PullTrigger();

    }
    private bool DetectObstacle(float distance)
    {
        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int GroundHitMask = LayerMask.GetMask("Ground");

        LayerMask layerMask = DefaultMask + BodyPartMask + GroundHitMask;
        Ray ray = new Ray(enemy.rayCastPos.position, enemy.rayCastPos.forward);
        Debug.DrawLine(enemy.rayCastPos.position, enemy.rayCastPos.forward * distance);
        if (Physics.SphereCast(ray, 0.0015f, distance, layerMask))
        {
            return true;
        }

        return false;
    }
}
