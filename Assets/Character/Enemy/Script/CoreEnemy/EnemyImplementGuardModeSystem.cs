using UnityEngine;

public partial class Enemy
{
    public float maxGuardGauge => this.enemyStatsScripableObject.guardGuage;

    public Gauge guardGauge;
    public bool isGuardModeEnabled
    {
        get => this.enemyStateManagerNode.enemyStateNodeComponentManager.CheckNodeIsActive(this.enemyStateManagerNode.guardModeComponentNodeLeaf);
    }
    public bool _triggerEvade { get; set; }
    public bool _triggerBlock { get; set; }

    public void InitializeGuardSystem()
    {
        guardGauge = new Gauge(maxGuardGauge, maxGuardGauge);
    }

    public void ResetGuardSystem()
    {
        this.guardGauge.SetGauge(maxGuardGauge);
    }
}
