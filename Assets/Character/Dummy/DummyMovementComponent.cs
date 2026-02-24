using UnityEngine;

public class DummyMovementComponent : MovementCompoent
{
    protected CharacterMovementController characterMovementController;
    public DummyMovementComponent(Transform transform, MonoBehaviour myMovement, CharacterMovementController characterController) : base(transform, myMovement)
    {
        this.characterMovementController = characterController; 
    }

    public MovementNodeLeaf restMovementNodeLeaf { get; set; }

    public override Vector3 curPosition { get => this.characterMovementController.position; }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "StartNodeSelector PlayerMovement");

        onUpdateMovementNodeLeaf = new OnUpdateMovementNodeLeaf(() => isOnUpdateEnable, this);
        restMovementNodeLeaf = new MovementNodeLeaf(() => true);

        startNodeSelector.AddtoChildNode(onUpdateMovementNodeLeaf);
        startNodeSelector.AddtoChildNode(restMovementNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void Move(Vector3 position)
    {
        this.characterMovementController.Move(position);
    }
}
