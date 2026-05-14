using UnityEngine;


public class EnemyDecisionContext
{
    public EnemyDecisionContext(
        float raduisTargetZone
        ,float lostSightTime
        ,CombatPhase startCombatPhase = CombatPhase.Suspect
        , EnemyRoleCommand roleCommand = EnemyRoleCommand.Support)
    {
        this.combatPhase = startCombatPhase;
        this.roleCommand = roleCommand;
        this.raduisTargetZone = raduisTargetZone;
        this.lostSightTime = lostSightTime;

        this._targetZone = new ZoneDefine(Vector3.zero,this.raduisTargetZone);
    }

    public EnemyDecisionContext(
        EnemyDecisionContextScriptableObject enemyDecisionContextScriptableObject
        ,CombatPhase startCombatPhase = CombatPhase.Suspect
        , EnemyRoleCommand startRoleCommand = EnemyRoleCommand.Support) 
        : this(
              enemyDecisionContextScriptableObject.raduisTargetZone
              ,enemyDecisionContextScriptableObject.lostSightTargetTime
               ,startCombatPhase
              , startRoleCommand
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
