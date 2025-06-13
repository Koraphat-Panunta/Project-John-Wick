using UnityEngine;

public interface IMicroOpticAttachAble : IWeaponAttachmentAttachAble
{
    public Transform _microOpticSocket { get; set; }
    public MicroOpticWeaponAttachment _microOptic { get; set; }

    public float _accuracyAdditional { get; set; }
    public float _min_PrecisionAdditional { get; set; }
    public float _max_PrecisionAdditional { get; set; }
    public float _aimDownSightSpeedAdditional { get; set; }
    //public 
}
