using UnityEngine;

public class DummyMovementComponent : MovementCompoent
{
    public DummyMovementComponent(Transform transform, MonoBehaviour myMovement, CharacterController characterController) : base(transform, myMovement)
    {

        this._curPos = transform.position;
    }

    public MovementNodeLeaf restMovementNodeLeaf { get; set; }

    public override Vector3 curPosition { get => this._curPos; }
    protected Vector3 _curPos;

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
        this.transform.position = this._curPos;
        base.FixedUpdateNode();
    }
    public override void Move(Vector3 position)
    {
        this._curPos += position;
    }
}
