using UnityEngine;

[System.Serializable]
public struct EnemyDetailSpawn
{
    public EnemyObjectManager enemyObjectManager;
    public WeaponObjectManager weaponObjectManager;
    public int numberSpawn;

    public EnemyDetailSpawn(EnemyObjectManager enemyObjectManager, WeaponObjectManager weaponObjectManager, int numberSpawn)
    {
        this.enemyObjectManager = enemyObjectManager;
        this.weaponObjectManager = weaponObjectManager;
        this.numberSpawn = numberSpawn;
    }
}
