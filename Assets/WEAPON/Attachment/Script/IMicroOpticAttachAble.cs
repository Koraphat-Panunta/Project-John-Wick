using UnityEngine;

public interface IMicroOpticAttachAble : IWeaponAttachmentAttachAble
{
    public Transform _microOpticSocket { get; set; }
    public MicroOpticWeaponAttachment _microOptic { get; set; }

    public float _reduceMinCrosshairSize { get; set; }
    public float _reduceMaxCrosshairSize { get; set; }
    public float _aimDownSightSpeedIncrease { get; set; }
    //public 
}
