using UnityEngine;

public interface IObjectGrabbedAble
{
    Transform grabSocketTransform { get; }
    IGrabAbleObject currentGrabbedObject { get; }
    void GrabAttach(IGrabAbleObject grabAble,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration);
    void GrabDetach();
}
