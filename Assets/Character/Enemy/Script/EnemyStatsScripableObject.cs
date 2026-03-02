using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScripableObject", menuName = "ScriptableObjects/Enemy/EnemyStatsScripableObject")]
public class EnemyStatsScripableObject : ScriptableObject
{
    [Range(1, 500)]
    public float maxHp;

    [Range(1, 500)]
    public float maxPosture;
}
