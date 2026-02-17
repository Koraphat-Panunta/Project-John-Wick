using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatsScriptableObject", menuName = "ScriptableObjects/Weapon/WeaponStats")]
public class WeaponDataScriptableObject : ScriptableObject
{

    [SerializeField, ReadOnly]
    private string weaponID;

    public string WeaponID => weaponID;
    public Weapon weaponPrefab;

    public int bulletCapacity;
    [Range(1,2000)]
    public float rate_of_fire; //fire per 60 second
    [Range(0.1f,10)]
    public float reloadTime; 
    [Range(0,1)]
    public float Recovery_CrosshairBloomSpeed;
    [Range(0,1)]
    public float Recovery_CrosshairPositionSpeed;
    [Range(0, 1)]
    public float Recoil_CrosshairBloomController;
    [Range(0, 1)]
    public float Recoil_KickPositionPositionCrosshairController;
    [Range(0, 1)]
    public float Recoil_CameraControlController;
    [Range(0, 1)]
    public float Recoil_VisualImpulseControl;
    [Range(1, 200)]
    public float min_CrosshairSize;
    [Range(1, 200)]
    public float max_CrosshairSize;
    [Range(1, 10)]
    public float aimDownSight_speed; //1 = 5 sec,10 .5 sec

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(weaponID))
        {
            weaponID = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
