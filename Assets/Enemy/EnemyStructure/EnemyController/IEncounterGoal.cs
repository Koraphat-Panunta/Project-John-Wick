using UnityEngine;

public interface IEncounterGoal 
{
   public IEnemyGOAP _enemyGOAP { get; }
   public IFindingTarget _findingTarget { get;}
   public EncouterGoal _encouterGoal { get; set; }
}
