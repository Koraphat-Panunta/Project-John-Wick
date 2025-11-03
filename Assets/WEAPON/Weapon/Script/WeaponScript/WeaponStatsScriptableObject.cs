using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatsScriptableObject", menuName = "ScriptableObjects/Weapon/WeaponStats")]
public class WeaponStatsScriptableObject : ScriptableObject
{
    public int bulletCapacity;
    [Range(1,2000)]
    public float rate_of_fire; //fire per 60 second
    [Range(0.1f,10)]
    public float reloadTime; 
    [Range(1, 10)]
    public float Recovery_CrosshairBloomSpeed;
    [Range(1, 10)]
    public float Recovery_CrosshairPositionSpeed;
    [Range(1, 10)]
    public float Recoil_CrosshairBloomController;
    [Range(1, 10)]
    public float Recoil_KickVerticalPositionCrosshairController;
    [Range(1, 10)]
    public float Recoil_KickHorizontalPositionCrosshairController;
    [Range(1, 10)]
    public float Recoil_CameraControlController;
    [Range(1, 10)]
    public float Recoil_VisualImpulseControl;
    [Range(1, 200)]
    public float min_CrosshairSize;
    [Range(1, 200)]
    public float max_CrosshairSize;
    [Range(1, 10)]
    public float aimDownSight_speed; //1 = 5 sec,10 .5 sec
}
