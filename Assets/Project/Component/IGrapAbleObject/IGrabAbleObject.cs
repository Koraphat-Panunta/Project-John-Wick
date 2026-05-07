using UnityEngine;

public interface IGrabAbleObject
{
    Transform _grabSocketTransform { get; }
    IObjectGrabbedAble _currentGrabbedObject { get; }
    void GrabAttach(IObjectGrabbedAble grabAble,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration);
    void GrabDetach();

    public static T GetCurentGrabAbleObjectAs<T>(IGrabAbleObject grabRangeWeaponAble) where T : IObjectGrabbedAble
    {
        return grabRangeWeaponAble._currentGrabbedObject is T returnObject ? returnObject : default;
    }
   
}
