using UnityEngine;

public class EnemyTestTarget : Character,I_EnemyAITargeted
{
    public Character selfEnemyAIBeenTargeted => this;

    public override MovementCompoent _movementCompoent { get; set; }
    public override void Initialized()
    {
        _movementCompoent = GetComponent<MovementCompoent>();
        base.Initialized();
    }
}
