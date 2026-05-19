using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScripableObject", menuName = "ScriptableObjects/Enemy/EnemyStatsScripableObject")]
public class EnemyStatsScripableObject : ScriptableObject
{
    [Range(1, 500)]
    public float maxHp;

    [Range(1, 500)]
    public float maxPosture;

    [Range(1, 500)]
    public float guardGuage;
    [Range(1, 20)]
    public float guardTime;
    [Range(1, 50)]
    public float guardCoolDownTime;

    [Range(1, 100)]
    public float minDodgeCoolDownTime;
    [Range(1, 100)]
    public float maxDodgeCoolDownTime;

    [Range(1, 100)]
    public float minDodgeHitRate;
    [Range(1, 100)]
    public float maxDodgeHitRate;

    [Range(1, 100)]
    public float minGuardRate;
    [Range(1, 100)]
    public float maxGuardRate;

    [Range(1, 100)]
    public float minCounterRate;
    [Range(1, 100)]
    public float maxCounterRate;


    [Range(1, 10)]
    public float reactionTime;
}
