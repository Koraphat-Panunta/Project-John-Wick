using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(EnemyCommandAPI))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyDecision : MonoBehaviour,IInitializedAble
{
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
        enemy = GetComponent<Enemy>();
        enemyCommand = GetComponent<EnemyCommandAPI>();
    }

  
}
public interface IObserverEnemyDecision
{
    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var);
 
}
