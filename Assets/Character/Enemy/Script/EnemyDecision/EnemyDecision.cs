using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(EnemyCommandAPI))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyDecision : MonoBehaviour
{
    public abstract EnemyCommandAPI enemyCommand { get; set; }
    public Enemy enemy;
    
    protected virtual void Awake()
    {
        enemyCommand = GetComponent<EnemyCommandAPI>();
        this.enemy = GetComponent<Enemy>();
        this.enemy.NotifyGotHearing += OnNotifyHearding;
        this.enemy.NotifyEnemySpottingTarget += OnNotifySpottingTarget;
    }
    protected virtual void Start()
    {
        
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
    }
}
public interface IObserverEnemyDecision
{
    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var);
 
}
