using UnityEngine;

public static class RangeWeaponSocketBehavior
{

    public static void GrabAttach(
        IRangeWeaponAdvanceUser weaponAdvanceUser,
        IGrabRangeWeaponAble socket
        , RangeWeapon rangeWeapon,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration)
    {

        rangeWeapon._weaponAttacherComponent.Attach(
            socket.weaponAttachingAbleTransform,
            rangeWeapon._mainHandGripTransform,
            additionalOffsetPosition,
            additionalOffsetRotation,
            attachingDuration);

        rangeWeapon.SetCurAttatchAble(socket);
    }

    public static void GrabDetach(IGrabRangeWeaponAble socket)
    {
        if (socket.curRangeWeaponAtSocket == null)
            return;

        RangeWeapon weapon = socket.curRangeWeaponAtSocket;

        weapon.SetCurAttatchAble(null);
        weapon._weaponAttacherComponent.Detach();
    }
}
