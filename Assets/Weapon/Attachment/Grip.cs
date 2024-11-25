using UnityEngine;

public abstract class Grip : WeaponAttachment
{
    protected abstract float recoilCameraKickBackController { get; set; }
    protected abstract float aimDownSightSpeed { get; set; }
    protected abstract float movementSpeed  {get; set; }
    protected abstract float accuracy { get; set; }
    protected override AttachmentSlot myAttachmentSlot { get => AttachmentSlot.GRIP; set => throw new System.NotImplementedException(); }
    public override void Attach(Weapon weapon)
    {
        weapon.RecoilCameraKickBack += recoilCameraKickBackController;
        weapon.aimDownSight_speed += aimDownSightSpeed;
        weapon.movementSpeed += movementSpeed;
        weapon.Accuracy += accuracy;
        base.Attach(weapon);
    }
}
