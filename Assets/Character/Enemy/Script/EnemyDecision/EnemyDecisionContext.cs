using UnityEngine;


public class EnemyDecisionContext
{
    public EnemyDecisionContext(CombatPhase startCombatPhase
        ,EnemyRoleCommand roleCommand
        ,float raduisTargetZone
        ,float lostSightTime)
    {
        this.combatPhase = startCombatPhase;
        this.roleCommand = roleCommand;
        this.raduisTargetZone = raduisTargetZone;
        this.lostSightTime = lostSightTime;

        this._targetZone = new ZoneDefine(Vector3.zero,this.raduisTargetZone);
    }

    public EnemyDecisionContext(CombatPhase startCombatPhase, EnemyRoleCommand startRoleCommand,EnemyDecisionContextScriptableObject enemyDecisionContextScriptableObject) 
        : this(
              startCombatPhase
              ,startRoleCommand
              ,enemyDecisionContextScriptableObject.raduisTargetZone
              ,enemyDecisionContextScriptableObject.lostSightTargetTime
              )
    {

    }

    public void SetCombatPhase(CombatPhase combatPhase) => this.combatPhase = combatPhase; 
    public CombatPhase combatPhase { get; protected set; }

    public void SetRoleCommand(EnemyRoleCommand enemyRoleCommand) => this.roleCommand = enemyRoleCommand;
    public EnemyRoleCommand roleCommand { get; protected set; }

    public ZoneDefine _targetZone { get; protected set; }

    public float raduisTargetZone;

    public float elapesLostSightTime;
    public float lostSightTime;
}
