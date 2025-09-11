using UnityEngine;

public class EnemyAutoDefendCommand : IObserverEnemy
{
    protected EnemyCommandAPI enemyCommandAPI;
    protected Enemy enemy;
    protected IWeaponAdvanceUser targerFireArmed;
    protected GameObject target;

    public float dodgeCoolDownTimer;
    protected float minDodgeCoolDownTime = 8;
    protected float maxDodgeCoolDownTime = 17;

    protected float gotHitedReactionDefendTime;
    public EnemyAutoDefendCommand(EnemyCommandAPI enemyCommandAPI)
    {
        this.enemyCommandAPI = enemyCommandAPI;
        this.enemy = enemyCommandAPI._enemy;
        dodgeCoolDownTimer = Random.Range(minDodgeCoolDownTime, maxDodgeCoolDownTime);
        this.enemy.AddObserver(this);
    }
    public void UpdateAutoDefend()
    {
        if(IsBeenAimedAt() && this.dodgeCoolDownTimer <= 0)
        {
            enemyCommandAPI.Dodge(Quaternion.AngleAxis(Random.Range(-30,30),Vector3.up)*(enemy.transform.right * (Random.value > 0.5f?1:-1))) ;
        }
        else if(gotHitedReactionDefendTime > 0 && dodgeCoolDownTimer <= 0 && ((enemy.GetHP()/enemy.GetMaxHp()) <= enemy.GetMaxHp()*0.8f))
        {
            enemyCommandAPI.Dodge(Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.up) * (enemy.transform.forward * -1));
        }
    }
    protected bool IsBeenAimedAt()
    {

        if(enemy.target == null)
            return false;
        Debug.Log("enemy.target != null");
        if (this.target == null)
        {
            this.target = enemy.target;
            if (this.target.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted enemyAITargeted)
                && enemyAITargeted.selfEnemyAIBeenTargeted is IWeaponAdvanceUser weaponAdvanceUser)
            {
                //Debug.Log("this.targerFireArmed = weaponAdvanceUser;");
                this.targerFireArmed = weaponAdvanceUser;
            }
            else
            {
                //Debug.Log("this.targerFireArmed = null;");
                this.targerFireArmed = null;
            }
        }

        if(this.target != enemy.target)
        {
            this.target = enemy.target;
            if (this.target.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted enemyAITargeted)
                && enemyAITargeted.selfEnemyAIBeenTargeted is IWeaponAdvanceUser weaponAdvanceUser)
            {
                Debug.Log("this.targerFireArmed = weaponAdvanceUser;");
                this.targerFireArmed = weaponAdvanceUser;
            }
            else
            {
                Debug.Log("this.targerFireArmed = null;");
                this.targerFireArmed = null;
            }
        }

        if (this.targerFireArmed == null)
            return false;
        Debug.Log("this.targerFireArmed != null;");
        return EnemyBewareAnalysis.IsTargetAimingTo(this.targerFireArmed, enemy.transform.position, 1.7f,12f);

    }
    public void UpdateDefendActionBlackBoard()
    {
        if(dodgeCoolDownTimer > 0)
            dodgeCoolDownTimer -= Time.deltaTime;

        if(gotHitedReactionDefendTime > 0)
            gotHitedReactionDefendTime -= Time.deltaTime;
    }

    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyDodgeRollStateNodeLeaf enemyDodgeRollStateNodeLeaf && enemyDodgeRollStateNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Exit)
            this.dodgeCoolDownTimer = Random.Range(minDodgeCoolDownTime, maxDodgeCoolDownTime);

        if (node is SubjectEnemy.EnemyEvent enemyEvent && enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit)
        {
            dodgeCoolDownTimer -= 1;
            //Debug.Log("dodgeCoolDownTimer got shooted = " + dodgeCoolDownTimer);
        }

        if (node is GotGunFuHitNodeLeaf gotGunFuHitNodeLeaf)
        {
            gotHitedReactionDefendTime += 1.25f;
            dodgeCoolDownTimer -= 1;
            Debug.Log("dodgeCoolDownTimer got hit = "+dodgeCoolDownTimer);
        }

        if (node is GotRestrictNodeLeaf gotRestrictNodeLeaf && gotRestrictNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Exit)
        {
            gotHitedReactionDefendTime += 3;
            dodgeCoolDownTimer -= 3;
        }
        
    }
}
