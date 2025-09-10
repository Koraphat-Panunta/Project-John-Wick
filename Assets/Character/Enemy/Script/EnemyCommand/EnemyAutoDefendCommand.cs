using UnityEngine;

public class EnemyAutoDefendCommand 
{
    protected EnemyCommandAPI enemyCommandAPI;
    protected Enemy enemy;
    protected IWeaponAdvanceUser targerFireArmed;
    protected GameObject target;

    protected float dodgeCoolDownTimer;
    protected float minDodgeCoolDownTime = 6;
    protected float maxDodgeCoolDownTime = 9;
    public EnemyAutoDefendCommand(EnemyCommandAPI enemyCommandAPI)
    {
        this.enemyCommandAPI = enemyCommandAPI;
        this.enemy = enemyCommandAPI._enemy;
        dodgeCoolDownTimer = Random.Range(minDodgeCoolDownTime, maxDodgeCoolDownTime);
    }
    public void UpdateAutoDefend()
    {
        if (IsBeenAimedAt())
        {

        }
    }
    protected bool IsBeenAimedAt()
    {
        
        if(enemy.target == null)
            return false;

        if(this.target == null)
            this.target = enemy.target;

        if(this.target != enemy.target)
        {
            this.target = enemy.target;
            if(this.target.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser weaponAdvanceUser))
                this.targerFireArmed = weaponAdvanceUser;
            else
                this.targerFireArmed = null;
        }

        if (this.targerFireArmed == null)
            return false;

        return EnemyBewareAnalysis.IsTargetAimingTo(this.targerFireArmed, enemy.transform.position, 1);

    }
    public void CoolDownDefendAction()
    {
        if(dodgeCoolDownTimer > 0)
            dodgeCoolDownTimer -= Time.deltaTime;
    }
}
