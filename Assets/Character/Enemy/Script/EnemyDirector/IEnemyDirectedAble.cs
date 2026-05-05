using UnityEngine;

public interface IEnemyDirectedAble 
{
    public EnemyRoleCommand _curCommandPerforme { get; protected set; }
    public CombatPhase _combatPhase { get; }
    public Enemy _enemy { get; }
    public EnemyCommandAPI _enemyCommandAPI { get;}
    public EnemyDecision _enemyDecision { get; }
    public void SetDirectorCommand(EnemyRoleCommand directorCommand) => this._curCommandPerforme = directorCommand;
}
