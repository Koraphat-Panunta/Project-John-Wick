using System;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Muzzle :WeaponAttachment
{
    protected abstract float recoilController{ get; set; }
    protected abstract float aimDownSightSpeed { get; set; }
    protected abstract Transform bulletSpawnPos { get; set; }
    private void Awake()
    {
        
    }
    public override AttachmentSlot myAttachmentSlot {
        get { return AttachmentSlot.MUZZLE; } 
        protected set => throw new NotImplementedException(); 
    }

    public override void Attach(Weapon weapon)
    {
        weapon.RecoilController += recoilController;
        weapon.aimDownSight_speed += aimDownSightSpeed;
        weapon.bulletSpawnerPos = bulletSpawnPos;
        base.Attach(weapon);
    }
}
