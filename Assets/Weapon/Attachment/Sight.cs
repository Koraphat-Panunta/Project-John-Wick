using UnityEngine;

public abstract class Sight : WeaponAttachment
{
    protected abstract float min_Precision { get; set; }
    protected abstract float max_Precision { get; set; }
    protected abstract float accuracy { get; set; }
    protected abstract float aimDownSightSpeed { get; set; }
    protected override AttachmentSlot myAttachmentSlot { get => AttachmentSlot.SCOPE;set => throw new System.NotImplementedException(); }

    public override void Attach(Weapon weapon)
    {
        weapon.min_Precision += this.min_Precision;
        weapon.max_Precision += this.max_Precision;
        weapon.Accuracy += this.accuracy;
        weapon.aimDownSight_speed += this.aimDownSightSpeed;

        base.Attach(weapon);
    }

}

   
