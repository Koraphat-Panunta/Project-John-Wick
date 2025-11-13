using UnityEngine;

public class DummyMovementComponent : MovementCompoent
{
    CharacterController characterController;
    public DummyMovementComponent(Transform transform, MonoBehaviour myMovement, CharacterController characterController) : base(transform, myMovement)
    {
        this.characterController = characterController;
    }

    public MovementNodeLeaf restMovementNodeLeaf { get; set; }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "StartNodeSelector PlayerMovement");

        onUpdateMovementNodeLeaf = new OnUpdateMovementNodeLeaf(() => isOnUpdateEnable, this);
        restMovementNodeLeaf = new MovementNodeLeaf(() => true);

        startNodeSelector.AddtoChildNode(onUpdateMovementNodeLeaf);
        startNodeSelector.AddtoChildNode(restMovementNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }

    public override void Move(Vector3 position)
    {
        characterController.Move(position);
    }
}
