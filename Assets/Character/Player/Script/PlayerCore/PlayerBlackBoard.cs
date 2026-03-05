using UnityEngine;

public partial class Player 
{
    [SerializeField] public PlayerStatsScriptableObject playerStatsScriptableObject;

    public override float StandMoveMaxSpeed 
    {
        get 
        {
            try
            {
                return base.StandMoveMaxSpeed * ((this._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>()? .8f : 1);
            }
            catch
            {
                return base.StandMoveMaxSpeed;
            }

        }
    } 
    public bool CompareStamina(float value)
    {
        if(this.staminaGauge._gauge >= value)
            return true;
        return false;
    }
}
