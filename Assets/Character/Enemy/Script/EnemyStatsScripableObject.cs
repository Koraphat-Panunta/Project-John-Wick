using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScripableObject", menuName = "ScriptableObjects/Enemy/EnemyStatsScripableObject")]
public class EnemyStatsScripableObject : ScriptableObject
{
    [Range(1, 500)]
    public float maxHp;

    [Range(1, 500)]
    public float maxPosture;

    [Range(1, 10)]
    public float reactionTime;
}
