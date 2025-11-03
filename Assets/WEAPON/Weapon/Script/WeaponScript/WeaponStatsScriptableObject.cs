using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatsScriptableObject", menuName = "ScriptableObjects/Weapon/WeaponStats")]
public class WeaponStatsScriptableObject : ScriptableObject
{
    public int bulletCapacity;
    [Range(1,2000)]
    public float rate_of_fire;
    [Range(1,10)]
    public float reloadSpeed;
    [Range(1, 10)]
    public float Recovery_CrosshairBloomSpeed;
    [Range(1, 10)]
    public float Recovery_CrosshairPositionSpeed;
    [Range(1, 10)]
    public float Recoil_CrosshairBloomController;
    [Range(1, 10)]
    public float Recoil_KickVerticalCrosshairBloomController;
    [Range(1, 10)]
    public float Recoil_KickHorizontalCrosshairBloomController;
    [Range(1, 10)]
    public float Recoil_CameraControlController;
    [Range(1, 10)]
    public float Recoil_VisualImpulseControl;
    [Range(1, 10)]
    public float min_CrosshairSize;
    [Range(1, 10)]
    public float max_CrosshairSize;
    [Range(1, 10)]
    public float aimDownSight_speed;
}
