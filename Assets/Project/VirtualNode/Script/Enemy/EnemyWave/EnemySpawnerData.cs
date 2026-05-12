using UnityEngine;

[System.Serializable]
public struct EnemySpawnerData
{
    public EnemyDataScriptableObject enemyData;
    public WeaponObjectManager weaponObjectManager;
    public EnemyDirector enemyDirector;
    public int numberSpawn;

    [SerializeReference]
    public EnemySpawnerExtensionData[] extensions;
}
