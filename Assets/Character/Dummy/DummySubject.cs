using UnityEngine;

public class DummySubject : Character
{
    [SerializeField] private CharacterController characterController;
    public override void Initialized()
    {
        _movementCompoent = new DummyMovementComponent(this.transform,this,this.characterController);
        base.Initialized();
    }
    public override MovementCompoent _movementCompoent { get ; set ; }
}
