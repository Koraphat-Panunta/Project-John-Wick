using UnityEngine;

public class DummySubject : Character
{
    [SerializeField] private CharacterController characterController;
    public override void Initialized()
    {
        _movementCompoent = new DummyMovementComponent(this.transform,this,this.characterController);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position,Vector3.one * 0.1f);
    }
}
