using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DummySubject : Character
{
    [SerializeField] private CharacterController characterControllerComponent;
    public override void Initialized()
    {
        this._movementCompoent = new DummyMovementComponent(this.transform,this,this.characterControllerComponent);
        base.Initialized();
    }
    protected override void OnAnimatorMove()
    {
        if (this.enableRootMotion)
        {
            this._movementCompoent.SetPosition(this._movementCompoent.curPosition + animator.deltaPosition);
            this._movementCompoent.SetRotation(this.transform.rotation * animator.deltaRotation);
        }
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
