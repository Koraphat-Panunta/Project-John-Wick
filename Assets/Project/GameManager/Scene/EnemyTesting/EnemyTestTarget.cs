using UnityEngine;

public class EnemyTestTarget : Character,I_NPCTargetAble
{
    public Character selfNPCTarget => this;

    public override MovementCompoent _movementCompoent { get; set; }
    protected override void Awake()
    {
        _movementCompoent = GetComponent<MovementCompoent>();
        base.Awake();
    }
}
