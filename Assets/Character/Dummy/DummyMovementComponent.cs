using UnityEngine;

public class DummyMovementComponent : MovementCompoent
{
    protected CharacterMovementController characterMovementController;
    public DummyMovementComponent(Transform transform, MonoBehaviour myMovement, CharacterMovementController characterController) : base(transform, myMovement)
    {
        this.characterMovementController = characterController; 
    }

    public override Vector3 curPosition { get => this.characterMovementController.position; }
    public override Quaternion curRotation => this.characterMovementController.rotation;

    public override void Move(Vector3 position)
    {
        this.characterMovementController.Move(position);
    }

    public override void SetRotation(Quaternion rotation)
    {
        characterMovementController.SetRotation(rotation);
    }

    public override void ForceUpdateTransform()
    {
        this.characterMovementController.UpdateCharacterPosition();
        this.characterMovementController.UpdateCharacterRotation();
    }
}
