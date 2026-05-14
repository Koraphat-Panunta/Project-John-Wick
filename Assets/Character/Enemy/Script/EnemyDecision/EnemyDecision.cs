using UnityEngine;
using System.Collections.Generic;
public abstract class EnemyDecision : MonoBehaviour,IInitializedAble
{
    public EnemyDecisionContext enemyDecisionContext;
    public EnemyCommandAPI enemyCommand;
    public Enemy enemy;
    public virtual void Initialized()
    {
        this.enemy.NotifyGotHearing += OnNotifyHearding;
        this.enemy.NotifyEnemySpottingTarget += OnNotifySpottingTarget;
    }
   

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {

    }
    protected List<IObserverEnemyDecision> observerEnemyDecisions = new List<IObserverEnemyDecision>();
    public void AddEnemyDecisionObserver(IObserverEnemyDecision observerEnemyDecision)
    {
        observerEnemyDecisions.Add(observerEnemyDecision);
    }
    public void RemoveEnemyDecisionObserver(IObserverEnemyDecision observerEnemyDecision)
    {
        observerEnemyDecisions.Remove(observerEnemyDecision);
    }
    public void NotifyEnemyDecision<T>(EnemyDecision enemyDecision,T var)
    {
        for(int i = 0;i < observerEnemyDecisions.Count; i++)
        {
            this.observerEnemyDecisions[i].OnNotifyEnemyDecision(enemyDecision, var);
        }
    }
    protected abstract void OnNotifyHearding(INoiseMakingAble noiseMaker);
    protected abstract void OnNotifySpottingTarget(GameObject target);
    protected virtual void OnValidate()
    {
    }

  
}
public interface IObserverEnemyDecision
{
    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var);
 
}

public static class EnemyDecisionInjectionEvent
{
    public static void OnHearding(
        INoiseMakingAble noiseMaker
        , EnemyDecisionContext enemyDecisionContext
        )
    {

        if (enemyDecisionContext.combatPhase == CombatPhase.Alert)
            return;

        if (noiseMaker is Bullet bullet
            && bullet.weapon.userWeapon._character.gameObject.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted i_NPCTargetAble))
        {    
            enemyDecisionContext.SetCombatPhase(CombatPhase.Aware);

            enemyDecisionContext._targetZone.SetZone(noiseMaker.position, enemyDecisionContext.raduisTargetZone);
        }
    }

    public static void OnSpotingTarget(Transform target,EnemyDecisionContext enemyDecisionContext)
    {

        enemyDecisionContext.SetCombatPhase(CombatPhase.Alert);
        enemyDecisionContext._targetZone.SetZone(target.transform.position, enemyDecisionContext.raduisTargetZone);

        enemyDecisionContext.elapesLostSightTime = 0;
    }
}
