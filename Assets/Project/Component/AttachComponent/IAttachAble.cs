using UnityEngine;

public interface IAttachable<TAttacher>
{
    Transform _transform { get; }
    TAttacher CurrentAttacher { get; set; }
    void OnAttached(TAttacher attacher);
    void OnDetached(TAttacher attacher);
}

