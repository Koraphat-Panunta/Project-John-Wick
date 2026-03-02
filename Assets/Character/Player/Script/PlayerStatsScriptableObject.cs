using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "ScriptableObjects/Player/PlayerStatsScriptableObject")]
public class PlayerStatsScriptableObject : ScriptableObject
{
    [Range(1, 500)]
    public float maxHP;

    [Range(1,500)]
    public float maxStamina;

    [Range(1, 500)]
    public float limitExecuteGauge;
    [Range(1, 100)]
    public float addExecuteGauge;

    [Range(1, 10)]
    public float delayStaminaDrain;

    [Range(1, 500)]
    public float HitStaminaDrain;

    [Range(1, 500)]
    public float dodgeStaminaDrain;

    [Range(1, 500)]
    public float dolphinDiveStaminaDrain;

    [Range(1, 500)]
    public float restrainHumanShieldStaminaDrain;

    [Range(1, 500)]
    public float WeaponDisarmStaminaDrain;
}
