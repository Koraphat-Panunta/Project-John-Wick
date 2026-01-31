using UnityEngine;

public interface IQuickSwitchNode : INodeLeaf
{
    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    public static float adsSpeed = 40;
}
