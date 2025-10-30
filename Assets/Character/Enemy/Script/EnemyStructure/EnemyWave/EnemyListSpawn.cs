using UnityEngine;

[System.Serializable]
public struct EnemyListSpawn
{
    public EnemyObjectManager enemyObjectManager;
    public WeaponObjectManager weaponObjectManager;
    public int numberSpawn;

    public EnemyListSpawn(EnemyObjectManager enemyObjectManager, WeaponObjectManager weaponObjectManager, int numberSpawn)
    {
        this.enemyObjectManager = enemyObjectManager;
        this.weaponObjectManager = weaponObjectManager;
        this.numberSpawn = numberSpawn;
    }
}
