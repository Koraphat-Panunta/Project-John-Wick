using UnityEngine;

[System.Serializable]
public struct EnemySpawnerData
{
    public EnemyDataScriptableObject enemyData;
    public WeaponSpawnerData weaponSpawnerData;
    public EnemyDirector enemyDirector;
    public int numberSpawn;

    public EnemyRoleCommand spawnRole ;
    public CombatPhase spawnCombatPhase;

    [SerializeReference]
    public EnemySpawnerExtensionData[] extensions;
}



