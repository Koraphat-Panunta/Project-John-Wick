using System;
using UnityEngine;

public class EnemySpawnerPoint : MonoBehaviour 
{

    public virtual Vector3 spawnPosition { get => transform.position; protected set { } }
    public virtual Quaternion spawnRotiation { get => transform.rotation; protected set { } }
    public Func<bool> preCondition { get; protected set; }
    protected virtual void Awake()
    {
        preCondition = () => true;
    }
    public void SetPrecondition(Func<bool> preCondition)=> this.preCondition = preCondition;

    public virtual bool SpawnEnemy(EnemyObjectManager enemyObjectManager,EnemyDirector enemyDirector,WeaponObjectManager weaponObjectManager,out Enemy enemy)
    {
        enemy = null;
        if(preCondition.Invoke() == false)
            return false;
        enemy = enemyObjectManager.SpawnEnemy(this.spawnPosition, this.spawnRotiation, enemyDirector);
        Weapon weapon = weaponObjectManager.SpawnWeapon(enemy);

        return true;
    }
    public virtual bool SpawnEnemy(EnemyObjectManager enemyObjectManager, EnemyDirector enemyDirector, WeaponObjectManager weaponObjectManager)
    {
        return this.SpawnEnemy(enemyObjectManager, enemyDirector, weaponObjectManager, out Enemy enemy); ;
    }
    public virtual bool SpawnEnemy(EnemyObjectManager enemyObjectManager, WeaponObjectManager weaponObjectManager)
    {
        if (preCondition.Invoke() == false)
            return false;

        Enemy enemy = enemyObjectManager.SpawnEnemy(this.spawnPosition, this.spawnRotiation);
        Weapon weapon = weaponObjectManager.SpawnWeapon(enemy);

        return true;
    }
    public virtual bool SpawnEnemy(EnemyObjectManager enemyObjectManager)
    {
        if (preCondition.Invoke() == false)
            return false;

        Enemy enemy = enemyObjectManager.SpawnEnemy(this.spawnPosition, this.spawnRotiation);

        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.spawnPosition,0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.spawnPosition, transform.forward);
    }

}
