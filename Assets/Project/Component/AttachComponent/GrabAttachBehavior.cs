using UnityEngine;

public static class GrabAttachBehavior
{
    public static void Attach(IGrabAbleObject grabAble, IObjectGrabbedAble socket)
        => Attach(grabAble, socket, Vector3.zero, Quaternion.identity, 0f);

    public static void Attach(IGrabAbleObject grabAble, IObjectGrabbedAble socket,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration)
    {
        if (grabAble == null || socket == null)
            return;

        if (grabAble.currentGrabbedAt != null && grabAble.currentGrabbedAt != socket)
            grabAble.currentGrabbedAt.GrabDetach();

        socket.GrabAttach(grabAble,
            additionalOffsetPosition,
            additionalOffsetRotation,
            attachingDuration);
    }

    public static void Detach(IGrabAbleObject grabAble)
    {
        if (grabAble == null)
            return;

        if (grabAble.currentGrabbedAt != null)
            grabAble.currentGrabbedAt.GrabDetach();
    }
}
