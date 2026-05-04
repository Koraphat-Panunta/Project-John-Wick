using UnityEngine;

public interface IEnemyDirectedAble 
{
    public enum DirectorCommand
    {
        Ambush,
        Support,
    }
    public DirectorCommand curCommandPerforme { get; set; }
    public IEnemyActionNodeManagerImplementDecision.CombatPhase combatPhase { get; }
    public Enemy _enemy { get; }
    public EnemyCommandAPI _enemyCommandAPI { get;}
    public EnemyDecision _enemyDecision { get; }
    public void SetDirectorCommand(DirectorCommand directorCommand) => this.curCommandPerforme = directorCommand;
}
