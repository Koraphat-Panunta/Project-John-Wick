using UnityEngine;

public partial class Enemy 
{
    [SerializeField] public EnemyStatsScripableObject enemyStatsScripableObject;
    [SerializeField] public FindingTargetScriptableObject findingTargetScriptableObject;

    public bool isReactAble => this.reactionTime.IsFull();
    public Gauge reactionTime;

}
