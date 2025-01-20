using UnityEngine;

public interface IGunFuAble
{
    public IWeaponAdvanceUser _weaponUser { get; set; }
    public bool _triggerGunFu { get; set; }
    public Transform _gunFuUserTransform { get; set; }
    public Transform _targetAdjustTranform { get; set; }
    public Vector3 _gunFuAimDir { get; }
    public float _limitAimAngleDegrees { get; set; }
    public float _shpere_Raduis_Detecion { get; set; }
    public float _sphere_Distance_Detection { get; set; }
    public LayerMask _layerTarget { get; set; }
    public void InitailizedGunFuComponent();
}
