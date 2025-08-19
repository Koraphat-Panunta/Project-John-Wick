using UnityEngine;
using UnityEngine.Animations;

public interface IAttacher<TAttachable>
{
    Transform _transform { get; }
    ParentConstraint _parentConstraint { get; }
    Vector3 _offsetPosition { get; set; }
    Vector3 _offsetRotation { get; set; }
    TAttachable _CurrentAttachable { get; set; }
    void Attach(TAttachable attachable);
    void Detach();
}

