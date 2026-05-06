using UnityEngine;

public static class WeaponSocketBehavior
{
    public static void Attatch(IGrabWeaponAble socket, Weapon weapon)
    {
        weapon.SetCurAttatchAble(socket);
    }

    public static void Detach(IGrabWeaponAble socket)
    {
        Weapon weapon = socket.curWeaponAtSocket;
        if (weapon == null)
            return;

        weapon.SetCurAttatchAble(null);
        weapon._weaponAttacherComponent.Detach();
    }

    public static void GrabAttach(IGrabWeaponAble socket, IGrabAbleObject grabAble,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attachingDuration)
    {
        if (grabAble is Weapon weapon)
            socket.Attatch(weapon, additionalOffsetPosition, additionalOffsetRotation, attachingDuration);
    }

    public static void GrabDetach(IGrabWeaponAble socket)
    {
        if (socket.curWeaponAtSocket != null)
            socket.Detach();
    }
}
