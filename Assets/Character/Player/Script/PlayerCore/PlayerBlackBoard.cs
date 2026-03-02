using UnityEngine;

public partial class Player 
{
    [SerializeField] public PlayerStatsScriptableObject playerStatsScriptableObject;

    public bool CompareStamina(float value)
    {
        if(this.staminaGauge._gauge >= value)
            return true;
        return false;
    }
}
