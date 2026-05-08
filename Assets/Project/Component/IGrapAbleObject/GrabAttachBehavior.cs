using UnityEngine;

public static class GrabAttachBehavior
{
    public static void Attach(IObjectGrabbedAble grabAble, IGrabAbleObject socket)
        => Attach(grabAble, socket, Vector3.zero, Quaternion.identity, 0f);

    public static void Attach(IObjectGrabbedAble grabAble, IGrabAbleObject socket,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration)
    {
        if (grabAble == null || socket == null)
            return;

        if (grabAble._currentGrabbedAt != null && grabAble._currentGrabbedAt != socket)
            grabAble._currentGrabbedAt.GrabDetach();

        socket.GrabAttach(grabAble,
            additionalOffsetPosition,
            additionalOffsetRotation,
            attachingDuration);
    }

    public static void Detach(IObjectGrabbedAble grabAble)
    {
        if (grabAble == null)
            return;

        if (grabAble._currentGrabbedAt != null)
            grabAble._currentGrabbedAt.GrabDetach();
    }
}
