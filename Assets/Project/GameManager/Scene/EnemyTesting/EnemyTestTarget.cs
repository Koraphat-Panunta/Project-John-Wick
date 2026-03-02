using UnityEngine;

public class EnemyTestTarget : Character,I_EnemyAITargeted
{
    public Character selfEnemyAIBeenTargeted => this;

    public override MovementCompoent _movementCompoent { get; set; }

    public override Stance stance => throw new System.NotImplementedException();

    public override Gauge _hpGauge { get ; protected set ; }

    public override void Initialized()
    {
        _hpGauge = new Gauge(100,100);
        _movementCompoent = GetComponent<MovementCompoent>();
        base.Initialized();
    }
}
