using UnityEngine;
using UnityEngine.Animations;

public interface IWeaponAttachAble
{
    public Transform anchor { get; set; }
    public ParentConstraint constraint { get; set; }
    public Transform center { get; set; }
}
