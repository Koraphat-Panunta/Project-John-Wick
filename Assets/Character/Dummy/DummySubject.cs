using UnityEngine;

public class DummySubject : Character
{

    public override Gauge _hpGauge { get ; protected set ; }

    public override void Initialized()
    {
        this._hpGauge = new Gauge(100,100);
        this._movementCompoent = new DummyMovementComponent(this.transform,this,this.characterController);
        base.Initialized();
    }
 
    private void Update()
    {
        _movementCompoent.UpdateNode();
    }
    private void FixedUpdate()
    {
        _movementCompoent.FixedUpdateNode();
    }
    public override MovementCompoent _movementCompoent { get ; set ; }

    public override Stance stance => Stance.stand;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position,Vector3.one * 0.1f);
    }
}
