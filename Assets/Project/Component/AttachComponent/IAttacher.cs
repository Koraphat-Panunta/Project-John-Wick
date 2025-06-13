using UnityEngine;
using UnityEngine.Animations;

public interface IAttacher<TAttachable>
{
    Transform transform { get; }
    ParentConstraint parentConstraint { get; }
    Vector3 offsetPosition { get; set; }
    Vector3 offsetRotation { get; set; }
    TAttachable CurrentAttachable { get; set; }
    void Attach(TAttachable attachable);
    void Detach();
}

